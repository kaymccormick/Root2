using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
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

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class UiElementTypeConverter : TypeConverter
    {
        private ILifetimeScope _scope;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        public UiElementTypeConverter(ILifetimeScope scope)
        {
            _scope = scope;
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
                return DoConvertToString(value, sb, true).ToString();
            }

            throw new InvalidOperationException();
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private static HashSet<object> seen = new HashSet<object>();

        public static StringBuilder DoConvertToString(object value, StringBuilder sb, bool useTypeConverters = true,
            HashSet<object> hashSet = null)
        {
            if (value == null)
            {
                return sb.Append("null/converted");
            }

            switch (value)
            {
                case DateTime dt:
                    return sb.Append(dt.ToString());
                case SyntaxNode sn:
                    return sb.Append($"SyntaxNode{{{sn.RawKind};{sn.GetType().Name}}}");
                case Type tt:
                    sb.Append("Type{");
                    TypeToText(tt, sb);
                    return sb.Append('}');
                case string ss:
                    return sb.Append(ss);
            }

            if (hashSet != null && !value.GetType().IsValueType)
            {
                if (hashSet.Contains(value)) throw new InvalidOperationException(value.ToString());

                hashSet.Add(value);
            }

            switch (value)
            {
                case IEnumerable vv:
                {
                    var i = 0;
                    foreach (var o in vv)
                    {
                        sb.AppendFormat("[{0}]", i);
                        sb.Append(DoConvertToString(o, sb, useTypeConverters, hashSet));
                        sb.Append("n");
                        i++;
                    }

                    return sb;
                }
            }

            var t = value.GetType();
            if (t.IsGenericType && t.IsConstructedGenericType)
            {
                var args = t.GenericTypeArguments;
                if (args.Length == 2)
                {
                    var t2 = typeof(IDictionary<,>).MakeGenericType(args);
                    if (t2.IsAssignableFrom(t))
                    {
                        var conv = typeof(DictConverter<,>).MakeGenericType(t2);
                        var c = Activator.CreateInstance(conv);
                        var method = conv.GetMethod("DumpInstance", BindingFlags.Instance | BindingFlags.Public);
                        var strval = method.Invoke(c, new object[] {value})?.ToString() ?? "null";
                        return sb.Append(strval);
                    }
                }
            }

            if ((bool) value?.GetType().IsPrimitive)
            {
                sb.Append(value).Append("{");
                TypeToText(value.GetType(), sb);
                return sb.Append("}");
            }

            var coll = TypeDescriptor.GetProperties(value.GetType());
            if (coll.Count != 0)
            {
                foreach (PropertyDescriptor property in coll)
                {
                    if (!property.IsBrowsable)
                    {
                        DebugUtils.WriteLine($"{property.Name} unbrowsable");
                        continue;
                    }

                    sb.Append(property.Name);
                    sb.Append('(');
                    TypeToText(property.PropertyType, sb);
                    sb.Append(")\t = ");
                    var val = property.GetValue(value);
                    // DebugUtils.WriteLine($"{property.Name} = {val}");
                    string convval = null;
                    if (val is string sss)
                        convval = sss;
                    else if (val is byte[]) convval = "byte[]";

                    if (val == null)
                    {
                        convval = "null/converted";
                    }
                    else
                    {
                        if (useTypeConverters)
                        {
                            var conv = property.Converter;

                            if (conv != null && conv.CanConvertTo(typeof(string)))
                                convval = conv.ConvertTo(val, typeof(string))?.ToString() ?? "null";
                        }

                        if (convval == null && (!useTypeConverters || !(val is string) && val is IEnumerable))
                        {
                            DoConvertToString(val, sb, useTypeConverters, hashSet);
                            sb.Append("\n");
                            continue;
                        }

                        if (convval == null)
                        {
                            var conv = TypeDescriptor.GetConverter(val.GetType());
                            convval = conv.CanConvertTo(typeof(string))
                                ? conv.ConvertTo(val, typeof(string))?.ToString() ?? "null"
                                : val.ToString() + " {" + val.GetType().FullName + "}";
                        }

                    }

                    sb.Append(convval);
                    sb.Append("\n");
                }
            }
            else
            {
                return sb.Append(value);
            }

            return sb;
        }

        private static StringBuilder TypeToText(Type tt, StringBuilder sb)
        {
            if(!string.IsNullOrEmpty(tt.Namespace)) {
                
                    sb.Append(tt.Namespace);
                    sb.Append('.');
            }

            sb.Append(tt.Name);
            if (tt.IsGenericType)
            {
                if (!tt.IsConstructedGenericType)
                {
                    throw new InvalidOperationException(tt.ToString());
                }

                sb.Append('<');
                foreach (var ttarg in tt.GenericTypeArguments)
                {
                    TypeToText(ttarg, sb);
                }

                sb.Append('>');
            }

            return sb;

        }

        public virtual UIElement ConvertToUiElement(object value)
        {
            var g = ControlForValue(value, 0);
            return g;
        }

        [NotNull]
        public static UIElement ControlForValueS(IEnumerable values, int i)
        {
            var r = new ListBox();
            foreach (var value in values) r.Items.Add(value);

            return r;
        }

        [NotNull]
        public UIElement TreeViewValue(IEnumerable value, int i)
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

        public Border WrapBorder(UIElement objView, int i)
        {
            return new Border
            {
                Child = objView, Padding = new Thickness(10), BorderBrush = i == 0 ? Brushes.White : Brushes.Gray,
                BorderThickness = new Thickness(3)
            };
        }

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
                Text = Resources.UiElementTypeConverter__ControlForValue_Property_Name,
                TextWrapping = TextWrapping.NoWrap, Background = solidColorBrush
            };
            var header2 = new TextBlock()
            {
                Text = "Value", TextWrapping = TextWrapping.NoWrap,
                Padding = new Thickness(5), Background = solidColorBrush
            };
            var header3 = new TextBlock()
            {
                Text = Resources.UiElementTypeConverter__ControlForValue_Property_Type,
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


        #region Overrides of TypeConverter

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            DebugUtils.WriteLine(
                $"{context?.Instance} {context?.PropertyDescriptor?.Name} {context?.Container} {destinationType?.FullName}");
            if (destinationType == typeof(UIElement)) return true;

            return base.CanConvertTo(context, destinationType);
        }

        protected UIElement CreateUiElement()
        {
            return new Grid();
        }

        #endregion
    }
}