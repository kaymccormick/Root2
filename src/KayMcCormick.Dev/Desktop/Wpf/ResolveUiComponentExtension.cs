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
    /// <summary>
    /// </summary>
    [ TypeConverter ( typeof ( ResolveUiComponentTypeConverter ) ) ]
    [ MarkupExtensionReturnType ( typeof ( UIElement ) ) ]
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
        private ILifetimeScope _lifetimeScope ;
        private string         _name ;

        /// <summary>
        /// </summary>
        public ResolveUiComponentExtension ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="componentType"></param>
        public ResolveUiComponentExtension ( Type componentType )
        {
            ComponentType = componentType ;
        }

        /// <summary>
        /// </summary>
        public Type ComponentType { get ; set ; }

        /// <summary>
        /// </summary>
        public ILifetimeScope LifetimeScope
        {
            get { return _lifetimeScope ; }
            set { _lifetimeScope = value ; }
        }

        /// <summary>
        /// </summary>
        public string Name { get { return _name ; } set { _name = value ; } }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static ResolveUiComponentExtension CreateInstance ( )
        {
            return new ResolveUiComponentExtension ( ) ;
        }

        #region Overrides of MarkupExtension
        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public override object ProvideValue ( [ NotNull ] IServiceProvider serviceProvider )
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
                    rootObject = rootObjectProvider.RootObject ;
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
                            var value = dependencyObject.GetValue ( NameScope.NameScopeProperty ) ;
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
                var result = scope.Resolve ( ComponentType ) ;
                if ( Name != null )
                {
                    nameScope?.RegisterName ( Name , result ) ;
                }

                return result ;
            }

            throw new Exception ( "No lifetime scope" ) ;
        }
        #endregion
    }
}