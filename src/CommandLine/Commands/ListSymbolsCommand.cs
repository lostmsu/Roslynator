// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Roslynator.Documentation;
using static Roslynator.Logger;

namespace Roslynator.CommandLine
{
    internal class ListSymbolsCommand : MSBuildWorkspaceCommand
    {
        public ListSymbolsCommand(
            ListSymbolsCommandLineOptions options,
            DefinitionListDepth depth,
            ImmutableArray<Visibility> visibilities,
            ImmutableArray<MetadataName> ignoredNames,
            ImmutableArray<MetadataName> ignoredAttributeNames,
            in ProjectFilter projectFilter) : base(projectFilter)
        {
            Options = options;
            Depth = depth;
            Visibilities = visibilities;
            IgnoredNames = ignoredNames;
            IgnoredAttributeNames = ignoredAttributeNames;
        }

        public ListSymbolsCommandLineOptions Options { get; }

        public DefinitionListDepth Depth { get; }

        public ImmutableArray<Visibility> Visibilities { get; }

        public ImmutableArray<MetadataName> IgnoredNames { get; }

        public ImmutableArray<MetadataName> IgnoredAttributeNames { get; }

        public override async Task<CommandResult> ExecuteAsync(ProjectOrSolution projectOrSolution, CancellationToken cancellationToken = default)
        {
            AssemblyResolver.Register();

            var options = new DefinitionListOptions(
                visibilities: Visibilities,
                depth: Depth,
                omitContainingNamespace: Options.OmitContainingNamespace,
                ignoredNames: IgnoredNames,
                ignoredAttributeNames: IgnoredAttributeNames,
                indentChars: Options.IndentChars,
                placeSystemNamespaceFirst: true,
                nestNamespaces: Options.NestNamespaces,
                emptyLineBetweenMembers: Options.EmptyLineBetweenMembers,
                formatBaseList: Options.FormatBaseList,
                formatConstraints: Options.FormatConstraints,
                formatParameters: Options.FormatParameters,
                splitAttributes: true,
                includeAttributeArguments: !Options.NoAttributeArguments,
                omitIEnumerable: true,
                assemblyAttributes: Options.AssemblyAttributes);

            ImmutableArray<Compilation> compilations = await GetCompilationsAsync(projectOrSolution, cancellationToken);

            string text = null;

            using (var writer = new StringWriter())
            {
                var builder = new DefinitionListWriter(
                    writer,
                    options: options,
                    comparer: SymbolDefinitionComparer.SystemNamespaceFirstInstance);

                builder.Write(compilations.Select(f => f.Assembly));

                text = builder.ToString();
            }

            WriteLine(Verbosity.Minimal);
            WriteLine(text, Verbosity.Minimal);

            //TODO: Summary

            if (Options.Output != null)
                File.WriteAllText(Options.Output, text, Encoding.UTF8);

            return CommandResult.Success;
        }
    }
}
