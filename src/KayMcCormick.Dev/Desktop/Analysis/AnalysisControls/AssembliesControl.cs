using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class AssembliesControl : Control, IAppCustomControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AssemblySourceProperty = DependencyProperty.Register(
            "AssemblySource", typeof(IEnumerable<Assembly>), typeof(AssembliesControl), new PropertyMetadata(default(IEnumerable<Assembly>)));

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Assembly> AssemblySource
        {
            get { return (IEnumerable<Assembly>) GetValue(AssemblySourceProperty); }
            set { SetValue(AssemblySourceProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedAssemblyProperty = DependencyProperty.Register(
            "SelectedAssembly", typeof(Assembly), typeof(AssembliesControl), new PropertyMetadata(default(Assembly)));

        /// <summary>
        /// 
        /// </summary>
        public Assembly SelectedAssembly
        {
            get { return (Assembly) GetValue(SelectedAssemblyProperty); }
            set { SetValue(SelectedAssemblyProperty, value); }
        }
        static AssembliesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AssembliesControl), new FrameworkPropertyMetadata(typeof(AssembliesControl)));
        }
    }
}