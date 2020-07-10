#region header

// Kay McCormick (mccor)
//
// KayMcCormick.Dev
// ProjTests
// ProjTests.cs
//
// 2020-02-21-12:38 AM
//
// ---

#endregion

using Microsoft.Build.Locator;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Resources;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Security.Permissions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Markup.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using System.Xaml;
using System.Xml;
using System.Xml.Linq;
using AnalysisAppLib;
using AnalysisAppLib.Serialization;
using AnalysisAppLib.Syntax;
using AnalysisControls;
using AnalysisControls.Commands;
using AnalysisControls.Converters;
using AnalysisControls.Properties;
using AnalysisControls.TypeDescriptors;
using AnalysisControls.ViewModel;
using AnalysisControlsCore;
using Autofac;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Themes;
using Castle.DynamicProxy;
// using CsvHelper;
// using CsvHelper.Excel;

using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Application;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Container;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Dev.TestLib;
using KayMcCormick.Dev.TestLib.Fixtures;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;
using KayMcCormick.Lib.Wpf.JSON;
using KayMcCormick.Lib.Wpf.ViewModel;
using KmDevLib;
using KmDevWpfControls;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NLog;
using RibbonLib.Model;
using RoslynCodeControls;
using Xunit;
using Xunit.Abstractions;
using static AnalysisControls.TypeDescriptors.UiElementTypeConverter;
using AssembliesControl = AnalysisControls.AssembliesControl;
using Binding = System.Windows.Data.Binding;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using ColorConverter = System.Windows.Media.ColorConverter;
using CompilationError = AnalysisControls.CompilationError;
using Condition = System.Windows.Automation.Condition;
using Control = System.Windows.Forms.Control;
using ConversionUtils = AnalysisControls.ConversionUtils;
using CustomTextCharacters = AnalysisControls.CustomTextCharacters;
using CustomTextEndOfLine = AnalysisControls.CustomTextEndOfLine;
using DiagnosticError = AnalysisControls.DiagnosticError;
using File = System.IO.File;
using FontFamily = System.Windows.Media.FontFamily;
using FormattingHelper = AnalysisControls.FormattingHelper;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using ListBox = System.Windows.Controls.ListBox;
using Menu = System.Windows.Controls.Menu;
using MenuItem = System.Windows.Controls.MenuItem;
using MethodInfo = System.Reflection.MethodInfo;
using Orientation = System.Windows.Controls.Orientation;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;
using Process = System.Diagnostics.Process;
using Rectangle = System.Windows.Shapes.Rectangle;
using RegionInfo = RoslynCodeControls.RegionInfo;
using String = System.String;

using TextBlock = System.Windows.Controls.TextBlock;
using Window = System.Windows.Window;
using XamlReader = System.Windows.Markup.XamlReader;
using XamlWriter = System.Windows.Markup.XamlWriter;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedVariable
// ReSharper disable RedundantOverriddenMember

namespace ProjTests
{
public class RibbonBuilderTests {
[WpfFact]
public void TestRibbonBuilderContent() {
var w = new Window();
var rb = new RibbonBuilder1();
rb.BeginInit();
rb.EndInit();
w.Content = rb;
w.Loaded += (sender, args) =>
{
    ProjTestsHelper.DumpVisualTree(w);
    w.Close();
};
w.ShowDialog();
}

}

}
