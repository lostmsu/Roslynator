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
            Visibility visibility,
            SymbolDisplayContainingNamespaceStyle containingNamespaceStyle,
            in ProjectFilter projectFilter) : base(projectFilter)
        {
            Options = options;
            Depth = depth;
            Visibility = visibility;
            ContainingNamespaceStyle = containingNamespaceStyle;
        }

        public ListSymbolsCommandLineOptions Options { get; }

        public DefinitionListDepth Depth { get; }

        public Visibility Visibility { get; }

        public SymbolDisplayContainingNamespaceStyle ContainingNamespaceStyle { get; }

        public override async Task<CommandResult> ExecuteAsync(ProjectOrSolution projectOrSolution, CancellationToken cancellationToken = default)
        {
            AssemblyResolver.Register();

            var options = new DefinitionListOptions(
                visibility: Visibility,
                depth: Depth,
                containingNamespaceStyle: ContainingNamespaceStyle,
                ignoredNames: Options.IgnoredNames,
                indent: !Options.NoIndent,
                indentChars: Options.IndentChars,
                placeSystemNamespaceFirst: !Options.NoPrecedenceForSystem,
                nestNamespaces: Options.NestNamespaces,
                emptyLineBetweenMembers: Options.EmptyLineBetweenMembers,
                formatBaseList: Options.FormatBaseList,
                formatConstraints: Options.FormatConstraints,
                formatParameters: Options.FormatParameters,
                splitAttributes: !Options.MergeAttributes,
                includeAttributeArguments: !Options.NoAttributeArguments,
                omitIEnumerable: !Options.IncludeIEnumerable,
                assemblyAttributes: Options.AssemblyAttributes);

            ImmutableArray<Compilation> compilations = await GetCompilationsAsync(projectOrSolution, cancellationToken);

            string text = null;

            using (var writer = new StringWriter())
            {
                var builder = new DefinitionListWriter(
                    writer,
                    options: options,
                    comparer: SymbolDefinitionComparer.GetInstance(systemNamespaceFirst: !Options.NoPrecedenceForSystem));

                builder.Write(compilations.Select(f => f.Assembly));

                text = builder.ToString();
            }

            WriteLine(Verbosity.Minimal);
            WriteLine(text, Verbosity.Minimal);

            //TODO: write summary

            if (Options.Output != null)
                File.WriteAllText(Options.Output, text, Encoding.UTF8);

            return CommandResult.Success;
        }
    }
}
