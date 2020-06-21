using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace AnalysisControl
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
    public class SubjectView2 : Control
    {
        public static readonly DependencyProperty ObservableProperty = DependencyProperty.Register(
            "Observable", typeof(IObservable<object>), typeof(SubjectView2), new PropertyMetadata(default(IObservable<object>), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubjectView2 vv = ((SubjectView2) d);
            ((IObservable<object>)e.NewValue).SubscribeOn(Scheduler.Default)
                .ObserveOnDispatcher(DispatcherPriority.Background).Subscribe(o =>
                {
                    ((ICollection<object>) vv.Items).Add(o);
                });


// .ObserveOnDispatcher(DispatcherPriority.Background)
// .Subscribe(x =>


        }

        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object>();

        public static readonly DependencyProperty ItemTypeProperty = DependencyProperty.Register(
            "ItemType", typeof(Type), typeof(SubjectView2), new PropertyMetadata(default(Type), PropertyChangedCallback2));

        
        private static void PropertyChangedCallback2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubjectView2 v1 = (SubjectView2) d;
            Type t = (Type) e.NewValue;
        }

        public Type ItemType
        {
            get { return (Type) GetValue(ItemTypeProperty); }
            set { SetValue(ItemTypeProperty, value); }
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

        }
        public IObservable<object> Observable
        {
            get { return (IObservable<object>) GetValue(ObservableProperty); }
            set { SetValue(ObservableProperty, value); }
        }
        static SubjectView2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SubjectView2), new FrameworkPropertyMetadata(typeof(SubjectView2)));
            //ListView.ViewProperty.OverrideMetadata(typeof(SubjectView2), new PropertyMetadata(A));
        }

        private static void A(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SubjectView2 s = (SubjectView2) d;
            GridView g = (GridView) e.NewValue;
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(s.ItemType))
            {
                g.Columns.Add(new GridViewColumn() { Header = property.DisplayName, DisplayMemberBinding = new Binding() });
            }
        }

        public SubjectView2()
        {
            //var observableCollection = new ObservableCollection<object>();

            //SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = observableCollection});
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
        }
    }
}