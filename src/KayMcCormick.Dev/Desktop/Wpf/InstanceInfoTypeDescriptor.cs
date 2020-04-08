using System;
using System.Collections.Generic;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Globalization ;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class InstanceInfoTypeDescriptor : CustomTypeDescriptor
    {
        #region Overrides of CustomTypeDescriptor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TypeConverter GetConverter ( )
        {
            return new WpfInstanceInfoConverter ( ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class WpfInstanceInfoConverter : InstanceInfoTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public WpfInstanceInfoConverter ( )
        {
            Debug.WriteLine ( "Instantiating WpfInst.." ) ;
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
            
            if ( destinationType == typeof ( UIElement ) )
            {
                var v = ( InstanceInfo ) value ;
                var tb = new TextBlock() { Text = v.Instance.ToString() };
                var sp = new StackPanel { Orientation = Orientation.Horizontal } ;
                var uie = new Border { BorderBrush = Brushes.Crimson, BorderThickness = new Thickness(3),  CornerRadius = new CornerRadius(1, 2, 1, 2 )} ;
                sp.Children.Add ( tb ) ;
                ToolTip tp = new ToolTip { Content = v.Instance.GetType ( ).FullName } ;
                uie.ToolTip = tp ;
                uie.Child = sp ;
                return uie ;
            }
            return base.ConvertTo ( context , culture , value , destinationType ) ;
        }
        #endregion
    }
}
