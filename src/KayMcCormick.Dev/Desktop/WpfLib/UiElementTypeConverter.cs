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
using System.Windows.Media ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    public class UiElementTypeConverter:TypeConverter
    {
        public UiElementTypeConverter ( ) {
        }

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
            if ( destinationType == typeof ( UIElement ) )
            {
                return ConvertToUiElement ( value) ;
            }
            return base.ConvertTo ( context , culture , value , destinationType ) ;
        }

        public virtual UIElement ConvertToUiElement ( object value )
        {
            var g = ControlForValue ( value, 0 ) ;
            return g;
        }

        [ NotNull ]
        public static UIElement ControlForValueS ( IEnumerable values , int i )
        {
            var r = new ListBox();
            foreach(var value in values) {
                r.Items.Add(value);
            }
            return r;
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
            var data = _scope.Resolve<Func<object, DataTable>>();

            var enumerator = value.GetEnumerator() ;
            if (! enumerator.MoveNext ( ) )
            {
                return new Grid (  );
            }
            var r2 = data ( enumerator.Current ) ;
                foreach ( DataColumn r2Column in r2.Columns )
                {
                    var gridViewColumn = new GridViewColumn() ;
                    gridViewColumn.Header = r2Column.ColumnName ;
                    gridViewColumn.DisplayMemberBinding = new Binding(r2Column.ColumnName) {Mode=BindingMode.OneWay,Converter = new BasicConverte(),ConverterParameter = r2Column} ;
                    gv.Columns.Add (gridViewColumn );
                }

                // foreach ( var o in ( IEnumerable ) value )
                // {
                    // r.Items.Add ( o ) ;
                // }

                r.ItemsSource = ( IEnumerable ) value ;
            
            return r ;
            
        }


        [ NotNull ]
        public UIElement ControlForValue ( object value , int i )
        {
            if(value == null)
            {
                return new StackPanel();
            }

            if ( value is string ss )
            {
                return new TextBlock { Text = ss } ;
            }
            DebugUtils.WriteLine($"{value}{value.GetType (  )}");
            Grid g = new Grid ( ) ;
            int curRow = 0 ;
            g.ColumnDefinitions.Add (
                                     new ColumnDefinition ( )
                                     {
                                         Width = new GridLength ( 1 , GridUnitType.Star )
                                     }
                                    ) ;
            g.ColumnDefinitions.Add (
                                     new ColumnDefinition ( )
                                     {
                                         Width = new GridLength ( 1 , GridUnitType.Star )
                                     }
                                    ) ;
            foreach ( var propertyInfo in value.GetType ( )
                                               .GetProperties (
                                                               BindingFlags.Instance | BindingFlags.Public
                                                              ) )
            {
                try
                {
                    LogManager.GetCurrentClassLogger().Info ($"Propert {value.GetType()} {propertyInfo.Name}" ) ;
                    var val1 = propertyInfo.GetValue ( value ) ;
                    if ( val1 == null ) continue ;
                    TextBlock b1 = new TextBlock ( ) { Text = propertyInfo.Name } ;
                    g.Children.Add ( b1 ) ;
                    UIElement d ;
                    if ( i > 0 )
                    {
                        if ( val1 is IEnumerable ie1
                             && val1.GetType ( ) != typeof ( string ) )
                        {
                            d = TreeViewValue ( ie1,0 ) ;
                            //d = ControlForValueS(ie1, 0);
//                            var controlForValue = new StackPanel ( ) ;
//                            foreach ( var elem in ie1 )
//                            {
//                                controlForValue.Children.Add ( ControlForValue ( elem , 0 ) ) ;
//                            }

//                            d = controlForValue ;
                        }
                        else
                        {
                            d = ControlForValue ( val1 , 0 ) ;
                        }
                    }
                    else
                    {
                        d = new TextBlock ( ) { Text = val1.ToString ( ) } ;
                    }

                    g.Children.Add ( d ) ;
                    g.RowDefinitions.Add ( new RowDefinition { Height = GridLength.Auto } ) ;
                    d.SetValue ( Grid.RowProperty , curRow ) ;
                    b1.SetValue ( Grid.RowProperty , curRow ) ;
                    d.SetValue ( Grid.ColumnProperty , 1 ) ;
                    b1.SetValue ( Grid.ColumnProperty , 0 ) ;

                    curRow ++ ;
                }
                catch { }
            }

            if ( curRow <= 3 )
            {
                return g ;
            }
            Border brd = new Border { Child = g, BorderBrush = i == 0 ? Brushes.Red : Brushes.Blue, BorderThickness = new Thickness(2) };
            return brd;
        }
        #endregion

        #region Overrides of TypeConverter
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            if ( destinationType == typeof ( UIElement ) )
            {
                return true ;
            }
            return base.CanConvertTo ( context , destinationType ) ;
        }

        protected UIElement CreateUiElement ( )
        {
            return new Grid ( ) ;
        }
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

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}