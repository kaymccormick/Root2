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
using AnalysisControls.RibbonModel;

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
    ///     <MyNamespace:RibbonModelView/>
    ///
    /// </summary>
    public class RibbonModelView : Control
    {

        public static readonly DependencyProperty RootItemsCollectionProperty = DependencyProperty.Register(
            "RootItemsCollection", typeof(RibbonModelViewItemsCollection), typeof(RibbonModelView), new PropertyMetadata(default(RibbonModelViewItemsCollection)));

        public static readonly DependencyProperty RibbonModelProperty = DependencyProperty.Register(
            "RibbonModel", typeof(PrimaryRibbonModel), typeof(RibbonModelView), new PropertyMetadata(default(PrimaryRibbonModel)));

        public RibbonModelView()
        {
            RootItemsCollection = new RibbonModelViewItemsCollection();
            var tabsNode = new RibbonTabsNode();
            BindingOperations.SetBinding(tabsNode, RibbonModelViewItem.RibbonModelProperty, new Binding("RibbonModel") {Source = this});
            RootItemsCollection.Add(tabsNode);
            var ctabsNode = new RibbonContextualTabGroupsNode();
            BindingOperations.SetBinding(ctabsNode, RibbonModelViewItem.RibbonModelProperty, new Binding("RibbonModel") { Source = this });
            RootItemsCollection.Add(ctabsNode);
            var qatNode = new RibbonQATNode();
            BindingOperations.SetBinding(qatNode, RibbonModelViewItem.RibbonModelProperty, new Binding("RibbonModel") { Source = this });
            RootItemsCollection.Add(qatNode);
        }

        public PrimaryRibbonModel RibbonModel
        {
            get { return (PrimaryRibbonModel) GetValue(RibbonModelProperty); }
            set { SetValue(RibbonModelProperty, value); }
        }

        public RibbonModelViewItemsCollection RootItemsCollection
        {
            get { return (RibbonModelViewItemsCollection) GetValue(RootItemsCollectionProperty); }
            set { SetValue(RootItemsCollectionProperty, value); }
        }

        static RibbonModelView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonModelView), new FrameworkPropertyMetadata(typeof(RibbonModelView)));
        }
    }

    public class RibbonQATNode : RibbonModelViewItem
    {
    }

    public class RibbonTabsNode : RibbonModelViewItem
    {
    }

    public class RibbonContextualTabGroupsNode : RibbonModelViewItem
    {

    }

    public class RibbonModelViewItemsCollection : ObservableCollection<RibbonModelViewItem>
    {

    }

    public class RibbonModelViewItem : DependencyObject
    {
        public static readonly DependencyProperty RibbonModelProperty = DependencyProperty.Register(
            "RibbonModel", typeof(PrimaryRibbonModel), typeof(RibbonModelViewItem), new PropertyMetadata(default(PrimaryRibbonModel)));

        public PrimaryRibbonModel RibbonModel
        {
            get { return (PrimaryRibbonModel) GetValue(RibbonModelProperty); }
            set { SetValue(RibbonModelProperty, value); }
        }
    }
}
