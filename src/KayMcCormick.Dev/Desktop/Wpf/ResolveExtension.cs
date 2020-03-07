using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Autofac ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResolveExtension : MarkupExtension
    {
        private readonly Type _componentType ;

        public ResolveExtension (Type componentType ) { _componentType = componentType ; }

        #region Overrides of MarkupExtension
        public override object 
            ProvideValue ( IServiceProvider serviceProvider )
        {
            var p = ( IAmbientProvider ) serviceProvider.GetService (
                                                                     typeof ( IAmbientProvider )
                                                                    ) ;
            ILifetimeScope scope = null ;
            var service = serviceProvider.GetService ( typeof ( IProvideValueTarget ) ) ;
            if ( service != null )
            {
                var pp = ( IProvideValueTarget ) service ;

                var o = pp.TargetObject ;
                if ( o is DependencyObject d )
                {
                    scope = ( ILifetimeScope ) d.GetValue (
                                                           AttachedProperties.LifetimeScopeProperty
                                                          ) ;
                }
            }
            if(scope == null)
            {
                var svc = serviceProvider.GetService ( typeof ( IRootObjectProvider ) ) ;
                if ( svc != null )
                {
                    var rootP = ( IRootObjectProvider ) svc ;
                    if ( rootP.RootObject is DependencyObject d )
                    {
                        scope = ( ILifetimeScope ) d.GetValue (
                                                               AttachedProperties
                                                                  .LifetimeScopeProperty
                                                              ) ;
                    }
                }

            }

            if ( scope != null )
            {
                return scope.Resolve ( _componentType ) ;
            }
            else
            {
                throw new Exception ( "No lifetime scope" ) ;
            }

            return null ;
        }
        #endregion
    }
}