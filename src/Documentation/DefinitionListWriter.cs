// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Roslynator.Documentation
{
    internal class DefinitionListWriter
    {
        private bool _pendingIndentation;
        private int _indentationLevel;

        public DefinitionListWriter(
            TextWriter writer,
            DefinitionListOptions options = null,
            IComparer<ISymbol> comparer = null)
        {
            Writer = writer;
            Options = options ?? DefinitionListOptions.Default;
            Comparer = comparer ?? SymbolDefinitionComparer.Instance;
        }

        public TextWriter Writer { get; }

        public DefinitionListOptions Options { get; }

        public IComparer<ISymbol> Comparer { get; }

        public virtual bool IsVisibleNamespace(INamespaceSymbol namespaceSymbol)
        {
            return !Options.ShouldBeIgnored(namespaceSymbol);
        }

        public virtual bool IsVisibleType(INamedTypeSymbol typeSymbol)
        {
            return !typeSymbol.IsImplicitlyDeclared
                && Options.IsVisible(typeSymbol)
                && !Options.ShouldBeIgnored(typeSymbol);
        }

        public virtual bool IsVisibleMember(ISymbol symbol)
        {
            bool canBeImplicitlyDeclared = false;

            switch (symbol.Kind)
            {
                case SymbolKind.Event:
                case SymbolKind.Field:
                case SymbolKind.Property:
                    {
                        break;
                    }
                case SymbolKind.Method:
                    {
                        var methodSymbol = (IMethodSymbol)symbol;

                        switch (methodSymbol.MethodKind)
                        {
                            case MethodKind.Constructor:
                                {
                                    TypeKind typeKind = methodSymbol.ContainingType.TypeKind;

                                    Debug.Assert(typeKind == TypeKind.Class || typeKind == TypeKind.Struct, typeKind.ToString());

                                    if (typeKind == TypeKind.Class)
                                    {
                                        if (!methodSymbol.Parameters.Any())
                                            canBeImplicitlyDeclared = true;
                                    }
                                    else if (typeKind == TypeKind.Struct)
                                    {
                                        if (!methodSymbol.Parameters.Any())
                                            return false;
                                    }

                                    break;
                                }
                            case MethodKind.Conversion:
                            case MethodKind.UserDefinedOperator:
                            case MethodKind.Ordinary:
                                break;
                            case MethodKind.ExplicitInterfaceImplementation:
                            case MethodKind.StaticConstructor:
                            case MethodKind.Destructor:
                            case MethodKind.EventAdd:
                            case MethodKind.EventRaise:
                            case MethodKind.EventRemove:
                            case MethodKind.PropertyGet:
                            case MethodKind.PropertySet:
                                return false;
                            default:
                                {
                                    Debug.Fail(methodSymbol.MethodKind.ToString());
                                    return false;
                                }
                        }

                        break;
                    }
                default:
                    {
                        Debug.Assert(symbol.Kind == SymbolKind.NamedType, symbol.Kind.ToString());
                        return false;
                    }
            }

            return (canBeImplicitlyDeclared || !symbol.IsImplicitlyDeclared)
                && Options.IsVisible(symbol)
                && !Options.HasIgnoredAttribute(symbol);
        }

        public virtual bool IsVisibleAttribute(INamedTypeSymbol attributeType)
        {
            return AttributeDisplay.ShouldBeDisplayed(attributeType);
        }

        public void Write(IEnumerable<IAssemblySymbol> assemblies)
        {
            using (IEnumerator<IAssemblySymbol> en = assemblies
                .OrderBy(f => f.Name)
                .ThenBy(f => f.Identity.Version).GetEnumerator())
            {
                if (en.MoveNext())
                {
                    while (true)
                    {
                        WriteAssembly(en.Current);

                        if (!en.MoveNext())
                            break;

                        if (Options.AssemblyAttributes)
                            WriteLine();
                    }
                }
            }

            IEnumerable<IGrouping<INamespaceSymbol, INamedTypeSymbol>> typesByNamespace = assemblies
                .SelectMany(a => a.GetTypes(t => t.ContainingType == null && IsVisibleType(t)))
                .GroupBy(t => t.ContainingNamespace, MetadataNameEqualityComparer<INamespaceSymbol>.Instance)
                .Where(g => IsVisibleNamespace(g.Key))
                .OrderBy(g => g.Key, Comparer);

            if (Options.NestNamespaces)
            {
                WriteWithNamespaceHierarchy(typesByNamespace.ToDictionary(f => f.Key, f => f.AsEnumerable()));
            }
            else
            {
                foreach (IGrouping<INamespaceSymbol, INamedTypeSymbol> grouping in typesByNamespace)
                {
                    INamespaceSymbol namespaceSymbol = grouping.Key;

                    WriteStartNamespace(namespaceSymbol);

                    if (Options.Depth <= DefinitionListDepth.Type)
                        WriteTypes(grouping);

                    WriteEndNamespace(namespaceSymbol);
                }
            }
        }

        private void WriteAssembly(IAssemblySymbol assembly)
        {
            Write("assembly ");
            WriteLine(assembly.Identity.ToString());

            if (Options.AssemblyAttributes)
            {
                ImmutableArray<SymbolDisplayPart> attributeParts = SymbolDefinitionDisplay.GetAttributesParts(
                    assembly,
                    IsVisibleAttribute,
                    containingNamespaceStyle: (Options.OmitContainingNamespace) ?  SymbolDisplayContainingNamespaceStyle.Omitted : SymbolDisplayContainingNamespaceStyle.Included,
                    splitAttributes: Options.SplitAttributes,
                    includeAttributeArguments: Options.IncludeAttributeArguments);

                if (attributeParts.Any())
                    Write(attributeParts);
            }
        }

        private void WriteWithNamespaceHierarchy(Dictionary<INamespaceSymbol, IEnumerable<INamedTypeSymbol>> typesByNamespace)
        {
            var rootNamespaces = new HashSet<INamespaceSymbol>(MetadataNameEqualityComparer<INamespaceSymbol>.Instance);

            var nestedNamespaces = new HashSet<INamespaceSymbol>(MetadataNameEqualityComparer<INamespaceSymbol>.Instance);

            foreach (INamespaceSymbol namespaceSymbol in typesByNamespace.Select(f => f.Key))
            {
                if (namespaceSymbol.IsGlobalNamespace)
                {
                    rootNamespaces.Add(namespaceSymbol);
                }
                else
                {
                    INamespaceSymbol n = namespaceSymbol;

                    while (true)
                    {
                        INamespaceSymbol containingNamespace = n.ContainingNamespace;

                        if (containingNamespace.IsGlobalNamespace)
                        {
                            rootNamespaces.Add(n);
                            break;
                        }

                        nestedNamespaces.Add(n);

                        n = containingNamespace;
                    }
                }
            }

            foreach (INamespaceSymbol namespaceSymbol in rootNamespaces.OrderBy(f => f, Comparer))
            {
                WriteNamespace(namespaceSymbol);
            }

            void WriteNamespace(INamespaceSymbol namespaceSymbol)
            {
                WriteStartNamespace(namespaceSymbol);

                if (Options.Depth <= DefinitionListDepth.Type)
                    WriteTypes(typesByNamespace[namespaceSymbol]);

                foreach (INamespaceSymbol nestedNamespace in nestedNamespaces
                    .Where(f => MetadataNameEqualityComparer<INamespaceSymbol>.Instance.Equals(f.ContainingNamespace, namespaceSymbol))
                    .Distinct(MetadataNameEqualityComparer<INamespaceSymbol>.Instance)
                    .OrderBy(f => f, Comparer)
                    .ToArray())
                {
                    nestedNamespaces.Remove(nestedNamespace);

                    WriteNamespace(nestedNamespace);
                }

                WriteEndNamespace(namespaceSymbol);
            }
        }

        private void WriteTypes(IEnumerable<INamedTypeSymbol> types)
        {
            using (IEnumerator<INamedTypeSymbol> en = types
                .OrderBy(f => f, Comparer).GetEnumerator())
            {
                if (en.MoveNext())
                {
                    WriteLine();

                    while (true)
                    {
                        INamedTypeSymbol namedType = en.Current;

                        WriteStartNamedType(namedType);

                        switch (namedType.TypeKind)
                        {
                            case TypeKind.Class:
                            case TypeKind.Interface:
                            case TypeKind.Struct:
                                {
                                    WriteMembers(namedType);
                                    break;
                                }
                            case TypeKind.Enum:
                                {
                                    WriteEnumMembers(namedType);
                                    break;
                                }
                            default:
                                {
                                    Debug.Assert(namedType.TypeKind == TypeKind.Delegate, namedType.TypeKind.ToString());
                                    break;
                                }
                        }

                        WriteEndNamedType(namedType);

                        if (en.MoveNext())
                        {
                            WriteLine();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void WriteStartNamespace(INamespaceSymbol namespaceSymbol)
        {
            if (namespaceSymbol.IsGlobalNamespace)
                return;

            WriteLine();
            Write(namespaceSymbol, SymbolDefinitionDisplayFormats.NamespaceDefinition);
            WriteLine();

            _indentationLevel++;
        }

        private void WriteEndNamespace(INamespaceSymbol namespaceSymbol)
        {
            if (namespaceSymbol.IsGlobalNamespace)
                return;

            DecreaseIndentation();
        }

        private void WriteStartNamedType(INamedTypeSymbol namedType)
        {
            Write(namedType, SymbolDefinitionDisplayFormats.FullDefinition_NameOnly);

            TypeKind typeKind = namedType.TypeKind;

            if (namedType.TypeKind != TypeKind.Delegate)
            {
                WriteLine();

                _indentationLevel++;
            }
        }

        private void WriteEndNamedType(INamedTypeSymbol namedType)
        {
            if (namedType.TypeKind != TypeKind.Delegate)
            {
                DecreaseIndentation();
            }
        }

        private void WriteMembers(INamedTypeSymbol namedType)
        {
            if (Options.Depth == DefinitionListDepth.Member)
            {
                using (IEnumerator<ISymbol> en = namedType.GetMembers()
                    .Where(f => IsVisibleMember(f))
                    .OrderBy(f => f, Comparer)
                    .GetEnumerator())
                {
                    if (en.MoveNext())
                    {
                        WriteLine();

                        MemberDeclarationKind kind = en.Current.GetMemberDeclarationKind();

                        while (true)
                        {
                            SymbolDisplayFormat format = (Options.OmitContainingNamespace)
                                ? SymbolDefinitionDisplayFormats.FullDefinition_NameAndContainingTypes
                                : SymbolDefinitionDisplayFormats.FullDefinition_NameAndContainingTypesAndNamespaces;

                            Write(en.Current, format);

                            WriteLine();

                            if (en.MoveNext())
                            {
                                MemberDeclarationKind kind2 = en.Current.GetMemberDeclarationKind();

                                if (kind != kind2
                                    || Options.EmptyLineBetweenMembers)
                                {
                                    WriteLine();
                                }

                                kind = kind2;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (Options.Depth <= DefinitionListDepth.Type)
                WriteTypes(namedType.GetTypeMembers().Where(f => IsVisibleType(f)));
        }

        private void WriteEnumMembers(INamedTypeSymbol enumType)
        {
            if (Options.Depth != DefinitionListDepth.Member)
                return;

            using (IEnumerator<ISymbol> en = enumType
                .GetMembers()
                .Where(m => m.Kind == SymbolKind.Field
                    && m.DeclaredAccessibility == Accessibility.Public)
                .GetEnumerator())
            {
                if (en.MoveNext())
                {
                    WriteLine();

                    do
                    {
                        Write(en.Current, SymbolDefinitionDisplayFormats.FullDefinition_NameOnly);
                        WriteLine();
                    }
                    while (en.MoveNext());
                }
            }
        }

        private void Write(ISymbol symbol, SymbolDisplayFormat format)
        {
            ImmutableArray<SymbolDisplayPart> parts = SymbolDefinitionDisplay.GetDisplayParts(
                symbol,
                format,
                SymbolDisplayTypeDeclarationOptions.IncludeAccessibility | SymbolDisplayTypeDeclarationOptions.IncludeModifiers,
                containingNamespaceStyle: (Options.OmitContainingNamespace) ? SymbolDisplayContainingNamespaceStyle.Omitted : SymbolDisplayContainingNamespaceStyle.Included,
                isVisibleAttribute: IsVisibleAttribute,
                formatBaseList: Options.FormatBaseList,
                formatConstraints: Options.FormatConstraints,
                formatParameters: Options.FormatParameters,
                splitAttributes: Options.SplitAttributes,
                includeAttributeArguments: Options.IncludeAttributeArguments,
                omitIEnumerable: Options.OmitIEnumerable,
                useDefaultLiteral: Options.UseDefaultLiteral,
                addAccessorAttributes: true,
                addParameterAttributes: true);

            Write(parts);
        }

        private void Write(ImmutableArray<SymbolDisplayPart> parts)
        {
            foreach (SymbolDisplayPart part in parts)
            {
                Write(part.ToString());

                if (part.Kind == SymbolDisplayPartKind.LineBreak)
                    _pendingIndentation = true;
            }
        }

        public void Write(string value)
        {
            CheckPendingIndentation();
            Writer.Write(value);
        }

        public void WriteLine()
        {
            Writer.WriteLine();

            _pendingIndentation = true;
        }

        public void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteIndentation()
        {
            for (int i = 0; i < _indentationLevel; i++)
            {
                Write(Options.IndentChars);
            }
        }

        private void CheckPendingIndentation()
        {
            if (_pendingIndentation)
            {
                _pendingIndentation = false;
                WriteIndentation();
            }
        }

        public override string ToString()
        {
            return Writer.ToString();
        }

        private void DecreaseIndentation()
        {
            Debug.Assert(_indentationLevel > 0, "Cannot decrease indentation level.");
            _indentationLevel--;
        }
    }
}
