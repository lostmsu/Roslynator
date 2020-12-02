﻿#nullable enable
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
            private TypeDeclarationSyntax? _existingDelegateContainer;
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
                    .Add(delegateProperty.Value.Property));

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
            var refactoring = new Refactoring();
            if (!await refactoring.Prepare(method, document, cancellationToken).ConfigureAwait(false))
                return document.Solution();

            if (!await refactoring.AddMethod(method).ConfigureAwait(false))
                return document.Solution();

            return refactoring.Complete();
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
            AttributeData? suppressSecurity = (suppressSecuritySymbol != null)
                ? methodSymbol.GetAttribute(suppressSecuritySymbol)
                : null;
            AttributeSyntax? suppressSecurityNode = (suppressSecurity != null)
                ? (AttributeSyntax)await suppressSecurity.ApplicationSyntaxReference.GetSyntaxAsync(cancellationToken).ConfigureAwait(false)
                : null;

            SyntaxTokenList modifiers = method.Modifiers.RemoveFirstUnchecked(SyntaxKind.ExternKeyword);
            SyntaxList<AttributeListSyntax> attributeLists = method.AttributeLists
                .Where(a => !ReferenceEquals(a, dllImportNode) && !ReferenceEquals(a, suppressSecurityNode),
                       out SyntaxTriviaList leftoverTrivia);

            var delegateType = MakeDelegateType(method, dllImportNode.ArgumentList!, suppressSecurityNode);
            return new DelegateProperty()
            {
                DelegateType = delegateType,
                Initializer = MakeInitializer(method, delegateType, dllImportNode),
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
                    type: delegateType,
                    explicitInterfaceSpecifier: default,
                    identifier: method.Identifier,
                    accessorList: AccessorList(AutoGetAccessorDeclaration())
                ),
            };
        }

        private static StatementSyntax MakeInitializer(
            MethodDeclarationSyntax method,
            TypeSyntax delegateType,
            AttributeSyntax dllImportNode)
        {
            ExpressionSyntax dllName = dllImportNode.ArgumentList.Arguments.Single(a => a.NameEquals == null).Expression;
            InvocationExpressionSyntax libraryHandle = InvocationExpression(_getUnmanagedDll, ArgumentList(Argument(dllName)));
            ExpressionSyntax functionName = dllImportNode.ArgumentList.Arguments
                .SingleOrDefault(a => a.NameEquals?.Name.Identifier.Text == nameof(DllImportAttribute.EntryPoint))
                ?.Expression ?? NameOfExpression(IdentifierName(method.Identifier));
            ExpressionSyntax functionPointer = InvocationExpression(_getFunctionByName, ArgumentList(
                    Argument(functionName),
                    Argument(libraryHandle)));
            var delegateForFunctionPointer = CastExpression(delegateType, functionPointer);

            return SimpleAssignmentStatement(
                DelegatePropertyName(method.Identifier),
                delegateForFunctionPointer);
        }

        private static TypeSyntax MakeDelegateType(
            MethodDeclarationSyntax method,
            AttributeArgumentListSyntax argumentList,
            AttributeSyntax? suppressSecurityNode)
        {
            ExpressionSyntax? callingConventionExpression = argumentList.Arguments
                .FirstOrDefault(arg => arg.NameEquals?.Name.Identifier.Text == nameof(DllImportAttribute.CallingConvention))
                ?.Expression;

            var callingConventionName = callingConventionExpression switch {
                IdentifierNameSyntax name => name,
                MemberAccessExpressionSyntax qualifiedName when qualifiedName.Name is IdentifierNameSyntax name => name,
                null => null,
                _ => throw new NotSupportedException(),
            };

            var callingConventionList = callingConventionName is null
                ? default
                : FunctionPointerUnmanagedCallingConventionList(
                    SingletonSeparatedList(
                        FunctionPointerUnmanagedCallingConvention(callingConventionName
                            .Identifier)));

            SyntaxList <AttributeListSyntax> attributes = default;
            if (suppressSecurityNode != null)
                attributes = attributes.Add(AttributeList(suppressSecurityNode));

            var pointerParameterTypes = method.ParameterList.Parameters.Select(p => FunctionPointerParameter(p.AttributeLists, p.Modifiers, p.Type));
            pointerParameterTypes = pointerParameterTypes.Append(FunctionPointerParameter(method.ReturnType));
            var callingConvention = FunctionPointerCallingConvention(Token(SyntaxKind.UnmanagedKeyword), callingConventionList);
            return FunctionPointerType(callingConvention, FunctionPointerParameterList(SeparatedList(pointerParameterTypes)));
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
            public TypeSyntax DelegateType { get; set; }
            public StatementSyntax Initializer { get; set; }
        }
    }
}