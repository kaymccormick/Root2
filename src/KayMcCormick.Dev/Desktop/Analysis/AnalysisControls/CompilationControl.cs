using System.Windows;
using System.Windows.Controls;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CompilationControl : Control, IAppCustomControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CompilationProperty = DependencyProperty.RegisterAttached(
            "Compilation", typeof(Compilation), typeof(CompilationControl), new FrameworkPropertyMetadata(default(Compilation), FrameworkPropertyMetadataOptions.None));

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