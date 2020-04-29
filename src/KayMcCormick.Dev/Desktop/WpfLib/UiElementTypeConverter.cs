#region header
// Kay McCormick (mccor)
// 
// Analysis
// WpfLib
// UiElementTypeConverter.cs
// 
// 2020-04-23-9:09 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.ComponentModel ;
using System.Data ;
using System.Globalization ;
using System.Reflection ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Documents ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Media.Animation ;
using System.Windows.Media.Effects ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    public class UiElementTypeConverter : TypeConverter
    {
        public UiElementTypeConverter ( ) { }

        private ILifetimeScope _scope ;

        public UiElementTypeConverter ( ILifetimeScope scope ) { _scope = scope ; }

        #region Overrides of TypeConverter
        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            DebugUtils.WriteLine($"{context.Instance} {context.PropertyDescriptor?.Name} {context.Container} {value} {destinationType?.FullName}");
            if ( destinationType == typeof ( UIElement ) )
            {
                return ConvertToUiElement ( value ) ;
            }

            return base.ConvertTo ( context , culture , value , destinationType ) ;
        }

        public virtual UIElement ConvertToUiElement ( object value )
        {
            var g = ControlForValue ( value , 0 ) ;
            return g ;
        }

        [ NotNull ]
        public static UIElement ControlForValueS ( IEnumerable values , int i )
        {
            var r = new ListBox ( ) ;
            foreach ( var value in values )
            {
                r.Items.Add ( value ) ;
            }

            return r ;
        }

        [ NotNull ]
        public UIElement TreeViewValue ( IEnumerable value , int i )
        {
            var r = new ListView ( ) ;
            var gv = new GridView ( ) ;
            r.View = gv ;
            if ( value is DataSourceProvider dp )
            {
            }

            var data = _scope.Resolve < Func < object , DataTable > > ( ) ;

            var enumerator = value.GetEnumerator ( ) ;
            if ( ! enumerator.MoveNext ( ) )
            {
                return new Grid ( ) ;
            }

            var r2 = data ( enumerator.Current ) ;
            foreach ( DataColumn r2Column in r2.Columns )
            {
                var gridViewColumn = new GridViewColumn ( ) ;
                gridViewColumn.Header = r2Column.ColumnName ;
                gridViewColumn.DisplayMemberBinding = new Binding ( r2Column.ColumnName )
                                                      {
                                                          Mode               = BindingMode.OneWay
                                                        , Converter          = new BasicConverte ( )
                                                        , ConverterParameter = r2Column
                                                      } ;
                gv.Columns.Add ( gridViewColumn ) ;
            }

            // foreach ( var o in ( IEnumerable ) value )
            // {
            // r.Items.Add ( o ) ;
            // }

            r.ItemsSource = ( IEnumerable ) value ;

            return r ;
        }

        public Border WrapBorder ( UIElement objView , int i )
        {
            return new Border
                   {
                       Child           = objView
                     , Padding         = new Thickness ( 10 )
                     , BorderBrush     = i == 0 ? Brushes.White : Brushes.Gray
                     , BorderThickness = new Thickness ( 3 )
                   } ;
        }

        public UIElement ControlForValue ( object value , int i )
        {
            Border Border ( UIElement uiElement ) { return WrapBorder ( uiElement , i ) ; }
            var controlForValue = _ControlForValue ( value , i , i == 0 ) ;
            return controlForValue ;
        }

        [ NotNull ]
        public UIElement _ControlForValue ( object value , int i , bool b2 )
        {
            if ( value == null )
            {
                return new StackPanel ( ) ;
            }

            if ( value is Type vt )
            {
                return new ContentControl { Content = vt } ;
            }

            if ( value is string ss )
            {
                return new TextBlock { Text = ss } ;
            }

            var type = value.GetType ( ) ;
            DebugUtils.WriteLine ( $"{value}{type}" ) ;

            Border b ;
            Brush[] bs = { Brushes.AliceBlue , Brushes.AntiqueWhite , Brushes.Pink } ;
            if ( i == 0 )
            {
                b = new Border ( )
                    {
                        Padding         = new Thickness ( 10 )
                      , BorderBrush     = Brushes.Beige
                      , BorderThickness = new Thickness ( 2 )
                      , Background      = bs[ i ]
                    } ;
            }
            else
            {
                b = new Border ( )
                    {
                        Padding         = new Thickness ( 2 )
                      , BorderBrush     = Brushes.Gray
                      , BorderThickness = new Thickness ( 2 )
                      , Background      = bs[ i ]
                    } ;
            }

            var g = CreateGrid ( ) ;
            b.Child = g ;

            var oneStar = new GridLength ( 1 , GridUnitType.Star ) ;
            var gridLength = oneStar ;
            g.RowDefinitions.Add ( new RowDefinition { Height = GridLength.Auto } ) ;

            var curRow = 1 ;
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
            var gridWidth = gridLength ;
            if ( b2 )
            {
                gridWidth = GridLength.Auto ;
            }
            else
            {
            }

            g.ColumnDefinitions.Add ( new ColumnDefinition ( ) { Width = GridLength.Auto } ) ;
            g.ColumnDefinitions.Add ( new ColumnDefinition ( ) { Width = gridWidth } ) ;
            g.ColumnDefinitions.Add ( new ColumnDefinition ( ) { Width = GridLength.Auto } ) ;

            var header1 = new TextBlock ( )
                          {
                              Margin       = new Thickness ( 5 , 0 , 0 , 0 )
                            , Text         = "Property Name"
                            , TextWrapping = TextWrapping.NoWrap
                          } ;
            header1.SetValue ( Grid.RowProperty ,    curRow ) ;
            header1.SetValue ( Grid.ColumnProperty , 0 ) ;
            var header2 = new TextBlock ( )
                          {
                              Text = "Value" , TextWrapping = TextWrapping.NoWrap
                          } ;
            header2.SetValue ( Grid.RowProperty ,    curRow ) ;
            header2.SetValue ( Grid.ColumnProperty , 1 ) ;
            var header3 = new TextBlock ( )
                          {
                              Text         = "Property Type"
                            , TextWrapping = TextWrapping.NoWrap
                            , Margin       = new Thickness ( 0 , 0 , 5 , 0 )
                          } ;
            header3.SetValue ( Grid.RowProperty ,    curRow ) ;
            header3.SetValue ( Grid.ColumnProperty , 2 ) ;
            curRow ++ ;
            foreach ( var textBlock in new[] { header1 , header2 , header3 } )
            {
                textBlock.TextDecorations.Add (
                                               new TextDecoration (
                                                                   TextDecorationLocation.Underline
                                                                 , new Pen ( Brushes.Black , 1 )
                                                                 , 0
                                                                 , TextDecorationUnit.Pixel
                                                                 , TextDecorationUnit.Pixel
                                                                  )
                                              ) ;
            }

            g.Children.Add ( header1 ) ;
            g.Children.Add ( header2 ) ;
            g.Children.Add ( header3 ) ;

            foreach ( var propertyInfo in type.GetProperties (
                                                              BindingFlags.Instance
                                                              | BindingFlags.Public
                                                             ) )
            {
                try
                {
                    LogManager.GetCurrentClassLogger ( )
                              .Info ( $"Property {type} {propertyInfo.Name}" ) ;
                    object val1 = null ;
                    try
                    {
                        val1 = propertyInfo.GetValue ( value ) ;
                    }
                    catch
                    {
                        continue ;
                    }

                    if ( val1 == null )
                    {
                        continue ;
                    }

                    var propName = new TextBlock ( )
                                   {
                                       Margin       = new Thickness ( 3 )
                                     , Text         = propertyInfo.Name
                                     , TextWrapping = TextWrapping.NoWrap
                                   } ;
                    g.Children.Add ( propName ) ;
                    UIElement d ;
                    if ( i < 2 ) // ?? lol
                    {
                        if ( val1 is IEnumerable ie1
                             && val1.GetType ( ) != typeof ( string ) )
                        {
                            //d = TreeViewValue ( ie1 , 0 ) ;
                            d = _ControlForValue(ie1, i + 1, false);
                            // var controlForValue = new StackPanel ( ) ;
                            // foreach ( var elem in ie1 )
                            // {
                                // controlForValue.Children.Add ( ControlForValue ( elem , 0 ) ) ;
                            // }
                         //  d = controlForValue ;
                        }
                        else
                        {
                            d = _ControlForValue ( val1 , i + 1 , true ) ;
                        }
                    }
                    else
                    {
                        d = new TextBlock ( )
                            {
                                Text = val1.ToString ( ) , TextWrapping = TextWrapping.NoWrap
                            } ;
                    }
                    g.Children.Add ( d ) ;
                    g.RowDefinitions.Add ( new RowDefinition { Height = GridLength.Auto } ) ;
                    g.RowDefinitions.Add ( new RowDefinition { Height = GridLength.Auto } ) ;
                    d.SetValue ( Grid.RowProperty , curRow ) ;
                    propName.SetValue ( Grid.RowProperty , curRow ) ;
                    d.SetValue ( Grid.ColumnProperty , 1 ) ;
                    propName.SetValue ( Grid.ColumnProperty , 0 ) ;

                    var tc1 = new TypeControl ( ) { Margin = new Thickness ( 3 ) } ;
                    tc1.SetValue ( Grid.RowProperty ,    curRow ) ;
                    tc1.SetValue ( Grid.ColumnProperty , 2 ) ;
                    tc1.SetValue (
                                  AttachedProperties.RenderedTypeProperty
                                , propertyInfo.PropertyType
                                 ) ;
                    g.Children.Add ( tc1 ) ;

                    curRow ++ ;
                }
                catch { }
            }

            curRow ++ ;
            var typeControl = new TypeControl ( ) { Margin = new Thickness ( 3 ) } ;
            typeControl.SetValue ( Grid.RowProperty ,                        curRow ) ;
            typeControl.SetValue ( AttachedProperties.RenderedTypeProperty , type ) ;
            g.Children.Add ( typeControl ) ;
            if ( curRow <= 3 )
            {
                return b ;
            }

            if ( i > 0 )
            {
                return b ;
            }

            // var glyphTypeface = new GlyphTypeface(new Uri ( @"c:\windows\fonts\cour.ttf" )) ;
            // var gr = new GlyphRun ( 72 ) ;
            // (gr as ISupportInitialize)?.BeginInit();
            // gr.GlyphTypeface = glyphTypeface ;
            // var word = "Kay" ;
            // gr.Characters.Clear();
            //
            // foreach ( var ch in word )
            // {
            //     gr.Characters.Add ( ch ) ;
            //     //var gm = gr.GlyphTypeface.CharacterToGlyphMap[ ch ] ;
            // }
            //
            // (gr as ISupportInitialize)?.EndInit();
            // var gl = _scope.Resolve < GlyphRun > ( ) ;
            // object gl = null ;
            // try
            // {
            // gl = XamlReader.Parse (
            //                                       @"<GlyphRun xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            // Characters = ""Kay"" GlyphTypeface=""file://system/fonts/cour.ttf""/>"
            // ) ;
            // }
            // catch
            // {

            // }

            var pen = new Pen ( Brushes.Crimson , 1 ) ;
            var center = new Point ( 10 , 10 ) ;
            var ellipseGeometry = new EllipseGeometry ( center , 5 , 5 ) ;
            var geometryDrawing = new GeometryDrawing ( Brushes.Aqua , pen , ellipseGeometry ) ;
            var drawingBrush = new DrawingBrush ( geometryDrawing ) ;
            drawingBrush.Viewport = new Rect ( new Size ( 10 , 10 ) ) ;
            drawingBrush.Stretch  = Stretch.None ;
            // if ( gl != null )
            // {
            //     var glyphRunDrawing = new GlyphRunDrawing ( Brushes.Black , ( GlyphRun ) gl ) ;
            //     drawingBrush = new DrawingBrush ( glyphRunDrawing ) ;
            // }

            return new ScrollViewer
                   {
                       Background                    = drawingBrush
                     , Padding                       = new Thickness ( 20 )
                     , Content                       = b
                     , VerticalAlignment             = VerticalAlignment.Top
                     , HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
                   } ;
        }

        private static Grid CreateGrid ( )
        {
            var grid = new Grid ( ) { } ;
            grid.SetValue ( TextElement.FontSizeProperty , ( double ) 22.0 ) ;
            return grid ;
        }
        #endregion

        #region Overrides of TypeConverter
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            DebugUtils.WriteLine($"{context.Instance} {context.PropertyDescriptor?.Name} {context.Container} {destinationType?.FullName}");
            if ( destinationType == typeof ( UIElement ) )
            {
                return true ;
            }

            return base.CanConvertTo ( context , destinationType ) ;
        }

        protected UIElement CreateUiElement ( ) { return new Grid ( ) ; }
        #endregion
    }

    public class BasicConverte : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return value?.ToString ( ) ;
        }

        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return null ;
        }
        #endregion
    }
}