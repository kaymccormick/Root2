﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:ProjInterface.ProjCommands.LoadSolution")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:ProjInterface.ProjCommands.NewAdHocWorkspace")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>", Scope = "member", Target = "~F:ProjInterface.ProjMainWindow.TaskCompleteEvent")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1801:Parameter options of method .ctor is never used. Remove the parameter or use it in the method body.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.JsonSyntaxNodeConverter.InnerConverter`1.#ctor(System.Text.Json.JsonSerializerOptions)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'object MruBackground.Convert(object value, Type targetType, object parameter, CultureInfo culture)', validate parameter 'value' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.MruBackground.Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Logger.Trace(string, string)' could vary based on the current user's locale settings. Replace this call in 'ProjInterfaceApp.OnStartup(StartupEventArgs)' with a call to 'Logger.Trace(IFormatProvider, string, string)'.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjInterfaceApp.OnStartup(System.Windows.StartupEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'void ProjInterfaceApp.OnStartup(StartupEventArgs e)' passes a literal string as parameter 'caption' of a call to 'MessageBoxResult MessageBox.Show(string messageBoxText, string caption)'. Retrieve the following string(s) from a resource table instead: \"Error\".", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjInterfaceApp.OnStartup(System.Windows.StartupEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'OnStartup' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjInterfaceApp.OnStartup(System.Windows.StartupEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'void ProjMainWindow._projectBrowser_OnSelected(object sender, RoutedEventArgs e)' passes a literal string as parameter 'message' of a call to 'void Logger.Info(string message, string argument)'. Retrieve the following string(s) from a resource table instead: \"selected {i}\".", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjMainWindow._projectBrowser_OnSelected(System.Object,System.Windows.RoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Logger.Info(string, string)' could vary based on the current user's locale settings. Replace this call in 'ProjMainWindow._projectBrowser_OnSelected(object, RoutedEventArgs)' with a call to 'Logger.Info(IFormatProvider, string, string)'.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjMainWindow._projectBrowser_OnSelected(System.Object,System.Windows.RoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'void ProjMainWindow.AddTask(Task task, TaskWrap tw)' passes a literal string as parameter 'message' of a call to 'void Logger.Trace(string message, string argument)'. Retrieve the following string(s) from a resource table instead: \"Adding task {desc}\".", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjMainWindow.AddTask(System.Threading.Tasks.Task,ProjInterface.TaskWrap)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Logger.Trace(string, string)' could vary based on the current user's locale settings. Replace this call in 'ProjMainWindow.AddTask(Task, TaskWrap)' with a call to 'Logger.Trace(IFormatProvider, string, string)'.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjMainWindow.AddTask(System.Threading.Tasks.Task,ProjInterface.TaskWrap)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Logger.Debug(string, object)' could vary based on the current user's locale settings. Replace this call in 'ProjMainWindow.CommandBinding_OnExecuted(object, ExecutedRoutedEventArgs)' with a call to 'Logger.Debug(IFormatProvider, string, object)'.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjMainWindow.CommandBinding_OnExecuted(System.Object,System.Windows.Input.ExecutedRoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'void ProjMainWindow.CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)' passes a literal string as parameter 'message' of a call to 'void Logger.Debug(string message, object argument)'. Retrieve the following string(s) from a resource table instead: \"Running analysis on {project}\".", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjMainWindow.CommandBinding_OnExecuted(System.Object,System.Windows.Input.ExecutedRoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'void ProjMainWindow.OnInitialized(EventArgs e)', validate parameter 'e' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjMainWindow.OnInitialized(System.EventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'void ProjUtils.SetupFormattedCode1(ContentControl w, string sourceText, ISyntaxTreeContext ctx, IAddChild addChild, FormattedCode control)', validate parameter 'control' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjUtils.SetupFormattedCode1(System.Windows.Controls.ContentControl,System.String,AnalysisFramework.ISyntaxTreeContext,System.Windows.Markup.IAddChild,AnalysisControls.FormattedCode)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2208:Call the ArgumentOutOfRangeException constructor that contains a message and/or paramName parameter.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.PropertyConverter.Process(System.Text.Json.JsonElement)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'Process' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.PropertyConverter.Process(System.Text.Json.JsonElement)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2227:Change 'ObsTasks' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:ProjInterface.ProjMainWindow.ObsTasks")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2227:Change 'PropertiesColumns' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:ProjInterface.ProjMainWindow.PropertiesColumns")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1822:Member WorkspaceActionBlock does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~P:ProjInterface.ProjMainWindow.WorkspaceActionBlock")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1507:Use nameof in place of string literal 'Task'", Justification = "<Pending>", Scope = "member", Target = "~P:ProjInterface.TaskWrap.Status")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1812:JsonSyntaxNodeConverter.InnerConverter<T> is an internal class that is apparently never instantiated. If so, remove the code from the assembly. If this class is intended to contain only static members, make it static (Shared in Visual Basic).", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.JsonSyntaxNodeConverter.InnerConverter`1")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1724:The type name MSBuild conflicts in whole or in part with the namespace name 'Microsoft.CodeAnalysis.MSBuild'. Change either name to eliminate the conflict.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.MSBuild")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1724:The type name Resources conflicts in whole or in part with the namespace name 'System.Resources' defined in the .NET Framework. Rename the type to eliminate the conflict.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.Resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Logger.Info(string, params object[])' could vary based on the current user's locale settings. Replace this call in 'ProjInterfaceApp.ProjInterfaceApp()' with a call to 'Logger.Info(IFormatProvider, string, params object[])'.", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjInterfaceApp.#ctor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2008:Do not create tasks without passing a TaskScheduler", Justification = "<Pending>", Scope = "member", Target = "~M:ProjInterface.ProjMainWindow.CommandBinding_OnExecuted(System.Object,System.Windows.Input.ExecutedRoutedEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1812:MSBuildWorkspaceManager is an internal class that is apparently never instantiated. If so, remove the code from the assembly. If this class is intended to contain only static members, make it static (Shared in Visual Basic).", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.MSBuildWorkspaceManager")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'AnalyzeResults' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.AnalyzeResults")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'BreakTraceListener' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.BreakTraceListener")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Interface 'IAbstractOperation' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.IAbstractOperation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Interface 'IBoundCommandOperation' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.IBoundCommandOperation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Interface 'IBoundOperation' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.IBoundOperation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Interface 'ICommandSpec' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.ICommandSpec")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Interface 'ICompilationView' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.ICompilationView")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'JsonSyntaxNodeConverter' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.JsonSyntaxNodeConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'InnerConverter' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.JsonSyntaxNodeConverter.InnerConverter`1")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'JsonSyntaxTokenConverter' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.JsonSyntaxTokenConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'LogInvocationConverter' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.LogInvocationConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'MruBackground' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.MruBackground")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'MSBuild' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.MSBuild")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'MSBuildWorkspaceManager' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.MSBuildWorkspaceManager")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ProjCommands' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.ProjCommands")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ProjInterfaceApp' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.ProjInterfaceApp")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ProjInterfaceModule' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.ProjInterfaceModule")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ProjMainWindow' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.ProjMainWindow")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ProjUtils' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.ProjUtils")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Delegate 'ContainDelegate' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.ProjUtils.ContainDelegate")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'Resources' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.Properties.Resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'Settings' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.Properties.Settings")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'PropertyConverter' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.PropertyConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'Resources' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.Resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'TaskWrap' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.TaskWrap")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'VsInstancePanel' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.VsInstancePanel")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'WorkspaceTable' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:ProjInterface.WorkspaceTable")]