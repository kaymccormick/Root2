using System ;
using System.ComponentModel ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InstanceInfoTypeDescriptor : CustomTypeDescriptor
    {
        #region Overrides of CustomTypeDescriptor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public override TypeConverter GetConverter ( )
        {
            return new WpfInstanceInfoConverter ( ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class WpfInstanceInfoConverter : InstanceInfoTypeConverter
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
            grid.RowDefinitions.Add(new RowDefinition { Height      = new GridLength(1, GridUnitType.Star) });
            var tb = new TextBlock { Text                           = v.Instance.ToString() };
            var sp = new StackPanel { Orientation                   = Orientation.Horizontal } ;
            var uie = new Border { BorderBrush                      = Brushes.Crimson, BorderThickness = new Thickness(3),  CornerRadius = new CornerRadius(1, 2, 1, 2 )} ;
            sp.Children.Add ( tb ) ;
            var tp = new ToolTip { Content = v.Instance.GetType ( ).FullName } ;
            uie.ToolTip = tp ;
            uie.Child   = grid ;
            var textBlock = new TextBlock { Text = "InstanceInfo" , FontSize = 12, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom} ;
            textBlock.SetValue ( Panel.ZIndexProperty , 100 ) ;
            textBlock.Background = Brushes.Azure ;

            grid.Children.Add ( textBlock ) ;

            grid.Children.Add ( sp ) ;
            return uie ;
        }
        #endregion
    }
}
