using System ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Input ;

namespace WpfApp.Controls
{
    /// <summary>
    ///     Control for displaying IOC container information.
    /// </summary>
    public partial class Container : UserControl
    {
        /// <summary>Parameterless constructor.</summary>
        public Container ( ) { InitializeComponent ( ) ; }

        private void LoadInstance ( object sender , ExecutedRoutedEventArgs e )
        {
            throw new NotImplementedException ( ) ;
        }

        private void Metadata ( object sender , ExecutedRoutedEventArgs e )
        {
            throw new NotImplementedException ( ) ;
        }

        private void InstancesOnly_OnChecked ( object sender , RoutedEventArgs e )
        {
            throw new NotImplementedException ( ) ;
        }

        private void InstancesOnly_OnUnchecked ( object sender , RoutedEventArgs e )
        {
            throw new NotImplementedException ( ) ;
        }
    }
}
