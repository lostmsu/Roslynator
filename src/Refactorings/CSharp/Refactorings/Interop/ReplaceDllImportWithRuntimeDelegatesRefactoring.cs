using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Roslynator.CSharp.CSharpFactory;

namespace Roslynator.CSharp.Refactorings.Interop {
    internal static class ReplaceDllImportWithRuntimeDelegatesRefactoring
    {
        private static readonly string _dllImportFullName = typeof(DllImportAttribute).FullName;
        private static readonly NameSyntax _functionPointerFullName = ParseName($"global::{typeof(UnmanagedFunctionPointerAttribute).FullName}");
        private static readonly NameSyntax _defaultCallingConvention = ParseName($"global::{typeof(CallingConvention).FullName}.{nameof(CallingConvention.Winapi)}");
        private const string SuppressSecurityFullName = "System.Security.SuppressUnmanagedCodeSecurityAttribute";
        private static readonly NameSyntax _marshal = ParseName($"global::{typeof(Marshal).FullName}");
        private static readonly NameSyntax _intPtr = ParseName($"global::{typeof(IntPtr).FullName}");

        private static readonly IdentifierNameSyntax _delegatesClassIdentifier = IdentifierName("Delegates");
        private static readonly IdentifierNameSyntax _getUnmanagedDll = IdentifierName("GetUnmanagedDll");
        private static readonly IdentifierNameSyntax _getFunctionByName = IdentifierName("GetFunctionByName");

        private static readonly BlockSyntax _notImplementedBlock = Block(ThrowNewStatement(CSharpTypeFactory.NotImplementedException()));

        private static readonly ClassDeclarationSyntax _emptyDelegateContainer =
            ClassDeclaration(Modifiers.Private_Static(),
                _delegatesClassIdentifier.Identifier,
                members: List(new MemberDeclarationSyntax[] {
                    ConstructorDeclaration(_delegatesClassIdentifier.Identifier)
                        .WithModifiers(Modifiers.Static())
                        .WithBody(Block()),
                    MethodDeclaration(
                        Modifiers.Static(),
                        returnType: _intPtr,
                        identifier: _getUnmanagedDll.Identifier,
                        parameterList: ParameterList(Parameter(PredefinedStringType(), "libraryName")),
                        body: _notImplementedBlock
                    ),
                    MethodDeclaration(
                        Modifiers.Static(),
                        returnType: _intPtr,
                        identifier: _getFunctionByName.Identifier,
                        parameterList: ParameterList(
                            Parameter(PredefinedStringType(), "functionName"),
                            Parameter(_intPtr, "libraryHandle")
                        ),
                        body: _notImplementedBlock
                    ),
                }));

        public static bool CanRefactor(MethodDeclarationSyntax methodDeclaration)
        {
            return methodDeclaration.Modifiers.Any(SyntaxKind.ExternKeyword)
                && (methodDeclaration.Parent is ClassDeclarationSyntax
                 || methodDeclaration.Parent is StructDeclarationSyntax);
        }

        private class Refactoring
        {
            private CancellationToken _cancellationToken;
            private Document _document;
            private SyntaxNode _root;
            private SemanticModel _semanticModel;
            private TypeDeclarationSyntax _newParent;
            private TypeDeclarationSyntax _existingDelegateContainer;
            private TypeDeclarationSyntax _newDelegateContainer;
            private ConstructorDeclarationSyntax _initialContainerConstructor;
            private ConstructorDeclarationSyntax _newContainerConstructor;
            private readonly Dictionary<MemberDeclarationSyntax, MemberDeclarationSyntax> _replacements = new Dictionary<MemberDeclarationSyntax, MemberDeclarationSyntax>();
            public TypeDeclarationSyntax OriginalParent { get; private set; }

            public async Task<bool> Prepare(MethodDeclarationSyntax method, Document document, CancellationToken cancellationToken)
            {
                if (this._root != null)
                    throw new InvalidOperationException();

                this._cancellationToken = cancellationToken;
                this._document = document;
                this._root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
                this._semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                this.OriginalParent = method.Parent as TypeDeclarationSyntax;
                this._newParent = this.OriginalParent;
                this._existingDelegateContainer = this.OriginalParent.Members.OfType<TypeDeclarationSyntax>()
                    .FirstOrDefault(t => t.Identifier.Text == _delegatesClassIdentifier.Identifier.Text);
                this._newDelegateContainer = this._existingDelegateContainer ?? _emptyDelegateContainer;

                this._initialContainerConstructor = this._newDelegateContainer.Members
                    .OfType<ConstructorDeclarationSyntax>()
                    .FirstOrDefault(ctor => ctor.Modifiers.Any(SyntaxKind.StaticKeyword));
                this._newContainerConstructor = this._initialContainerConstructor;

                return this._initialContainerConstructor?.Body != null;
            }

