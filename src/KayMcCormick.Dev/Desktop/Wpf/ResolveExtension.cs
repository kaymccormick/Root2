using System ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Autofac ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResolveExtension : MarkupExtension
    {
        private readonly Type _componentType ;

        public ResolveExtension (Type componentType ) { _componentType = componentType ; }

        #region Overrides of MarkupExtension
        public override object 
            // ReSharper disable once AnnotationRedundancyInHierarchy
            ProvideValue ( [ NotNull ] IServiceProvider serviceProvider )
        {
            if ( serviceProvider == null )
            {
                throw new ArgumentNullException ( nameof ( serviceProvider ) ) ;
            }

            // ReSharper disable once UnusedVariable
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

        }
        #endregion
    }
}