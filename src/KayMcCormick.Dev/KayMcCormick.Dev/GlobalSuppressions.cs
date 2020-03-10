﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2000:Call System.IDisposable.Dispose on object created by 'new MyLogFactory(DoLogMessage)' before all references to it are out of scope.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.Logging.AppLoggingConfigHelper.ConfigureLogging(KayMcCormick.Dev.Logging.LogDelegates.LogMethod,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1801:Parameter s of method Debug is never used. Remove the parameter or use it in the method body.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.Logging.AppLoggingConfigHelper.Debug(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'void LogFactoryInterceptor.Intercept(IInvocation invocation)', validate parameter 'invocation' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.Logging.LogFactoryInterceptor.Intercept(Castle.DynamicProxy.IInvocation)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'bool LoggerFactoryHook.ShouldInterceptMethod(Type type, MethodInfo methodInfo)', validate parameter 'methodInfo' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.Logging.LoggerFactoryHook.ShouldInterceptMethod(System.Type,System.Reflection.MethodInfo)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'void LoggerInterceptor.Intercept(IInvocation invocation)', validate parameter 'invocation' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.Logging.LoggerInterceptor.Intercept(Castle.DynamicProxy.IInvocation)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'void BeforeAfterLoggerAttribute.Before(MethodInfo methodUnderTest)', validate parameter 'methodUnderTest' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.BeforeAfterLoggerAttribute.Before(System.Reflection.MethodInfo)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'GlobalLoggingFixture.GlobalLoggingFixture(IMessageSink sink)' passes a literal string as parameter 'message' of a call to 'DiagnosticMessage.DiagnosticMessage(string message)'. Retrieve the following string(s) from a resource table instead: \"Constructing GlobalLoggingFixture.\".", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.Fixtures.GlobalLoggingFixture.#ctor(Xunit.Abstractions.IMessageSink)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'LoggingFixture.LoggingFixture(IMessageSink sink)' passes a literal string as parameter 'message' of a call to 'DiagnosticMessage.DiagnosticMessage(string message)'. Retrieve the following string(s) from a resource table instead: \"Constructing LoggingFixture.\".", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.Fixtures.LoggingFixture.#ctor(Xunit.Abstractions.IMessageSink)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'LoggingFixture.LoggingFixture(IMessageSink sink)', validate parameter 'sink' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.Fixtures.LoggingFixture.#ctor(Xunit.Abstractions.IMessageSink)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1816:Change LoggingFixture.Dispose() to call GC.SuppressFinalize(object). This will prevent derived types that introduce a finalizer from needing to re-implement 'IDisposable' to call it.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.Fixtures.LoggingFixture.Dispose")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1063:Modify 'LoggingFixture.Dispose' so that it calls Dispose(true), then calls GC.SuppressFinalize on the current object instance ('this' or 'Me' in Visual Basic), and then returns.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.Fixtures.LoggingFixture.Dispose")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'IDictionary LogHelper.TestMethodProperties(MethodInfo method, TestMethodLifecycle stage)', validate parameter 'method' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.LogHelper.TestMethodProperties(System.Reflection.MethodInfo,KayMcCormick.Dev.TestLib.TestMethodLifecycle)~System.Collections.IDictionary")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'void LogTestMethodAttribute.After(MethodInfo methodUnderTest)', validate parameter 'methodUnderTest' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.LogTestMethodAttribute.After(System.Reflection.MethodInfo)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'void LogTestMethodAttribute.Before(MethodInfo methodUnderTest)', validate parameter 'methodUnderTest' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.TestLib.LogTestMethodAttribute.Before(System.Reflection.MethodInfo)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1715:Prefix interface name ExecutionContext with 'I'.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Dev.Logging.ExecutionContext")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1812:RegConfigSource is an internal class that is apparently never instantiated. If so, remove the code from the assembly. If this class is intended to contain only static members, make it static (Shared in Visual Basic).", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Dev.Logging.RegConfigSource")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1001:Type 'GlobalLoggingFixture' owns disposable field(s) '_xunitSinkTarget' but is not disposable", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Dev.TestLib.Fixtures.GlobalLoggingFixture")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1063:Provide an overridable implementation of Dispose(bool) on 'LoggingFixture' or mark the type as sealed. A call to Dispose(false) should only clean up native resources. A call to Dispose(true) should clean up both managed and native resources.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Dev.TestLib.Fixtures.LoggingFixture")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>", Scope = "module")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.DataBindingTraceFilter.MyTraceFilter.ShouldTrace(System.Diagnostics.TraceEventCache,System.String,System.Diagnostics.TraceEventType,System.Int32,System.String,System.Object[],System.Object,System.Object[])~System.Boolean")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'void ApplicationInstance.DebugServices(IContainer c)' passes a literal string as parameter 'message' of a call to 'void Logger.Debug(string message, string argument)'. Retrieve the following string(s) from a resource table instead: \"services: {services}\".", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.ApplicationInstance.DebugServices(Autofac.IContainer)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1822:Member Shutdown does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.ApplicationInstance.Shutdown")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1801:Parameter config1 of method EnsureLoggingConfigured is never used. Remove the parameter or use it in the method body.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Dev.Logging.AppLoggingConfigHelper.EnsureLoggingConfigured(KayMcCormick.Dev.Logging.LogDelegates.LogMethod,KayMcCormick.Dev.Logging.ILoggingConfiguration,System.String)")]