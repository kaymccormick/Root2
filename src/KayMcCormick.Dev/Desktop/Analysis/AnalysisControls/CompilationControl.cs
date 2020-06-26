using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CompilationControl : Control, IAppCustomControl
    {
            public CSharpCompilation CSharpCompilation
            {
                get { return Compilation as CSharpCompilation; }
            }

            public static readonly DependencyProperty CSharpCompilationOptionsProperty = DependencyProperty.Register(
                "CSharpCompilationOptions", typeof(CSharpCompilationOptions), typeof(CompilationControl), new PropertyMetadata(default(CSharpCompilationOptions), OnCSharpCompilationOptionsChanged));

            public CSharpCompilationOptions CSharpCompilationOptions
            {
                get { return (CSharpCompilationOptions) GetValue(CSharpCompilationOptionsProperty); }
                set { SetValue(CSharpCompilationOptionsProperty, value); }
            }

            private static void OnCSharpCompilationOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                ((CompilationControl) d).OnCSharpCompilationOptionsChanged((CSharpCompilationOptions) e.OldValue, (CSharpCompilationOptions) e.NewValue);
            }



            protected virtual void OnCSharpCompilationOptionsChanged(CSharpCompilationOptions oldValue, CSharpCompilationOptions newValue)
            {
            }


            /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CompilationProperty = DependencyProperty.Register(
            "Compilation", typeof(Compilation), typeof(CompilationControl), new FrameworkPropertyMetadata(default(Compilation),  FrameworkPropertyMetadataOptions.None, PropertyChangedCallback));

            private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var c = (CompilationControl) d;
                Compilation cc = (Compilation) e.NewValue;
                if (cc != null)
                {
                    DebugUtils.WriteLine("Updating compilation to " + cc);
                    c.DeclarationDiagnostics = cc.GetDeclarationDiagnostics().ToList();
                    c.Diagnostics = cc.GetDiagnostics().ToList();
                }
            }

            public IEnumerable<Diagnostic> DeclarationDiagnostics { get; set; }
            public IEnumerable<Diagnostic> Diagnostics { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Compilation Compilation
        {
            get { return (Compilation)GetValue(CompilationProperty); }
            set { SetValue(CompilationProperty, value); }
        }

    }
}