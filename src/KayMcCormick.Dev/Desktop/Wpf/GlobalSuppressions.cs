﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1816:Change BaseApp.Dispose() to call GC.SuppressFinalize(object). This will prevent derived types that introduce a finalizer from needing to re-implement 'IDisposable' to call it.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.BaseApp.Dispose")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1063:Modify 'BaseApp.Dispose' so that it calls Dispose(true), then calls GC.SuppressFinalize on the current object instance ('this' or 'Me' in Visual Basic), and then returns.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.BaseApp.Dispose")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1822:Member ErrorExit does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.BaseApp.ErrorExit(KayMcCormick.Dev.ExitCode)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Convert.ChangeType(object, TypeCode)' could vary based on the current user's locale settings. Replace this call in 'BaseApp.ErrorExit([ExitCode])' with a call to 'Convert.ChangeType(object, TypeCode, IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.BaseApp.ErrorExit(KayMcCormick.Dev.ExitCode)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'object ResolveExtension.ProvideValue(IServiceProvider serviceProvider)' passes a literal string as parameter 'message' of a call to 'Exception.Exception(string message)'. Retrieve the following string(s) from a resource table instead: \"No lifetime scope\".", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.ResolveExtension.ProvideValue(System.IServiceProvider)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1716:Rename namespace KayMcCormick.Lib.Wpf so that it no longer conflicts with the reserved language keyword 'Lib'. Using a reserved keyword as the name of a namespace makes it harder for consumers in other languages to use the namespace.", Justification = "<Pending>", Scope = "namespace", Target = "~N:KayMcCormick.Lib.Wpf")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1063:Provide an overridable implementation of Dispose(bool) on 'BaseApp' or mark the type as sealed. A call to Dispose(false) should only clean up native resources. A call to Dispose(true) should clean up both managed and native resources.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.BaseApp")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'ApplyConfiguration' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.BaseApp.ApplyConfiguration~System.Collections.Generic.List{System.Object}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'void BaseApp.OnStartup(StartupEventArgs e)' passes a literal string as parameter 'message' of a call to 'void ILogger.Debug(string message, string argument)'. Retrieve the following string(s) from a resource table instead: \"Adding module {module}\".", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.BaseApp.OnStartup(System.Windows.StartupEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'ILogger.Debug(string, string)' could vary based on the current user's locale settings. Replace this call in 'BaseApp.OnStartup(StartupEventArgs)' with a call to 'ILogger.Debug(IFormatProvider, string, string)'.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.BaseApp.OnStartup(System.Windows.StartupEventArgs)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'void DataTemplateKeyConverter.Write(Utf8JsonWriter writer, DataTemplateKey value, JsonSerializerOptions options)', validate parameter 'writer' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.DataTemplateKeyConverter.Write(System.Text.Json.Utf8JsonWriter,System.Windows.DataTemplateKey,System.Text.Json.JsonSerializerOptions)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1303:Method 'object GetTypeConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)' passes a literal string as parameter 'message' of a call to 'void Logger.Debug(string message, string argument)'. Retrieve the following string(s) from a resource table instead: \"{type}\".", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.GetTypeConverter.Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Logger.Debug(string, string)' could vary based on the current user's locale settings. Replace this call in 'GetTypeConverter.Convert(object, Type, object, CultureInfo)' with a call to 'Logger.Debug(IFormatProvider, string, string)'.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.GetTypeConverter.Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'AddResourceInfos' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.ResourcesUtil.AddResourceInfos(System.Type,System.Windows.ResourceDictionary,System.Collections.ObjectModel.ObservableCollection{KayMcCormick.Lib.Wpf.ResourceInfo},System.Windows.ResourceDictionary)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'Convert' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.XamlXmlDocumentConverter.Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA3075:An XmlDocument instance is created without setting its XmlResolver property to a secure value.", Justification = "<Pending>", Scope = "member", Target = "~M:KayMcCormick.Lib.Wpf.XamlXmlDocumentConverter.Convert(System.Object,System.Type,System.Object,System.Globalization.CultureInfo)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2227:Change 'Parent' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:KayMcCormick.Lib.Wpf.ResourceInfo.Parent")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'AppWindow' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.AppWindow")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'AttachedProperties' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.AttachedProperties")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'BaseApp' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.BaseApp")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'DataTemplateKeyConverter' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.DataTemplateKeyConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'GetTypeConverter' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.GetTypeConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'LifetimeScopeExtension' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.LifetimeScopeExtension")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ResolveExtension' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.ResolveExtension")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ResourceInfo' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.ResourceInfo")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'ResourcesUtil' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.ResourcesUtil")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'XamlXmlDocumentConverter' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.XamlXmlDocumentConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "DV2002:Class 'XmlViewer' is not mapped to any Dependency Validation diagram.", Justification = "<Pending>", Scope = "type", Target = "~T:KayMcCormick.Lib.Wpf.XmlViewer")]