            public async Task<bool> AddMethod(MethodDeclarationSyntax method)
            {
                DelegateProperty? delegateProperty = await ConvertToPropertyAsync(method, this._semanticModel, this._cancellationToken).ConfigureAwait(false);
                if (delegateProperty == null)
                    return false;

                this._newContainerConstructor = this._newContainerConstructor
                    .AddBodyStatements(delegateProperty.Value.Initializer);
                this._newDelegateContainer = this._newDelegateContainer.WithMembers(this._newDelegateContainer.Members
                    .Add(delegateProperty.Value.Property)
                    .Add(delegateProperty.Value.DelegateType));

                this._replacements[method] = delegateProperty.Value.Method;

                return true;
            }

            public Solution Complete()
            {
                if (this._newContainerConstructor != this._initialContainerConstructor)
                {
                    // reacquire initial constructor
                    this._initialContainerConstructor = this._newDelegateContainer.Members
                        .OfType<ConstructorDeclarationSyntax>()
                        .First(ctor => ctor.Modifiers.Any(SyntaxKind.StaticKeyword));
                    this._newDelegateContainer = this._newDelegateContainer
                       .ReplaceNode(this._initialContainerConstructor, this._newContainerConstructor);
                }

                if (this._existingDelegateContainer != null)
                    this._replacements[this._existingDelegateContainer] = this._newDelegateContainer;

                this._newParent = this._newParent.ReplaceNodes(this._replacements.Keys, (orig, _) => this._replacements[orig]);
                this._newParent = (this._existingDelegateContainer == null)
                    ? this._newParent.WithMembers(this._newParent.Members.Add(this._newDelegateContainer))
                    : this._newParent;

                SyntaxNode newRoot = this._root.ReplaceNode(this.OriginalParent, this._newParent);
                Solution solution = this._document.Solution();
                return solution.WithDocumentSyntaxRoot(this._document.Id, newRoot);
            }
        }

        public static async Task<Solution> RefactorAsync(
            Document document,
            MethodDeclarationSyntax method,
            CancellationToken cancellationToken = default)
        {
            var refacotring = new Refactoring();
            if (!await refacotring.Prepare(method, document, cancellationToken).ConfigureAwait(false))
                return document.Solution();

            if (!await refacotring.AddMethod(method).ConfigureAwait(false))
                return document.Solution();

            return refacotring.Complete();
        }

        public static async Task<Solution> RefactorAllAsync(
            Document document,
            MethodDeclarationSyntax method,
            CancellationToken cancellationToken = default)
        {
            var refacotring = new Refactoring();
            if (!await refacotring.Prepare(method, document, cancellationToken).ConfigureAwait(false))
                return document.Solution();

            foreach (MethodDeclarationSyntax externMethod in refacotring.OriginalParent.Members
                .OfType<MethodDeclarationSyntax>().Where(m => m.Modifiers.Any(SyntaxKind.ExternKeyword)))
            {
                await refacotring.AddMethod(externMethod).ConfigureAwait(false);
            }

            return refacotring.Complete();
        }

        private static async Task<DelegateProperty?> ConvertToPropertyAsync(
            MethodDeclarationSyntax method,
            SemanticModel semanticModel,
            CancellationToken cancellationToken)
        {
            IMethodSymbol methodSymbol = semanticModel.GetDeclaredSymbol(method, cancellationToken);
            INamedTypeSymbol dllImportSymbol = semanticModel.GetTypeByMetadataName(_dllImportFullName);
            AttributeData dllImport = methodSymbol.GetAttribute(dllImportSymbol);
            if (dllImport == null
                || dllImport.NamedArguments.Any(arg => arg.Key != nameof(DllImportAttribute.CallingConvention)
                                                    && arg.Key != nameof(DllImportAttribute.EntryPoint)
                                                    && arg.Key != nameof(DllImportAttribute.CharSet))
                || dllImport.ConstructorArguments.Length != 1)
            {
                return null;
            }

            var dllImportNode = (AttributeSyntax)await dllImport.ApplicationSyntaxReference.GetSyntaxAsync(cancellationToken).ConfigureAwait(false);
            INamedTypeSymbol suppressSecuritySymbol = semanticModel.GetTypeByMetadataName(SuppressSecurityFullName);
            AttributeData suppressSecurity = (suppressSecuritySymbol != null)
                ? methodSymbol.GetAttribute(suppressSecuritySymbol)
                : null;
            AttributeSyntax suppressSecurityNode = (suppressSecurity != null)
                ? (AttributeSyntax)await suppressSecurity.ApplicationSyntaxReference.GetSyntaxAsync(cancellationToken).ConfigureAwait(false)
                : null;

            SyntaxTokenList modifiers = method.Modifiers.RemoveFirstUnchecked(SyntaxKind.ExternKeyword);
            SyntaxList<AttributeListSyntax> attributeLists = method.AttributeLists
                .Where(a => !ReferenceEquals(a, dllImportNode) && !ReferenceEquals(a, suppressSecurityNode),
                       out SyntaxTriviaList leftoverTrivia);

            return new DelegateProperty()
            {
                DelegateType = MakeDelegateType(method, dllImportNode, suppressSecurityNode),
                Initializer = MakeInitializer(method, dllImportNode),
                Method = method
                    .WithAttributeLists(attributeLists)
                    .WithExpressionBody(ArrowExpressionClause(
                        InvocationExpression(
                            QualifiedDelegatePropertyName(method.Identifier),
                            method.ParameterList.PassAsArguments())))
                    .WithModifiers(modifiers)
                    .PrependToLeadingTrivia(leftoverTrivia),
                Property = PropertyDeclaration(
                    attributeLists: default,
                    modifiers: Modifiers.Internal_Static(),
                    type: DelegateType(method.Identifier),
                    explicitInterfaceSpecifier: default,
                    identifier: method.Identifier,
                    accessorList: AccessorList(AutoGetAccessorDeclaration())
                ),
            };
        }

