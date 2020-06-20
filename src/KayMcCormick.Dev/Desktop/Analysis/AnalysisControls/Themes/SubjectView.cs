using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
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

namespace AnalysisControlsCore
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControlsCore.Themes"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControlsCore.Themes;assembly=AnalysisControlsCore.Themes"
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
    ///     <MyNamespace:SubjectView/>
    ///
    /// </summary>
    public class SubjectView : ListView
    {
        public static readonly DependencyProperty ObservableProperty = DependencyProperty.Register(
            "Observable", typeof(IObservable<object>), typeof(SubjectView), new PropertyMetadata(default(IObservable<object>), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubjectView vv = ((SubjectView) d);
            ((IObservable<object>)e.NewValue).SubscribeOn(Scheduler.Default)
                .ObserveOnDispatcher(DispatcherPriority.Background).Subscribe(o =>
                {
                    ((ICollection<object>) vv.ItemsSource).Add(o);
                });


// .ObserveOnDispatcher(DispatcherPriority.Background)
// .Subscribe(x =>


        }

        public static readonly DependencyProperty ItemTypeProperty = DependencyProperty.Register(
            "ItemType", typeof(Type), typeof(SubjectView), new PropertyMetadata(default(Type), PropertyChangedCallback2));

        private static void PropertyChangedCallback2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubjectView v1 = (SubjectView) d;
            Type t = (Type) e.NewValue;
        }

        public Type ItemType
        {
            get { return (Type) GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }
        public IObservable<object> Observable
        {
            get { return (IObservable<object>) GetValue(ObservableProperty); }
            set { SetValue(ObservableProperty, value); }
        }
        static SubjectView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SubjectView), new FrameworkPropertyMetadata(typeof(SubjectView)));
            ListView.ViewProperty.OverrideMetadata(typeof(SubjectView), new PropertyMetadata(A));
        }

        private static void A(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubjectView s = (SubjectView) d;
            GridView g = (GridView) e.NewValue;
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(s.ItemType))
            {
                g.Columns.Add(new GridViewColumn() { Header = property.DisplayName, DisplayMemberBinding = new Binding() });
            }
        }

        public SubjectView()
        {
            var observableCollection = new ObservableCollection<object>();

            SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = observableCollection});
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
        }
    }
}
