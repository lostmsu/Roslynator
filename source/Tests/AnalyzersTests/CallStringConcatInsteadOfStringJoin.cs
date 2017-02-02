﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Roslynator.CSharp.Analyzers.Tests
{
#pragma warning disable RCS1078
    public static class CallStringConcatInsteadOfStringJoin
    {
        private const string EmptyString = "";

        private static void Foo()
        {
            string x = "";

            x = string.Join("", default(object), default(object));

            x = string.Join("", "a", "b");

            x = string.Join(string.Empty, new string[] { "a", "b" });

            x = string.Join(EmptyString, new object[] { "a", "b" });

            x = string.Join("x", "a", "b");
        }
    }
}
