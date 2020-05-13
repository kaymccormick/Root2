using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace AnalysisControls
{
    public class AssembliesControl : Control
    {
        public static readonly DependencyProperty AssemblySourceProperty = DependencyProperty.Register(
            "AssemblySource", typeof(IEnumerable<Assembly>), typeof(AssembliesControl), new PropertyMetadata(default(IEnumerable<Assembly>)));

        public IEnumerable<Assembly> AssemblySource
        {
            get { return (IEnumerable<Assembly>) GetValue(AssemblySourceProperty); }
            set { SetValue(AssemblySourceProperty, value); }
        }

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