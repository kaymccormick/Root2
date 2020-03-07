using System ;
using System.Windows.Markup ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using NLog ;
using WpfApp.Core.Interfaces ;

namespace WpfApp.Core.Xaml
{
    /// <summary></summary>
    /// <seealso cref="System.Windows.Markup.MarkupExtension" />
    [ MarkupExtensionReturnType ( typeof ( object ) ) ]
    [ XamlSetTypeConverter ( "AppShared.CustomResourceLoader" ) ]
    [ UsedImplicitly ]
    public class ObjectIdExtension : MarkupExtension
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;


        /// <summary>Gets or sets the target.</summary>
        /// <value>The target.</value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public object Target { get ; set ; }

        /// <summary>Gets or sets the lifetime scope.</summary>
        /// <value>The lifetime scope.</value>
        public ILifetimeScope LifetimeScope { get ; set ; }


        /// <summary>
        ///     When implemented in a derived class, returns an object that is
        ///     provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">
        ///     A service provider helper that can provide
        ///     services for the markup extension.
        /// </param>
        /// <returns>
        ///     The object value to set on the property where the extension is
        ///     applied.
        /// </returns>
        public override object ProvideValue ( IServiceProvider serviceProvider )
        {
            if ( serviceProvider == null )
            {
                throw new ArgumentNullException ( nameof ( serviceProvider ) ) ;
            }

            var service =
                serviceProvider.GetService (
                                            typeof ( IProvideValueTarget )
                                           ) as IProvideValueTarget ;
            // Logger.Info (
                         // $"{string.Join ( ", " , serviceProvider.GetType ( ).GetInterfaces ( ).Select ( ( type , i ) => type.Name ) )}"
                        // ) ;
            if ( service?.TargetObject != null )
            {
                Logger.Debug ( "TargetObject " + service.TargetObject ) ;
            }

            if ( service?.TargetProperty != null )
            {
                Logger.Debug ( "TargetProperty " + service.TargetProperty ) ;
            }

            if ( LifetimeScope == null )
            {
                const string m = "lifetime scope is null" ;
                Logger.Error ( m ) ;
                return m ;
            }

            var objectIdProvider = LifetimeScope.Resolve < IObjectIdProvider > ( ) ;
            return Target == null
                       ? "Target is null"
                       : objectIdProvider.GetObjectInstanceIdentifier ( Target ) ;
        }
    }
}