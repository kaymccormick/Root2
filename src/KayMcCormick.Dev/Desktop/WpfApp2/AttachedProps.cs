using System.Windows ;
using System.Windows.Data ;

namespace WpfApp2
{
    public static class AttachedProps
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("WpfAnalyzers.DependencyProperty", "WPF0023:The callback is trivial, convert to lambda.", Justification = "<Pending>")]
        public static readonly DependencyProperty PropertiesCollectionProperty =
            DependencyProperty.RegisterAttached (
                                                 "PropertiesCollection"
                                               , typeof ( ObservablePropertyCollection )
                                               , typeof ( AttachedProps )
                                               , new FrameworkPropertyMetadata (
                                                                                null
                                                                              , FrameworkPropertyMetadataOptions
                                                                                   .None
                                                                              , OnPropertiesCollectionChanged
                                                                              , CoercePropertiesCollection
                                                                              , false
                                                                              , UpdateSourceTrigger
                                                                                   .PropertyChanged
                                                                               )
                                                ) ;
        private static object CoercePropertiesCollection ( DependencyObject d , object basevalue )
        {
            return basevalue ;
        }

        private static void OnPropertiesCollectionChanged (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
        }

        /// <summary>Helper for setting <see cref="PropertiesCollectionProperty"/> on <paramref name="target"/>.</summary>
        /// <param name="target"><see cref="DependencyObject"/> to set <see cref="PropertiesCollectionProperty"/> on.</param>
        /// <param name="value">PropertiesCollection property value.</param>
        public static void SetPropertiesCollection (
            DependencyObject                  target
          , ObservablePropertyCollection value
        )
        {
            target.SetValue ( PropertiesCollectionProperty , value ) ;
        }

        /// <summary>Helper for getting <see cref="PropertiesCollectionProperty"/> from <paramref name="target"/>.</summary>
        /// <param name="target"><see cref="DependencyObject"/> to read <see cref="PropertiesCollectionProperty"/> from.</param>
        /// <returns>PropertiesCollection property value.</returns>
        [AttachedPropertyBrowsableForType(typeof ( FrameworkElement ))]
        public static ObservablePropertyCollection GetPropertiesCollection ( DependencyObject target )
        {
            return ( ObservablePropertyCollection ) target.GetValue (
                                                                          PropertiesCollectionProperty
                                                                         ) ;
        }
    }
}