﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp
{
    /// <summary>
    /// Represents a comparer for member declarations.
    /// </summary>
    public abstract class MemberDeclarationComparer : IComparer<MemberDeclarationSyntax>, IEqualityComparer<MemberDeclarationSyntax>
    {
        internal static MemberDeclarationComparer ByKind { get; } = new MemberDeclarationComparer(MemberDeclarationSortMode.ByKind);

        internal static MemberDeclarationComparer ByKindThenByName { get; } = new MemberDeclarationComparer(MemberDeclarationSortMode.ByKindThenByName);


        /// <summary>
        /// Compares two member declarations and returns a value indicating whether one should be before,
        /// at the same position, or after the other.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public abstract int Compare(MemberDeclarationSyntax x, MemberDeclarationSyntax y);

        public abstract bool Equals(MemberDeclarationSyntax x, MemberDeclarationSyntax y);

        public abstract int GetHashCode(MemberDeclarationSyntax obj);

        internal static bool CanBeSortedByName(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.EventDeclaration:
                case SyntaxKind.EventFieldDeclaration:
                case SyntaxKind.PropertyDeclaration:
                case SyntaxKind.MethodDeclaration:
                case SyntaxKind.EnumDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.NamespaceDeclaration:
                    return true;
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.IndexerDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.IncompleteMember:
                    return false;
                default:
                    {
                        Debug.Fail($"unknown member '{kind}'");
                        return false;
                    }
            }
        }
    }
}
