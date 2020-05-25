using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AnalysisControls.TypeDescriptors
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericInterface2 : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TypeDescriptorProperty = DependencyProperty.Register(
            "TypeDescriptor", typeof(ICustomTypeDescriptor), typeof(GenericInterface2),
            new PropertyMetadata(default(ICustomTypeDescriptor), OnTypeDescriptorChanged));

        public ICustomTypeDescriptor TypeDescriptorz
        {
            get { return (ICustomTypeDescriptor) GetValue(TypeDescriptorProperty); }
            set { SetValue(TypeDescriptorProperty, value); }
        }

        private static void OnTypeDescriptorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GenericInterface2) d).OnTypeDescriptorChanged((ICustomTypeDescriptor) e.OldValue,
                (ICustomTypeDescriptor) e.NewValue);
        }



        protected virtual void OnTypeDescriptorChanged(ICustomTypeDescriptor oldValue, ICustomTypeDescriptor newValue)
        {
        }

        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register(
            "Properties", typeof(PropertyDescriptorCollection), typeof(GenericInterface2),
            new PropertyMetadata(default(PropertyDescriptorCollection), OnPropertiesChanged, CoerceProperties));

        private static object CoerceProperties(DependencyObject d, object basevalue)
        {
            return TypeDescriptor.GetProperties(d.GetValue(InstanceProperty));
        }

        public PropertyDescriptorCollection Properties
        {
            get { return (PropertyDescriptorCollection) GetValue(PropertiesProperty); }
            set { SetValue(PropertiesProperty, value); }
        }

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GenericInterface2) d).OnPropertiesChanged((PropertyDescriptorCollection) e.OldValue,
                (PropertyDescriptorCollection) e.NewValue);
        }

        public static readonly DependencyProperty InstanceProperty = DependencyProperty.Register(
            "Instance", typeof(object), typeof(GenericInterface2),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.AffectsRender,
                OnInstanceChanged));

        private ObservableCollection<object> _instanceProperties = new ObservableCollection<object>();

        public object Instance
        {
            get { return (object) GetValue(InstanceProperty); }
            set { SetValue(InstanceProperty, value); }
        }

        private static void OnInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GenericInterface2) d).OnInstanceChanged((object) e.OldValue, (object) e.NewValue);
        }



        protected virtual void OnInstanceChanged(object oldValue, object newValue)
        {
            CoerceValue(PropertiesProperty);
            CoerceValue(InstancePropertiesProperty);
        }

        public static readonly DependencyProperty InstancePropertiesProperty = DependencyProperty.Register(
            "InstanceProperties", typeof(IEnumerable), typeof(GenericInterface2),
            new PropertyMetadata(default(IEnumerable), OnInstancePropertiesChanged, CoerceInstanceProperties));

        private ICollectionView _view;
        private ListView _listView;
        private Context1 _serviceProvider;

        private static object CoerceInstanceProperties(DependencyObject d, object basevalue)
        {
            var instanceProperties = ((GenericInterface2) d)._instanceProperties;
            instanceProperties.Clear();
            var o1 = d.GetValue(InstanceProperty);
            var enumerable = (IEnumerable) d.GetValue(PropertiesProperty);
            if (enumerable != null)
                foreach (PropertyDescriptor o in enumerable)
                {
                    var editor = o.GetEditor(typeof(UITypeEditor));

                    InstanceProperty p = new InstanceProperty() {Instance = o1, PropertyDescriptor = o};
                    p.UITypeEditor = editor as UITypeEditor;
                    PaintValue(p);

                    if (p.PropertyDescriptor.Converter.GetStandardValuesSupported())
                    {
                        p.StandardValues = p.PropertyDescriptor.Converter.GetStandardValues();
                    }
                    instanceProperties.Add(p);
                }

            return instanceProperties;
        }

        public static void PaintValue(InstanceProperty p)
        {
            if (p.UITypeEditor != null && p.UITypeEditor.GetPaintValueSupported())
            {
                p.PaintValueSupported = true;
                var dim = 40;
                IntPtr intPtr = IntPtr.Zero;
                try
                {
                    var bitmap = new Bitmap(dim, dim);
                    var fromImage = Graphics.FromImage(bitmap);
                    p.UITypeEditor.PaintValue(p.Value, fromImage, new Rectangle(0, 0, dim, dim));
                    intPtr = bitmap.GetHbitmap();
                    p.ImageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        intPtr,
                        IntPtr.Zero,
                        System.Windows.Int32Rect.Empty,
                        BitmapSizeOptions.FromWidthAndHeight(dim, dim));
                }
                finally
                {
                    DeleteObject(intPtr);
                }
            }
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);


        public IEnumerable InstanceProperties
        {
            get { return (IEnumerable) GetValue(InstancePropertiesProperty); }
            set { SetValue(InstancePropertiesProperty, value); }
        }

        private static void OnInstancePropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GenericInterface2) d).OnInstancePropertiesChanged((IEnumerable) e.OldValue, (IEnumerable) e.NewValue);
        }


        /// <inheritdoc />
        public GenericInterface2()
        {
            InstanceProperties = _instanceProperties;
            _serviceProvider = new Context1();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open, CanOpen));
        }

        private void CanOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            var p = e.Parameter as InstanceProperty;
            e.CanExecute = p?.UITypeEditor != null && !Editing;
            e.ContinueRouting = false;
            e.Handled = true;
        }

        private void Open(object sender, ExecutedRoutedEventArgs e)
        {
            var p = e.Parameter as InstanceProperty;
            var u = sender as UIElement;
            var c = _listView.ItemContainerGenerator.ContainerFromItem(p);

            
            _serviceProvider.Instance = p.Instance;
            _serviceProvider.PropertyDescriptor = p.PropertyDescriptor;
            _serviceProvider._uiElement = e.OriginalSource as UIElement;
            _serviceProvider.AboutToEdit = true;
            Editing = true;
            var item = p.UITypeEditor.EditValue((ITypeDescriptorContext) _serviceProvider ,p.Value);
            p.Value = item;
            Editing = false;
            //p.PropertyDescriptor.SetValue(p.Instance, editValue);
        }

        public bool Editing { get; set; }


        protected virtual void OnInstancePropertiesChanged(IEnumerable oldValue, IEnumerable newValue)
        {
        }


        protected virtual void OnPropertiesChanged(PropertyDescriptorCollection oldValue,
            PropertyDescriptorCollection newValue)
        {
            CoerceValue(InstancePropertiesProperty);
        }


        static GenericInterface2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericInterface2),
                new FrameworkPropertyMetadata(typeof(GenericInterface2)));

        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _listView = (ListView) GetTemplateChild("ListView");
            _view = CollectionViewSource.GetDefaultView(InstanceProperties);
            _view.Filter = o => ((InstanceProperty) o).IsBrowsable;
            _view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            _listView.ItemsSource = _view;
        }
    }
}