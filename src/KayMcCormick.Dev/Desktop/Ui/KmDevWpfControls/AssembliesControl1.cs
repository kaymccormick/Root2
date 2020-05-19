using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace KmDevWpfControls
{
    /// <summary>
    /// 
    /// </summary>
    public class AssembliesControl1 : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty AssemblySourceProperty = DependencyProperty.Register(
            "AssemblySource", typeof(IEnumerable<Assembly>), typeof(AssembliesControl1),
            new PropertyMetadata(default(IEnumerable<Assembly>)));

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
            "SelectedAssembly", typeof(Assembly), typeof(AssembliesControl1), new PropertyMetadata(default(Assembly)));

        /// <summary>
        /// 
        /// </summary>
        public Assembly SelectedAssembly
        {
            get { return (Assembly) GetValue(SelectedAssemblyProperty); }
            set { SetValue(SelectedAssemblyProperty, value); }
        }

        static AssembliesControl1()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AssembliesControl1),
                new FrameworkPropertyMetadata(typeof(AssembliesControl1)));
        }

        public AssembliesControl1()
        {
            AssemblySource = AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}