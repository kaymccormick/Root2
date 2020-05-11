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
using AnalysisAppLib.Syntax;
using KayMcCormick.Dev;
using MessageTemplates.Core;

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
    ///     <MyNamespace:SyntaxTypeComboBox/>
    ///
    /// </summary>
    public class SyntaxTypeComboBox : ComboBox
    {
        public static readonly DependencyProperty ItemsUIElementProperty = DependencyProperty.Register(
            "ItemsUIElement", typeof(UIElement), typeof(SyntaxTypeComboBox), new PropertyMetadata(default(UIElement), OnItemsUIElementUpdated));

        private static void OnItemsUIElementUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SyntaxTypeComboBox c = (SyntaxTypeComboBox) d;
            c.OnItemsUIElementUpdates((UIElement) e.OldValue, (UIElement) e.NewValue);
            
        }

        private void OnItemsUIElementUpdates(UIElement old, UIElement @new)
        {
            var tv = old as TreeView;
            if (tv != null)
            {
                tv.SelectedItemChanged -= TreeViewOnSelectedItemChanged;
            }
            if(@new is TreeView tv1)
            {
                tv1.SelectedItemChanged += TreeViewOnSelectedItemChanged;
            }
        }

        public UIElement ItemsUIElement
        {
            get { return (UIElement) GetValue(ItemsUIElementProperty); }
            set { SetValue(ItemsUIElementProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register(
            "DisplayMode", typeof(SyntaxTypeDisplayMode), typeof(SyntaxTypeComboBox), new PropertyMetadata(default(SyntaxTypeDisplayMode), OnDisplayModeUpdated));

        private static void OnDisplayModeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SyntaxTypeComboBox c = (SyntaxTypeComboBox)d;
            c.OnDisplayModeUpdated((SyntaxTypeDisplayMode)e.OldValue, (SyntaxTypeDisplayMode)e.NewValue);
        }

        private void OnDisplayModeUpdated(SyntaxTypeDisplayMode oldValue, SyntaxTypeDisplayMode newValue)
        {
            DebugUtils.WriteLine($"{oldValue} - {newValue}");
            if (newValue == SyntaxTypeDisplayMode.Nested)
            {
                _treeView.Visibility = Visibility.Visible;
                _itemsHost.Visibility = Visibility.Collapsed;
            } else if(newValue == SyntaxTypeDisplayMode.Flat)
            {
                _treeView.Visibility = Visibility.Collapsed;
                _itemsHost.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxTypeDisplayMode DisplayMode
        {
            get { return (SyntaxTypeDisplayMode) GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        static SyntaxTypeComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SyntaxTypeComboBox), new FrameworkPropertyMetadata(typeof(SyntaxTypeComboBox)));

            ComboBox.SelectionBoxItemProperty.OverrideMetadata(typeof(SyntaxTypeComboBox),
                (PropertyMetadata) new FrameworkPropertyMetadata(
                    new PropertyChangedCallback(OnSelectionBoxItemUpdated)));
        }

        private static void OnSelectionBoxItemUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public static readonly DependencyProperty SyntaxTypesModelProperty = DependencyProperty.Register(
            "SyntaxTypesModel", typeof(SyntaxTypesModel), typeof(SyntaxTypeComboBox), new PropertyMetadata(default(SyntaxTypesModel)));

        public SyntaxTypesModel SyntaxTypesModel
        {
            get { return (SyntaxTypesModel) GetValue(SyntaxTypesModelProperty); }
            set { SetValue(SyntaxTypesModelProperty, value); }
        }

        public static readonly DependencyProperty DropDownTemplateProperty = DependencyProperty.Register(
            "DropDownTemplate", typeof(DataTemplate), typeof(SyntaxTypeComboBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate DropDownTemplate
        {
            get { return (DataTemplate) GetValue(DropDownTemplateProperty); }
            set { SetValue(DropDownTemplateProperty, value); }
        }
        public override void OnApplyTemplate()
        {
            //_ComboBox = (ComboBox) GetTemplateChild("ComboBox");
            _dropDown = Template.FindName("DropDown", this);
            _dropDownBorder = Template.FindName("DropDownBorder", this);
            _itemsHost = (Panel)Template.FindName("ItemsHost", this);
            _treeView = (TreeView) Template.FindName("TreeView", this);
            #if false
            if (_dropDownBorder is Border b)
            {
                DebugUtils.WriteLine($"{b.Child}");
                if (b.Child is ScrollViewer sv)
                {
                    if (sv.Content is Grid g)
                    {
                        foreach (UIElement gChild in g.Children)
                        {
                            if (gChild is StackPanel sp1)
                            {
                                
                            }
                            else if (gChild is ContentPresenter cp)

                            {
                                var tv000 = cp.ContentTemplate.FindName("TreeView", cp);
                                if (tv000 == null)
                                {
                                    TreeView FindTreeView(DependencyObject @do)
                                    {
                                        if (@do is TreeView tv1)
                                        {
                                            return tv1;
                                        }


                                        var tv2 = FindVisualTreeView(@do);
                                        if (tv2 != null)
                                        {
                                            return tv2;
                                        }

                                        foreach (DependencyObject child in LogicalTreeHelper.GetChildren(@do))
                                        {
                                            var tv3 = FindTreeView(child);
                                            if (tv2 != null) return tv3;
                                        }

                                        return null;
                                    }

                                    _treeView = FindTreeView(cp);
                                } else
                                {
                                    _treeView = (TreeView) tv000;
                                }
                            }
                        }
                    }
                }
            }
#endif
            if (ItemsUIElement is TreeView tv)
            {
                tv.SelectedItemChanged += TreeViewOnSelectedItemChanged;
            } else if (ItemsUIElement is StackPanel sp)
            {

            }
            _treeView.SelectedItemChanged += TreeViewOnSelectedItemChanged;
            //_ComboBox.SelectionChanged += ComboBoxOnSelectionChanged;
        }

        private TreeView FindVisualTreeView(DependencyObject @do)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(@do); i++)
            {
                var child0 = VisualTreeHelper.GetChild(@do, i);
                if (child0 is TreeView tv00)
                {
                    return tv00;
                    
                }
            }

            return null;
        }

        private void TreeViewOnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IsDropDownOpen = false;
            var appTypeInfo = (AppTypeInfo)e.NewValue;
            SyntaxTypesModel.SelectedTypeInfo = appTypeInfo;
            SetValue(ComboBox.SelectionBoxItemProperty, appTypeInfo);
        }

        private ComboBox _ComboBox;
        private TreeView _treeView;
        private object _dropDown;
        private object _dropDownBorder;
        private Panel _itemsHost;

        private void ComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SyntaxTypeDisplayMode
    {
        Nested,
        Flat,
        None
    }

    public class SyntaxTypesModel : DependencyObject
    {
        public static readonly DependencyProperty SelectedTypeInfoProperty = DependencyProperty.Register(
            "SelectedTypeInfo", typeof(AppTypeInfo), typeof(SyntaxTypesModel), new PropertyMetadata(default(AppTypeInfo)));

        public AppTypeInfo SelectedTypeInfo
        {
            get { return (AppTypeInfo) GetValue(SelectedTypeInfoProperty); }
            set { SetValue(SelectedTypeInfoProperty, value); }
        }

        public static readonly DependencyProperty SyntaxTypeInfosProperty = DependencyProperty.Register(
            "SyntaxTypeInfos", typeof(IEnumerable<AppTypeInfo>), typeof(SyntaxTypesModel), new PropertyMetadata(default(IEnumerable<AppTypeInfo>)));

        public IEnumerable<AppTypeInfo> SyntaxTypeInfos
        {
            get { return (IEnumerable<AppTypeInfo>) GetValue(SyntaxTypeInfosProperty); }
            set { SetValue(SyntaxTypeInfosProperty, value); }
        }

        public static readonly DependencyProperty TopLevelTypeInfosProperty = DependencyProperty.Register(
            "TopLevelTypeInfos", typeof(IEnumerable<AppTypeInfo>), typeof(SyntaxTypesModel), new PropertyMetadata(default(IEnumerable<AppTypeInfo>)));

        public IEnumerable<AppTypeInfo> TopLevelTypeInfos
        {
            get { return (IEnumerable<AppTypeInfo>) GetValue(TopLevelTypeInfosProperty); }
            set { SetValue(TopLevelTypeInfosProperty, value); }
        }
        public BindingExpressionBase SetBinding(
            DependencyProperty dp,
            BindingBase binding)
        {
            return BindingOperations.SetBinding((DependencyObject)this, dp, binding);
        }
    }
}
