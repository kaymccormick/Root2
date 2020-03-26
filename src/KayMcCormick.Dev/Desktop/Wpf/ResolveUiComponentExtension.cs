#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// ResolveUiComponentExtension.cs
// 
// 2020-03-17-12:02 PM
// 
// ---
#endregion
using System ;
using System.ComponentModel ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Autofac ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    [TypeConverter( typeof(ResolveUiComponentTypeConverter))]
    [MarkupExtensionReturnType( typeof(UIElement))]
    public class ResolveUiComponentExtension : MarkupExtension
    {
        // public T FromResolveUiComponentExtension < T > () where T : DependencyObject
        // {
            // return (T)_lifetimeScope.Resolve(_componentType);
        // }
         // public static implicit operator DependencyObject ( ResolveUiComponentExtension ext )
         // {
             // return (DependencyObject)ext._lifetimeScope.Resolve(ext._componentType);
        // }
        private Type _componentType ;
        private ILifetimeScope _lifetimeScope ;
        private string _name ;

        public ResolveUiComponentExtension ( ) {
        }

        public ResolveUiComponentExtension (Type componentType ) { _componentType = componentType ; }

        public Type ComponentType
        {
            get { return _componentType ; }
            set { _componentType = value ; }
        }

        public ILifetimeScope LifetimeScope
        {
            get { return _lifetimeScope ; }
            set { _lifetimeScope = value ; }
        }

        public string Name {
            get {
                return _name;
            }
            set { _name = value ; }
        }

        #region Overrides of MarkupExtension
        public override object 
            
            ProvideValue ( [ NotNull ] IServiceProvider serviceProvider )
        {
            if ( serviceProvider == null )
            {
                throw new ArgumentNullException ( nameof ( serviceProvider ) ) ;
            }

            
            var p = ( IAmbientProvider ) serviceProvider.GetService (
                                                                     typeof ( IAmbientProvider )
                                                                    ) ;
            ILifetimeScope scope = null ;
            INameScope nameScope = null ;
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
                    nameScope = ( NameScope ) d.GetValue ( NameScope.NameScopeProperty ) ;
                    
                }
            }



            object rootObject = null ;
            if ( scope        == null
                 || nameScope == null )
            {
                var svc = serviceProvider.GetService ( typeof ( IRootObjectProvider ) ) ;

                if ( svc != null )
                {
                    var rootObjectProvider = ( IRootObjectProvider ) svc ;
                    rootObject = rootObjectProvider.RootObject;
                    if ( rootObject is DependencyObject dependencyObject )
                    {
                        if ( scope == null )
                        {
                            scope = ( ILifetimeScope ) dependencyObject.GetValue (
                                                                                  AttachedProperties
                                                                                     .LifetimeScopeProperty
                                                                                 ) ;

                        }

                        if ( nameScope == null )
                        {
                            var value = dependencyObject.GetValue (
                                                                   NameScope
                                                                      .NameScopeProperty
                                                                  ) ;
                            if ( value != null )
                            {
                                nameScope = ( INameScope ) value ;
                            }
                        }
                    }
                }
            }
            if ( scope != null )
            {
                var result = scope.Resolve ( _componentType ) ;
                if ( Name != null )
                {
                    if ( nameScope != null )
                    {
                        nameScope.RegisterName ( Name , result ) ;
                    }
                }

                return result ;
            }
            else
            {
                throw new Exception ( "No lifetime scope" ) ;
            }

        }
        #endregion
    }
}