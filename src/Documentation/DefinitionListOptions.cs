// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Roslynator.Documentation
{
    //TODO: order by namespace, accessibility, typekind, memberkind, name
    internal class DefinitionListOptions
    {
        private static readonly ImmutableArray<Visibility> _allVisibilities = ImmutableArray.Create(Visibility.Public, Visibility.Internal, Visibility.Private);

        private readonly VisibilityFlags _visibilityFlags;

        public DefinitionListOptions(
            ImmutableArray<Visibility> visibilities = default,
            DefinitionListDepth depth = DefaultValues.Depth,
            IEnumerable<MetadataName> ignoredNames = null,
            IEnumerable<MetadataName> ignoredAttributeNames = null,
            string indentChars = DefaultValues.IndentChars,
            bool omitContainingNamespace = DefaultValues.OmitContainingNamespace,
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
            Visibilities = (!visibilities.IsDefault) ? visibilities : _allVisibilities;

            Depth = depth;
            IgnoredNames = ignoredNames?.ToImmutableArray() ?? ImmutableArray<MetadataName>.Empty;
            IgnoredAttributeNames = ignoredAttributeNames?.ToImmutableArray() ?? ImmutableArray<MetadataName>.Empty;
            IndentChars = indentChars;
            OmitContainingNamespace = omitContainingNamespace;
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

            foreach (Visibility visibility in Visibilities)
            {
                switch (visibility)
                {
                    case Visibility.Private:
                        {
                            _visibilityFlags |= VisibilityFlags.Private;
                            break;
                        }
                    case Visibility.Internal:
                        {
                            _visibilityFlags |= VisibilityFlags.Internal;
                            break;
                        }
                    case Visibility.Public:
                        {
                            _visibilityFlags |= VisibilityFlags.Public;
                            break;
                        }
                    default:
                        {
                            throw new ArgumentException("", nameof(visibilities));
                        }
                }
            }
        }

        public static DefinitionListOptions Default { get; } = new DefinitionListOptions();

        public ImmutableArray<Visibility> Visibilities { get; }

        public DefinitionListDepth Depth { get; }

        public ImmutableArray<MetadataName> IgnoredNames { get; }

        public ImmutableArray<MetadataName> IgnoredAttributeNames { get; }

        public string IndentChars { get; }

        public bool OmitContainingNamespace { get; }

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
            foreach (MetadataName metadataName in IgnoredNames)
            {
                if (symbol.HasMetadataName(metadataName))
                    return true;
            }

            return HasIgnoredAttribute(symbol);
        }

        internal bool HasIgnoredAttribute(ISymbol symbol)
        {
            if (symbol.Kind != SymbolKind.Namespace
                && IgnoredAttributeNames.Any())
            {
                foreach (AttributeData attribute in symbol.GetAttributes())
                {
                    foreach (MetadataName attributeName in IgnoredAttributeNames)
                    {
                        if (attribute.AttributeClass.HasMetadataName(attributeName))
                            return true;
                    }
                }
            }

            return false;
        }

        public bool IsVisible(ISymbol symbol)
        {
            switch (symbol.GetVisibility())
            {
                case Visibility.NotApplicable:
                    break;
                case Visibility.Private:
                    return (_visibilityFlags & VisibilityFlags.Private) != 0;
                case Visibility.Internal:
                    return (_visibilityFlags & VisibilityFlags.Internal) != 0;
                case Visibility.Public:
                    return (_visibilityFlags & VisibilityFlags.Public) != 0;
            }

            Debug.Fail(symbol.ToDisplayString());

            return false;
        }

        internal static class DefaultValues
        {
            public const Visibility Visibility = Roslynator.Visibility.Private;
            public const DefinitionListDepth Depth = DefinitionListDepth.Member;
            public const string IndentChars = "  ";
            public const bool OmitContainingNamespace = false;
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
