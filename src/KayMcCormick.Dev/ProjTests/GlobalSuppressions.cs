﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'ContainerTests.ContainerTests(ITestOutputHelper output, LoggingFixture loggingFixture)', validate parameter 'loggingFixture' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjTests.ContainerTests.#ctor(Xunit.Abstractions.ITestOutputHelper,KayMcCormick.Dev.TestLib.Fixtures.LoggingFixture)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1822:Member CreateCompilation does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~M:ProjTests.ProjTests.CreateCompilation(Microsoft.CodeAnalysis.SyntaxTree,System.String)~Microsoft.CodeAnalysis.CSharp.CSharpCompilation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2000:Call System.IDisposable.Dispose on object created by 'File.OpenText ( @\"C:\\data\\logs\\ProjInterface.json\" )' before all references to it are out of scope.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjTests.ProjTests.DeserializeLog")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Logger.Debug(object)' could vary based on the current user's locale settings. Replace this call in 'ProjTests.DeserializeLog()' with a call to 'Logger.Debug(IFormatProvider, object)'.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjTests.ProjTests.DeserializeLog")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'Dispose' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjTests.ProjTests.Dispose")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'HandleInnerExceptions' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjTests.ProjTests.HandleInnerExceptions(System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2008:Do not create tasks without passing a TaskScheduler", Justification = "<Pending>", Scope = "member", Target = "~M:ProjTests.ProjTests.TestFormattedCodeControl2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2227:Change 'Finalizers' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:ProjTests.ProjTests.Finalizers")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1724:The type name ProjTests conflicts in whole or in part with the namespace name 'ProjTests'. Change either name to eliminate the conflict.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjTests.ProjTests")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ContainerTests' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjTests.ContainerTests")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'GeneralPurpose' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjTests.GeneralPurpose")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ProjectFixture' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjTests.ProjectFixture")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ProjTests' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjTests.ProjTests")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "member", Target = "~M:ProjTests.ProjTests.TestResourcesTree1")]