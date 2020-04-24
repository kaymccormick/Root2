#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Lib.Wpf
// WpfInstanceInfoConverter.cs
// 
// 2020-04-15-12:43 PM
// 
// ---
#endregion
using System ;
using System.ComponentModel ;
using System.Globalization ;
using System.IO ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class WpfInstanceInfoConverter : InstanceInfoTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public WpfInstanceInfoConverter ( )
        {
            DebugUtils.WriteLine ( "Instantiating WpfInst.." ) ;
        }


        #region Overrides of InstanceInfoTypeConverter
        /// <inheritdoc />
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            if ( destinationType == typeof ( UIElement ) )
            {
                return true ;
            }
            return base.CanConvertTo ( context , destinationType ) ;
        }

        /// <inheritdoc />
        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            if ( destinationType != typeof ( UIElement ) )
            {
                return base.ConvertTo ( context , culture , value , destinationType ) ;
            }

            var v = ( InstanceInfo ) value ;
            var grid = new Grid ( ) ;
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var tb = new TextBlock { Text                           = v.Instance.ToString(), BaselineOffset = 20};
            // var sp = new StackPanel
                     // {
                         // Orientation = Orientation.Horizontal,
                     // } ;
            var uie = new Border { BorderBrush                      = Brushes.Crimson, BorderThickness = new Thickness(3),  CornerRadius = new CornerRadius(1, 2, 1, 2 )} ;
            tb.SetValue(Grid.RowSpanProperty, 2);
            grid.Children.Add ( tb ) ;
            var tp = new ToolTip { Content = v.Instance.GetType ( ).FullName } ;
            uie.ToolTip = tp ;
            uie.Child   = grid ;
            var textBlock = new TextBlock { Text = "InstanceInfo", FontSize = 12, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom, BaselineOffset = -20};
            textBlock.SetValue(Panel.ZIndexProperty, 100);
            textBlock.SetValue(Grid.RowSpanProperty, 2);
            textBlock.SetValue(Grid.RowProperty, 1);
            textBlock.Background = Brushes.Azure;
            grid.Children.Add(textBlock);
            if ( v.Metadata == null )
            {
                return uie ;
            }

            const string callerfilepath = "CallerFilePath" ;
            if ( ! v.Metadata.ContainsKey ( callerfilepath ) )
            {
                return uie ;
            }

            var s = ( string ) v.Metadata[ callerfilepath ] ;
            var o = v.Metadata[ "CallerLineNumber" ] ;
            var fileName = Path.GetFileName ( s ) + ";" + o ;
            var textBlock2 = new TextBlock
                             {
                                 Text                = fileName
                               , FontSize            = 12
                               , HorizontalAlignment = HorizontalAlignment.Right
                               , VerticalAlignment   = VerticalAlignment.Bottom
                             } ;
            textBlock2.SetValue(Grid.RowSpanProperty,    2);
            textBlock2.SetValue(Grid.RowProperty,        1);
            textBlock2.SetValue(Grid.ColumnProperty,     1);
            textBlock2.SetValue ( Panel.ZIndexProperty , 100 ) ;
            textBlock2.Background = Brushes.Azure ;

            grid.Children.Add ( textBlock2 ) ;

            return uie ;
        }
        #endregion
    }
}