﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.InvocationParms.ProcessInvocation~AnalysisFramework.ILogInvocation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.Transforms.TransformTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1823:Unused field 'Logger'.", Justification = "<Pending>", Scope = "member", Target = "~F:AnalysisFramework.CodeAnalyseContext.Logger")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1801:Parameter opts of method FromSyntaxTree is never used. Remove the parameter or use it in the method body.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.CodeAnalyseContext.FromSyntaxTree(Microsoft.CodeAnalysis.SyntaxTree,System.String,Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions)~AnalysisFramework.ISyntaxTreeContext")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'CodeAnalyseContext2.CodeAnalyseContext2(SemanticModel currentModel)', validate parameter 'currentModel' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.CodeAnalyseContext2.#ctor(Microsoft.CodeAnalysis.SemanticModel)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'char.ToString()' could vary based on the current user's locale settings. Replace this call in 'Ext.GetRelativePath(string, string)' with a call to 'char.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.Ext.GetRelativePath(System.String,System.String)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'string Ext.RelativePath(Document doc)', validate parameter 'doc' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.Ext.RelativePath(Microsoft.CodeAnalysis.Document)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'char.ToString()' could vary based on the current user's locale settings. Replace this call in 'Document.ShortenedPath()' with a call to 'char.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.Ext.ShortenedPath(Microsoft.CodeAnalysis.Document)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1062:In externally visible method 'string Ext.ShortenedPath(Document doc)', validate parameter 'doc' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.Ext.ShortenedPath(Microsoft.CodeAnalysis.Document)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'int.ToString()' could vary based on the current user's locale settings. Replace this call in 'InvocationParms.ProcessInvocation()' with a call to 'int.ToString(IFormatProvider)'.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.InvocationParms.ProcessInvocation~AnalysisFramework.ILogInvocation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'ProcessInvocation' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.InvocationParms.ProcessInvocation~AnalysisFramework.ILogInvocation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1305:The behavior of 'Logger.Info(string, string)' could vary based on the current user's locale settings. Replace this call in 'LogUsages.CheckInvocationExpression(SyntaxNode, SemanticModel, params INamedTypeSymbol[])' with a call to 'Logger.Info(IFormatProvider, string, string)'.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.LogUsages.CheckInvocationExpression(Microsoft.CodeAnalysis.SyntaxNode,Microsoft.CodeAnalysis.SemanticModel,Microsoft.CodeAnalysis.INamedTypeSymbol[])~System.Tuple{System.Boolean,Microsoft.CodeAnalysis.IMethodSymbol,Microsoft.CodeAnalysis.SyntaxNode}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1801:Parameter semanticModel of method CreateLogInvocation is never used. Remove the parameter or use it in the method body.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.LogUsages.CreateLogInvocation(System.String,Microsoft.CodeAnalysis.IMethodSymbol,Microsoft.CodeAnalysis.SyntaxNode,Microsoft.CodeAnalysis.SemanticModel,Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax,System.Collections.Generic.IList{AnalysisFramework.ILogInvocationArgument})~AnalysisFramework.ILogInvocation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.RefactorLogUsage.ComputeRefactoringsAsync(Microsoft.CodeAnalysis.CodeRefactorings.CodeRefactoringContext)~System.Threading.Tasks.Task")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2227:Change 'Arguments' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:AnalysisFramework.LogInvocation2.Arguments")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1812:LogInvocation is an internal class that is apparently never instantiated. If so, remove the code from the assembly. If this class is intended to contain only static members, make it static (Shared in Visual Basic).", Justification = "<Pending>", Scope = "type", Target = "~T:AnalysisFramework.LogInvocation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1823:Unused field 'Logger'.", Justification = "<Pending>", Scope = "member", Target = "~F:AnalysisFramework.LogUsage.LogUsages.Logger")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1031:Modify 'ProcessInvocation' to catch a more specific allowed exception type, or rethrow the exception.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.LogUsage.InvocationParams.ProcessInvocation~AnalysisFramework.LogUsage.Interfaces.ILogInvocation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA1801:Parameter semanticModel of method CreateLogInvocation is never used. Remove the parameter or use it in the method body.", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.LogUsage.LogUsages.CreateLogInvocation(System.String,Microsoft.CodeAnalysis.IMethodSymbol,Microsoft.CodeAnalysis.SyntaxNode,Microsoft.CodeAnalysis.SemanticModel,Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax)~AnalysisFramework.LogUsage.Interfaces.ILogInvocation")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:AnalysisFramework.LogUsage.RefactorLogUsage.ComputeRefactoringsAsync(Microsoft.CodeAnalysis.CodeRefactorings.CodeRefactoringContext)~System.Threading.Tasks.Task")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Build", "CA2227:Change 'Arguments' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:AnalysisFramework.LogUsage.LogInvocation2.Arguments")]