
# `list-symbols` Command

List symbol definitions from the specified project or solution.

## Synopsis

```
roslynator list-symbols <PROJECT|SOLUTION>
[--assembly-attributes]
[--containing-namespace-style]
[--depth]
[--empty-line-between-members]
[--file-log]
[--file-log-verbosity]
[--format-base-list]
[--format-constraints]
[--format-parameters]
[--ignored-names]
[--ignored-projects]
[--include-ienumerable]
[--indent-chars]
[--language]
[--merge-attributes]
[--msbuild-path]
[--nest-namespaces]
[--no-attribute-arguments]
[--no-indent]
[--no-precedence-for-system]
[--output]
[--projects]
[-p|--properties]
[-v|--verbosity]
[--visibility]
```

## Arguments

**`PROJECT|SOLUTION`**

The project or solution to analyze.

### Optional Options

**`[--assembly-attributes]`**

Indicates whether assembly attributes should be displayed.

**`[--containing-namespace-style]`** `<CONTAINING-NAMESPACE-STYLE>`

Defines how a containing namespace of a symbol is displayed. Allowed values are omitted, omitted-as-containing or included. Default value is omitted.

**`[--depth]`** `{member|type|namespace}`

Defines a depth of a documentation. Default value is `member`.

**`[--empty-line-between-members]`**

Indicates whether an empty line should be added between two member declarations.

**`[--format-base-list]`**

Indicates whether a base list should be formatted on a multiple lines.

**`[--format-constraints]`**

Indicates whether constraints should be formatted on a multiple lines.

**`[--format-parameters]`**

Indicates whether parameters should be formatted on a multiple lines.

**`[--ignored-names]`** `<FULLY_QUALIFIED_METADATA_NAME>`

Defines a list of metadata names that should be excluded from a documentation. Namespace of type names can be specified.

**`--ignored-projects`** <PROJECT_NAME>

Defines projects that should be skipped.

**`[--include-ienumerable]`**

Indicates whether interface `System.Collections.IEnumerable` should be included in a documentation if a type also implements interface `System.Collections.Generic.IEnumerable<T>`.

**`[--indent-chars]`** `<INDENT_CHARS>`

Defines characters that should be used for indentation. Default value is four spaces.

**`--language`** `{cs[harp]|v[isual-]b[asic])}`

Defines project language.

**`[--merge-attributes]`**

Indicates whether attributes should be displayed in a single attribute list.

**`--msbuild-path`** <MSBUILD_PATH>

Defines a path to MSBuild.

*Note: First found instance of MSBuild will be used if the path to MSBuild is not specified.*

**`[--nest-namespaces]`**

Indicates whether namespaces should be nested.

**`[--no-attribute-arguments]`**

Indicates whether attribute arguments should be omitted when displaying an attribute.

**`[--no-indent]`**

Indicates whether declarations should not be indented.

**`[--no-precedence-for-system]`**

Indicates whether symbols contained in `System` namespace should be ordered as any other symbols and not before other symbols.

**`[--output]`** `<OUTPUT_PATH>`

Defines path to file that will store a list of symbol definitions.

**`--projects`** <PROJECT_NAME>

Defines projects that should be analyzed.

**`-p|--properties`** `<NAME=VALUE>`

Defines one or more MSBuild properties.

**`-v|--verbosity`** `{q[uiet]|m[inimal]|n[ormal]|d[etailed]|diag[nostic]}`

Defines the amount of information to display in the log.

**`[--visibility]`** `{public|internal|private}`

Defines a visibility of a type or a member. Default value is `public`.

## See Also

* [Roslynator Command-Line Interface](README.md)
