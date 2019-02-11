// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Roslynator.Documentation
{
    //TODO: order by namespace, accessibility, typekind, memberkind, name
    internal class DefinitionListOptions
    {
        private readonly ImmutableArray<MetadataName> _ignoredMetadataNames;

        public DefinitionListOptions(
            Visibility visibility = DefaultValues.Visibility,
            DefinitionListDepth depth = DefaultValues.Depth,
            SymbolDisplayContainingNamespaceStyle containingNamespaceStyle = DefaultValues.ContainingNamespaceStyle,
            IEnumerable<string> ignoredNames = null,
            bool indent = DefaultValues.Indent,
            string indentChars = DefaultValues.IndentChars,
            bool placeSystemNamespaceFirst = DefaultValues.PlaceSystemNamespaceFirst,
            bool nestNamespaces = DefaultValues.NestNamespaces,
            bool emptyLineBetweenMembers = DefaultValues.EmptyLineBetweenMembers,
            bool formatBaseList = DefaultValues.FormatBaseList,
            bool formatConstraints = DefaultValues.FormatConstraints,
            bool formatParameters = DefaultValues.FormatParameters,
            bool splitAttributes = DefaultValues.SplitAttributes,
            bool includeAttributeArguments = DefaultValues.IncludeAttributeArguments,
            bool omitIEnumerable = DefaultValues.OmitIEnumerable,
            bool useDefaultLiteral = DefaultValues.UseDefaultLiteral,
            bool assemblyAttributes = DefaultValues.AssemblyAttributes)
        {
            _ignoredMetadataNames = ignoredNames?.Select(name => MetadataName.Parse(name)).ToImmutableArray() ?? ImmutableArray<MetadataName>.Empty;

            Visibility = visibility;
            Depth = depth;
            ContainingNamespaceStyle = containingNamespaceStyle;
            IgnoredNames = ignoredNames?.ToImmutableArray() ?? ImmutableArray<string>.Empty;
            Indent = indent;
            IndentChars = indentChars;
            PlaceSystemNamespaceFirst = placeSystemNamespaceFirst;
            NestNamespaces = nestNamespaces;
            EmptyLineBetweenMembers = emptyLineBetweenMembers;
            FormatBaseList = formatBaseList;
            FormatConstraints = formatConstraints;
            FormatParameters = formatParameters;
            SplitAttributes = splitAttributes;
            IncludeAttributeArguments = includeAttributeArguments;
            OmitIEnumerable = omitIEnumerable;
            UseDefaultLiteral = useDefaultLiteral;
            AssemblyAttributes = assemblyAttributes;
        }

        public static DefinitionListOptions Default { get; } = new DefinitionListOptions();

        public Visibility Visibility { get; }

        public DefinitionListDepth Depth { get; }

        public SymbolDisplayContainingNamespaceStyle ContainingNamespaceStyle { get; }

        public ImmutableArray<string> IgnoredNames { get; }

        public bool Indent { get; }

        public string IndentChars { get; }

        public bool PlaceSystemNamespaceFirst { get; }

        public bool NestNamespaces { get; }

        public bool EmptyLineBetweenMembers { get; }

        public bool FormatBaseList { get; }

        public bool FormatConstraints { get; }

        public bool FormatParameters { get; }

        public bool SplitAttributes { get; }

        public bool IncludeAttributeArguments { get; }

        public bool OmitIEnumerable { get; }

        public bool UseDefaultLiteral { get; }

        public bool AssemblyAttributes { get; }

        internal bool ShouldBeIgnored(ISymbol symbol)
        {
            foreach (MetadataName metadataName in _ignoredMetadataNames)
            {
                if (symbol.HasMetadataName(metadataName))
                    return true;
            }

            return false;
        }

        internal static class DefaultValues
        {
            public const Visibility Visibility = Roslynator.Visibility.Private;
            public const DefinitionListDepth Depth = DefinitionListDepth.Member;
            public const SymbolDisplayContainingNamespaceStyle ContainingNamespaceStyle = SymbolDisplayContainingNamespaceStyle.Omitted;
            public const bool Indent = true;
            public const string IndentChars = "  ";
            public const bool PlaceSystemNamespaceFirst = true;
            public const bool NestNamespaces = false;
            public const bool EmptyLineBetweenMembers = false;
            public const bool FormatBaseList = false;
            public const bool FormatConstraints = false;
            public const bool FormatParameters = false;
            public const bool SplitAttributes = true;
            public const bool IncludeAttributeArguments = true;
            public const bool OmitIEnumerable = true;
            public const bool UseDefaultLiteral = true;
            public const bool AssemblyAttributes = false;
        }
    }
}
