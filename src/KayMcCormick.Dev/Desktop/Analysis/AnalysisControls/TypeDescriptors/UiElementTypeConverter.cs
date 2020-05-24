using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using Autofac;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Properties;
using KmDevWpfControls;
using TypeControl = KayMcCormick.Lib.Wpf.TypeControl;

namespace AnalysisControls.TypeDescriptors
{
    /// <summary>
    /// 
    /// </summary>
    public class GenericInterface2 : Control
    {
        public static readonly DependencyProperty TypeDescriptorProperty = DependencyProperty.Register(
            "TypeDescriptor", typeof(ICustomTypeDescriptor), typeof(GenericInterface2), new PropertyMetadata(default(ICustomTypeDescriptor), OnTypeDescriptorChanged));

        public ICustomTypeDescriptor TypeDescriptorz
        {
            get { return (ICustomTypeDescriptor) GetValue(TypeDescriptorProperty); }
            set { SetValue(TypeDescriptorProperty, value); }
        }

        private static void OnTypeDescriptorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GenericInterface2) d).OnTypeDescriptorChanged((ICustomTypeDescriptor) e.OldValue, (ICustomTypeDescriptor) e.NewValue);
        }



        protected virtual void OnTypeDescriptorChanged(ICustomTypeDescriptor oldValue, ICustomTypeDescriptor newValue)
        {
        }

        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register(
            "Properties", typeof(PropertyDescriptorCollection), typeof(GenericInterface2), new PropertyMetadata(default(PropertyDescriptorCollection), OnPropertiesChanged, CoerceProperties));

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
            ((GenericInterface2) d).OnPropertiesChanged((PropertyDescriptorCollection) e.OldValue, (PropertyDescriptorCollection) e.NewValue);
        }

        public static readonly DependencyProperty InstanceProperty = DependencyProperty.Register(
            "Instance", typeof(object), typeof(GenericInterface2), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.AffectsRender, OnInstanceChanged));

        private ObservableCollection<object> _instanceProperties= new ObservableCollection<object>();

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
            "InstanceProperties", typeof(IEnumerable), typeof(GenericInterface2), new PropertyMetadata(default(IEnumerable), OnInstancePropertiesChanged, CoerceInstanceProperties));

        private static object CoerceInstanceProperties(DependencyObject d, object basevalue)
        {
            var instanceProperties = ((GenericInterface2)d)._instanceProperties;
            instanceProperties.Clear();
            var o1 = d.GetValue(InstanceProperty);
            var enumerable = (IEnumerable)d.GetValue(PropertiesProperty);
            if (enumerable != null)
                foreach (PropertyDescriptor o in enumerable)
                {
                    InstanceProperty p = new InstanceProperty() {Instance = o1, PropertyDescriptor = o};
                    instanceProperties.Add(p);
                }

            return instanceProperties;
        }


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
        }

        protected virtual void OnInstancePropertiesChanged(IEnumerable oldValue, IEnumerable newValue)
        {
        }


        protected virtual void OnPropertiesChanged(PropertyDescriptorCollection oldValue, PropertyDescriptorCollection newValue)
        {
            CoerceValue(InstancePropertiesProperty);
        }


        static GenericInterface2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericInterface2),
                new FrameworkPropertyMetadata(typeof(GenericInterface2)));

        }

    }

    public class InstanceProperty : INotifyPropertyChanged
    {
        private PropertyDescriptor _propertyDescriptor;
        private object _instance;

        public PropertyDescriptor PropertyDescriptor
        {
            get { return _propertyDescriptor; }
            set
            {
                if (Equals(value, _propertyDescriptor)) return;
                _propertyDescriptor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
            }
        }

        public object Instance
        {
            get { return _instance; }
            set
            {
                if (Equals(value, _instance)) return;
                _instance = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Value));
            }
        }

        public object Value => PropertyDescriptor.GetValue(Instance);
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class GenericInterface :Control
    {
        private Border _border;

        static GenericInterface()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericInterface),
                new FrameworkPropertyMetadata(typeof(GenericInterface)));

        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _border = (Border) GetTemplateChild("Border");
            if (Target != null) _border.Child = ConvertToUiElement(Target);
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target", typeof(object), typeof(GenericInterface), new PropertyMetadata(default(object), OnTargetChanged));

        public object Target
        {
            get { return (object) GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            
            return base.MeasureOverride(constraint);
        }

        /// <inheritdoc />
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            var v = VisualParent;
            ItemsControl own = null;
            while (v != null)
            {
                own = ItemsControl.GetItemsOwner(v);
                if (own != null)
                {
                    break;
                }
                v = VisualTreeHelper.GetParent(v);
            }

            if (own != null)
            {
                var container = own.ItemContainerGenerator.ContainerFromItem(Target);
                if (container is MenuItem mm)
                {
                    DisplayMode = DisplayMode.Compact;
                    MaxHeight = 50;
                }
                MaxHeight = 50;
            }
            if (v is ContentPresenter pp)
            {
                v = VisualTreeHelper.GetParent(pp);
            }

        }

        public DisplayMode DisplayMode { get; set; }

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GenericInterface) d).OnTargetChanged((object) e.OldValue, (object) e.NewValue);
        }



        protected virtual void OnTargetChanged(object oldValue, object newValue)
        {
            if (newValue != null)
                if (_border != null)
                    _border.Child = ConvertToUiElement(newValue);
        }

        public GenericInterface()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual UIElement ConvertToUiElement(object value)
        {
            var g = ControlForValue(value, 0);
            return g;
        }

        public UIElement CreateUiElement()
        {
            return new Grid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        [NotNull]
        public static UIElement ControlForValueS(IEnumerable values, int i)
        {
            var r = new ListBox();
            foreach (var value in values) r.Items.Add(value);

            return r;
        }

        [NotNull]
        private UIElement TreeViewValue(IEnumerable value, int i)
        {
            var r = new ListView();
            var gv = new GridView();
            r.View = gv;

            var enumerator = value.GetEnumerator();
            if (!enumerator.MoveNext()) return new Grid();

            var elementType = value.GetType().GetInterfaces()
                .Where(if1 => if1.IsGenericType && if1.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(if1 => if1.GenericTypeArguments[0]).FirstOrDefault();
            if (elementType == null) return new Grid();
            if (typeof(Type).IsAssignableFrom(elementType)) return new Grid();
            if (typeof(Task).IsAssignableFrom(elementType) || elementType.IsGenericType &&
                elementType.GetGenericTypeDefinition() == typeof(Task<>))
                return new Grid();
            foreach (var propInfo in elementType.GetProperties())
            {
                var browsable = propInfo.GetCustomAttribute<BrowsableAttribute>();
                if (browsable != null && !browsable.Browsable) continue;

                var gridViewColumn = new GridViewColumn
                {
                    Header = propInfo.Name,
                    DisplayMemberBinding = new Binding(propInfo.Name)
                    {
                        Mode = BindingMode.OneWay, Converter = new BasicConverte(), ConverterParameter = propInfo
                    }
                };
                gv.Columns.Add(gridViewColumn);
            }

            // foreach ( var o in ( IEnumerable ) value )
            // {
            // r.Items.Add ( o ) ;
            // }

            r.ItemsSource = (IEnumerable) value;

            return r;
        }

        private Border WrapBorder(UIElement objView, int i)
        {
            return new Border
            {
                Child = objView, Padding = new Thickness(10), BorderBrush = i == 0 ? Brushes.White : Brushes.Gray,
                BorderThickness = new Thickness(3)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public UIElement ControlForValue(object value, int i)
        {
            Border Border(UIElement uiElement)
            {
                return WrapBorder(uiElement, i);
            }

            var controlForValue = _ControlForValue(value, i, i == 0);
            return controlForValue;
        }

        [NotNull]
        public UIElement _ControlForValue(object value, int i, bool b2)
        {
            if (value == null)
            {
                DebugUtils.WriteLine($"Value is null");
                return new StackPanel();
            }

            if (value.GetType() == typeof(Task) || value.GetType().IsGenericType &&
                value.GetType().GetGenericTypeDefinition() == typeof(Task<>))
            {
                DebugUtils.WriteLine($"Task related");
                return new StackPanel();
            }

            if (value is Type vt)
            {
                DebugUtils.WriteLine($"Value is type.");
                var controlForValue = new TypeControl();
                controlForValue.SetValue(AttachedProperties.RenderedTypeProperty, vt);
                return controlForValue;
                //return new ContentControl { Content = vt, VerticalContentAlignment =  VerticalAlignment.Center} ;
            }

            if (value is string ss) return new TextBlock {Text = ss, VerticalAlignment = VerticalAlignment.Center};

            if (value is DateTime dt)
                return new TextBlock
                    {Text = dt.ToString(CultureInfo.InvariantCulture), VerticalAlignment = VerticalAlignment.Center};

            if (value is int intval)
                return new TextBlock
                {
                    Text = intval.ToString(CultureInfo.InvariantCulture), VerticalAlignment = VerticalAlignment.Center
                };

            if (value.GetType().IsPrimitive)
                return new TextBlock {Text = value.ToString(), VerticalAlignment = VerticalAlignment.Center};

            if (value is IEnumerable en1) return TreeViewValue(en1, i);
            var type = value.GetType();
            DebugUtils.WriteLine($"{value}{type}");

            Border b;
            Brush[] bs = {Brushes.AliceBlue, Brushes.AntiqueWhite, Brushes.Pink};
            if (i == 0)
                b = new Border()
                {
                    Margin = new Thickness(2),
                    Padding = new Thickness(2), BorderBrush = Brushes.Beige, BorderThickness = new Thickness(4),
                    Background = bs[i], VerticalAlignment = VerticalAlignment.Center
                };
            else
                b = new Border()
                {
                    Margin = new Thickness(3),
                    Padding = new Thickness(5), BorderBrush = Brushes.Gray, BorderThickness = new Thickness(2),
                    Background = bs[i],
                    VerticalAlignment = VerticalAlignment.Center
                };

            var g = CreateGrid();
            b.Child = g;

            var oneStar = new GridLength(1, GridUnitType.Star);
            var gridLength = oneStar;
            g.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

            var curRow = 1;
            // var blur = new BlurEffect ( )
            // {
            // KernelType    = KernelType.Gaussian
            // , Radius        = 2
            // , RenderingBias = RenderingBias.Quality
            // } ;
            // g.Effect = blur ;

            // var storyboard = new Storyboard ( ) ;
            // DoubleAnimation dub = new DoubleAnimation (
            // 10
            // , 0
            // , new Duration (
            // new TimeSpan (
            // 0
            // , 0
            // , 0
            // , 0
            // , 300
            // )
            // )
            // ) ;
            // g.BeginStoryboard ( storyboard , HandoffBehavior.SnapshotAndReplace , false ) ;
            var gridWidth = gridLength;
            if (b2)
            {
                gridWidth = GridLength.Auto;
            }

            else
            {
            }

            g.ColumnDefinitions.Add(new ColumnDefinition() {Width = GridLength.Auto});
            g.ColumnDefinitions.Add(new ColumnDefinition() {Width = gridWidth});
            g.ColumnDefinitions.Add(new ColumnDefinition() {Width = GridLength.Auto});

            var solidColorBrush = new SolidColorBrush(Color.FromRgb(0x8a, 0x94, 0x8c));
            var header1 = new TextBlock()
            {
                Margin = new Thickness(5, 0, 0, 5), Padding = new Thickness(5),
                Text =  "Property_Name",
                TextWrapping = TextWrapping.NoWrap, Background = solidColorBrush
            };
            var header2 = new TextBlock()
            {
                Text = "Value", TextWrapping = TextWrapping.NoWrap,
                Padding = new Thickness(5), Background = solidColorBrush
            };
        var header3 = new TextBlock()
        {
            Text = "Property_Type",
                Padding = new Thickness(5), TextWrapping = TextWrapping.NoWrap, Margin = new Thickness(0, 0, 5, 0),
                Background = solidColorBrush
            };
            foreach (var textBlock in new[] {header1, header2, header3})
                textBlock.TextDecorations.Add(
                    new TextDecoration(
                        TextDecorationLocation.Underline
                        , new Pen(Brushes.Black, 1)
                        , 0
                        , TextDecorationUnit.Pixel
                        , TextDecorationUnit.Pixel
                    )
                );

            var curCol = 0;
            foreach (var o in new[] {header1, header2, header3})
            {
                var h0 = new Border()
                {
                    //BorderThickness = new Thickness(1), BorderBrush = Brushes.MediumSeaGreen,
                    Padding = new Thickness(3),
                    Child = o
                };
                h0.SetValue(Grid.RowProperty, curRow);
                h0.SetValue(Grid.ColumnProperty, curCol);
                curCol += 1;
                g.Children.Add(h0);
            }

            curRow++;

            foreach (var propertyInfo in type.GetProperties(
                BindingFlags.Instance
                | BindingFlags.Public
            ))
            {
                bool? browsable = null;
                foreach (Attribute propertyInfoCustomAttribute in propertyInfo.GetCustomAttributes(true))
                    switch (propertyInfoCustomAttribute)
                    {
                        case BrowsableAttribute ba:
                            browsable = ba.Browsable;
                            break;
                    }

                if (!browsable.GetValueOrDefault(true)) continue;

                try
                {
                    DebugUtils.WriteLine($"Property {type} {propertyInfo.Name}");
                    object val1 = null;
                    try
                    {
                        val1 = propertyInfo.GetValue(value);
                    }
                    catch (Exception ex)
                    {
                        DebugUtils.WriteLine($"Failed to retrieve property value: {ex.Message}");
                        continue;
                    }

                    if (val1 == null)
                    {
                        DebugUtils.WriteLine($"property value is null");
                        continue;
                    }

                    var propName = new TextBlock()
                    {
                        Margin = new Thickness(3, 10, 3, 5), Text = propertyInfo.Name,
                        TextWrapping = TextWrapping.NoWrap, VerticalAlignment = VerticalAlignment.Center
                    };
                    g.Children.Add(propName);
                    UIElement d;
                    if (i < 2) // ?? lol
                    {
                        if (val1 is IEnumerable ie1
                            && val1.GetType() != typeof(string))
                        {
                            DebugUtils.WriteLine($"Property value is enumerable and not string.");
                            d = TreeViewValue(ie1, 0);
                            //d = _ControlForValue(ie1, i + 1, false);
                            // var controlForValue = new StackPanel ( ) ;
                            // foreach ( var elem in ie1 )
                            // {
                            // controlForValue.Children.Add ( ControlForValue ( elem , 0 ) ) ;
                            // }
                            //  d = controlForValue ;
                        }
                        else
                        {
                            DebugUtils.WriteLine(
                                $"Generating representation of property value type is {val1.GetType().FullName}");
                            d = _ControlForValue(val1, i + 1, true);
                        }
                    }
                    else
                    {
                        d = new TextBlock()
                        {
                            Text = val1.ToString(), TextWrapping = TextWrapping.NoWrap
                        };
                    }

                    g.Children.Add(d);
                    g.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                    g.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
                    d.SetValue(Grid.RowProperty, curRow);
                    propName.SetValue(Grid.RowProperty, curRow);
                    d.SetValue(Grid.ColumnProperty, 1);
                    propName.SetValue(Grid.ColumnProperty, 0);

                    var tc1 = new TypeControl() {Margin = new Thickness(3)};
                    tc1.SetValue(Grid.RowProperty, curRow);
                    tc1.SetValue(Grid.ColumnProperty, 2);
                    tc1.SetValue(
                        AttachedProperties.RenderedTypeProperty
                        , propertyInfo.PropertyType
                    );
                    g.Children.Add(tc1);

                    curRow++;
                }
                catch
                {
                    // ignored
                }
            }

            curRow++;
            var typeControl = new TypeControl() {Margin = new Thickness(3)};
            typeControl.SetValue(Grid.RowProperty, curRow);
            typeControl.SetValue(AttachedProperties.RenderedTypeProperty, type);
            g.Children.Add(typeControl);
            if (curRow <= 3) return b;

            if (i > 0) return b;


            var pen = new Pen(Brushes.Crimson, 1);
            var center = new Point(10, 10);
            var ellipseGeometry = new EllipseGeometry(center, 5, 5);
            var geometryDrawing = new GeometryDrawing(Brushes.Aqua, pen, ellipseGeometry);
            var drawingBrush = new DrawingBrush(geometryDrawing);
            drawingBrush.Viewport = new Rect(new Size(10, 10));
            drawingBrush.Stretch = Stretch.None;

            return new ScrollViewer
            {
                Background = drawingBrush, Padding = new Thickness(0), Content = b,
                VerticalAlignment = VerticalAlignment.Top, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
            };
        }

        private static Grid CreateGrid()
        {
            var grid = new Grid();
            grid.SetValue(TextElement.FontSizeProperty, 12.0);
            return grid;
        }
    }

    public enum DisplayMode
    {
        Compact
    }

    /// <summary>
    /// 
    /// </summary>
    public class UiElementTypeConverter : TypeConverter
    {
        private ILifetimeScope _scope;
        private readonly GenericInterface _genericInterface;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        public UiElementTypeConverter(ILifetimeScope scope)
        {
            _scope = scope;
            _genericInterface = new GenericInterface();
        }

        public GenericInterface GenericInterface
        {
            get { return _genericInterface; }
        }

        /// <inheritdoc />
        public override object ConvertTo(
            ITypeDescriptorContext context
            , CultureInfo culture
            , object value
            , Type destinationType
        )
        {
            DebugUtils.WriteLine(
                $"{context?.Instance} {context?.PropertyDescriptor?.Name} {context?.Container} {value} {destinationType.FullName}");
            if (destinationType == typeof(UIElement)) return ConvertToUiElement(value);
            if (destinationType == typeof(string))
            {
                StringBuilder sb = new StringBuilder();
                return ConversionUtils.DoConvertToString(value, sb).ToString();
            }

        //    throw new AppInvalidOperationException();
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual UIElement ConvertToUiElement(object value)
        {
            return _genericInterface.ConvertToUiElement(value);
        }


        #region Overrides of TypeConverter

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            DebugUtils.WriteLine(
                $"{context} {context?.Instance} {context?.PropertyDescriptor?.Name} {context?.Container} {destinationType?.FullName}");
            if (context != null)
            {

            }
            if (destinationType == typeof(UIElement) || destinationType == typeof(string)) return true;

            return base.CanConvertTo(context, destinationType);
        }

        protected UIElement CreateUiElement()
        {
            return _genericInterface.CreateUiElement();
        }

        #endregion
    }
}