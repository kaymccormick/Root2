using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnalysisAppLib;
using Autofac;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ControlView/>
    ///
    /// </summary>
    public class ControlView : Control
    {
        public ControlView()
        {
            AddHandler(AttachedProperties.LifetimeScopeChangedEvent, new RoutedPropertyChangedEventHandler<ILifetimeScope>(Target));
            Controls = new ObservableCollection<ControlInfo>();
        }

        private void Target(object sender, RoutedPropertyChangedEventArgs<ILifetimeScope> e)
        {
            if (!ReferenceEquals(e.OriginalSource, this)) return;

            DebugUtils.WriteLine(e.NewValue);
            Dispatcher.InvokeAsync(() =>
            {
                if (e.NewValue != null)
                {
                    var items = e.NewValue.Resolve<IEnumerable<Meta<Lazy<IAppCustomControl>>>>();
                    foreach (var item in items)
                    {
                        var props = MetaHelper.GetMetadataProps(item.Metadata);

                        Controls.Add(new ControlInfo()
                        {
                            Metadata = item.Metadata, MetadataProps = props,
                            Item = item
                        });

                    }
                }
            }, DispatcherPriority.DataBind);
        }


        public static readonly DependencyProperty ControlsProperty = DependencyProperty.Register(
            "Controls", typeof(ObservableCollection<ControlInfo>), typeof(ControlView), new PropertyMetadata(default(ObservableCollection<ControlInfo>)));

        public ObservableCollection<ControlInfo> Controls
        {
            get { return (ObservableCollection<ControlInfo>) GetValue(ControlsProperty); }
            set { SetValue(ControlsProperty, value); }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        static ControlView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlView), new FrameworkPropertyMetadata(typeof(ControlView)));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ControlInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
        /// <summary>
        /// 
        /// </summary>
        public Props MetadataProps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title => MetadataProps.Title;
        /// <summary>
        /// 
        /// </summary>
        public Category Category => MetadataProps.Category;
        /// <summary>
        /// 
        /// </summary>
        public object TabHeader => MetadataProps.TabHeader;

        public Meta<Lazy<IAppCustomControl>> Item { get; set; }
    }
}