        private static StatementSyntax MakeInitializer(
            MethodDeclarationSyntax method,
            AttributeSyntax dllImportNode)
        {
            ExpressionSyntax dllName = dllImportNode.ArgumentList.Arguments.Single(a => a.NameEquals == null).Expression;
            TypeSyntax delegateType = DelegateType(method.Identifier);
            InvocationExpressionSyntax libraryHandle = InvocationExpression(_getUnmanagedDll, ArgumentList(Argument(dllName)));
            ExpressionSyntax functionName = dllImportNode.ArgumentList.Arguments
                .SingleOrDefault(a => a.NameEquals?.Name.Identifier.Text == nameof(DllImportAttribute.EntryPoint))
                ?.Expression ?? NameOfExpression(IdentifierName(method.Identifier));
            ExpressionSyntax functionPointer = InvocationExpression(_getFunctionByName, ArgumentList(
                    Argument(functionName),
                    Argument(libraryHandle)));
            InvocationExpressionSyntax delegateForFunctionPointer = SimpleMemberInvocationExpression(
                _marshal,
                GenericName(nameof(Marshal.GetDelegateForFunctionPointer), delegateType),
                Argument(functionPointer));

            return SimpleAssignmentStatement(
                DelegatePropertyName(method.Identifier),
                delegateForFunctionPointer);
        }

        private static DelegateDeclarationSyntax MakeDelegateType(
            MethodDeclarationSyntax method,
            AttributeSyntax dllImportNode,
            AttributeSyntax suppressSecurityNode)
        {
            ExpressionSyntax callingConvention = dllImportNode.ArgumentList.Arguments
                .FirstOrDefault(arg => arg.NameEquals?.Name.Identifier.Text == nameof(DllImportAttribute.CallingConvention))
                ?.Expression ?? _defaultCallingConvention;
            var unmanagedPointerAttribute = Attribute(_functionPointerFullName, AttributeArgument(callingConvention));

            foreach (var argument in dllImportNode.ArgumentList.Arguments) {
                string name = argument.NameEquals?.Name.Identifier.ValueText;
                switch (name) {
                case nameof(DllImportAttribute.CallingConvention):
                case null:
                    break;
                default:
                    unmanagedPointerAttribute = unmanagedPointerAttribute.AddArgumentListArguments(argument);
                    break;
                }
            }

            SyntaxList<AttributeListSyntax> attributes = SingletonList(AttributeList(unmanagedPointerAttribute));
            if (suppressSecurityNode != null)
                attributes = attributes.Add(AttributeList(suppressSecurityNode));

            return DelegateDeclaration(
                attributes,
                Modifiers.Public(),
                method.ReturnType,
                DelegateTypeName(method.Identifier).Identifier,
                typeParameterList: default,
                parameterList: method.ParameterList,
                constraintClauses: default
            );
        }

        private static TypeSyntax QualifiedDelegatePropertyName(in SyntaxToken methodIdentifier)
            => QualifiedName(_delegatesClassIdentifier, DelegatePropertyName(methodIdentifier));

        private static IdentifierNameSyntax DelegatePropertyName(in SyntaxToken methodIdentifier) => IdentifierName(methodIdentifier);
        private static TypeSyntax DelegateType(in SyntaxToken methodIdentifier)
            => DelegateTypeName(methodIdentifier);

        private static IdentifierNameSyntax DelegateTypeName(in SyntaxToken methodIdentifier)
            => IdentifierName(methodIdentifier.Text + "Delegate");

        private struct DelegateProperty
        {
            public MethodDeclarationSyntax Method { get; set; }
            public PropertyDeclarationSyntax Property { get; set; }
            public DelegateDeclarationSyntax DelegateType { get; set; }
            public StatementSyntax Initializer { get; set; }
        }
    }
}
