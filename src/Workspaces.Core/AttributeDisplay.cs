// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Roslynator
{
    internal static class AttributeDisplay
    {
        public static bool ShouldBeDisplayed(INamedTypeSymbol attributeType)
        {
            switch (attributeType.MetadataName)
            {
                case "ConditionalAttribute":
                case "DebuggableAttribute":
                case "DebuggerBrowsableAttribute":
                case "DebuggerDisplayAttribute":
                case "DebuggerHiddenAttribute":
                case "DebuggerNonUserCodeAttribute":
                case "DebuggerStepperBoundaryAttribute":
                case "DebuggerStepThroughAttribute":
                case "DebuggerTypeProxyAttribute":
                case "DebuggerVisualizerAttribute":
                    return !attributeType.ContainingNamespace.HasMetadataName(MetadataNames.System_Diagnostics);
                case "SuppressMessageAttribute":
                    return !attributeType.ContainingNamespace.HasMetadataName(MetadataNames.System_Diagnostics_CodeAnalysis);
                case "DefaultMemberAttribute":
                case "AssemblyConfigurationAttribute":
                case "AssemblyCultureAttribute":
                case "AssemblyVersionAttribute":
                    return !attributeType.ContainingNamespace.HasMetadataName(MetadataNames.System_Reflection);
                case "AsyncStateMachineAttribute":
                case "CompilationRelaxationsAttribute":
                case "CompilerGeneratedAttribute":
                case "IsReadOnlyAttribute":
                case "InternalsVisibleToAttribute":
                case "IteratorStateMachineAttribute":
                case "MethodImplAttribute":
                case "RuntimeCompatibilityAttribute":
                case "StateMachineAttribute":
                case "TupleElementNamesAttribute":
                case "TypeForwardedFromAttribute":
                case "TypeForwardedToAttribute":
                    return !attributeType.ContainingNamespace.HasMetadataName(MetadataNames.System_Runtime_CompilerServices);
#if DEBUG
                case "AssemblyCompanyAttribute":
                case "AssemblyCopyrightAttribute":
                case "AssemblyDescriptionAttribute":
                case "AssemblyFileVersionAttribute":
                case "AssemblyInformationalVersionAttribute":
                case "AssemblyProductAttribute":
                case "AssemblyTitleAttribute":
                case "AssemblyTrademarkAttribute":
                case "AttributeUsageAttribute":
                case "BarAttribute":
                case "CLSCompliantAttribute":
                case "ComVisibleAttribute":
                case "DefaultValueAttribute":
                case "FlagsAttribute":
                case "FooAttribute":
                case "GuidAttribute":
                case "ObsoleteAttribute":
                case "TargetFrameworkAttribute":
                case "XmlArrayItemAttribute":
                case "XmlAttributeAttribute":
                case "XmlElementAttribute":
                case "XmlRootAttribute":
                    return true;
#endif
            }

            Debug.Fail(attributeType.ToDisplayString());
            return true;
        }
    }
}
