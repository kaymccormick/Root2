#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// ResolveEnumerableExtension.cs
// 
// 2020-03-22-2:17 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    [ MarkupExtensionReturnType ( typeof ( IEnumerable ) ) ]
    public class ResolveEnumerableExtension : MarkupExtension
    {
        private Type _componentType ;

        // public static readonly DependencyProperty ParameterProperty =
        //     DependencyProperty.Register (
        //                                  "Parameter"
        //                                , typeof ( object )
        //                                , typeof ( ResolveEnumerableExtension )
        //
        //                                 ) ;
        private Type   _parameterType ;
        private object _parameterValue ;

        /// <summary>
        /// </summary>
        public ResolveEnumerableExtension ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="componentType"></param>
        public ResolveEnumerableExtension ( Type componentType )
        {
            _componentType = componentType ;
        }

        /// <summary>
        /// </summary>
        public Type ComponentType
        {
            get { return _componentType ; }
            set { _componentType = value ; }
        }

        /// <summary>
        /// </summary>
        public object ParameterValue
        {
            get { return _parameterValue ; }
            set { _parameterValue = value ; }
        }

        /// <summary>
        /// </summary>
        public Type ParameterType
        {
            get { return _parameterType ; }
            set { _parameterType = value ; }
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
            var resolveEnumerableExtensionName = "ResolveEnumerableExtension" ;
            GlobalDiagnosticsContext.Set ( "CurrentOperation" , resolveEnumerableExtensionName ) ;
            using ( MappedDiagnosticsLogicalContext.SetScoped (
                                                               "CurrentOperation"
                                                             , resolveEnumerableExtensionName
                                                              ) )
            {
                try
                {
                    if ( serviceProvider == null )
                    {
                        throw new ArgumentNullException ( nameof ( serviceProvider ) ) ;
                    }


                    var p = ( IAmbientProvider ) serviceProvider.GetService (
                                                                             typeof (
                                                                                 IAmbientProvider )
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
                                                                   AttachedProperties
                                                                      .LifetimeScopeProperty
                                                                  ) ;
                        }
                    }

                    if ( scope == null )
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
                        DebugUtils.WriteLine($"Scope is {scope}");
                        var funcType =
                            typeof ( Func < , > ).MakeGenericType (
                                                                   ParameterType
                                                                 , ComponentType
                                                                  ) ;
                        var enumerablefuncType =
                            typeof ( IEnumerable <> ).MakeGenericType ( funcType ) ;
                        var p1 = ( IEnumerable ) scope.Resolve ( enumerablefuncType ) ;
                        IList l = new ArrayList ( ) ;
                        foreach ( var x in p1 )
                        {
                            try
                            {
                                DebugUtils.WriteLine($"{x}");
                                var x1 = ( Delegate ) x ;
                                var result = x1.DynamicInvoke ( ParameterValue ) ;
                                DebugUtils.WriteLine($"{result}");
                                l.Add ( result ) ;
                            }
                            catch ( Exception ex )
                            {
                                DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                            }
                        }

                        DebugUtils.WriteLine ($"Count is {l.Count}");
                        return l ;
                        var provideValue = scope.Resolve (
                                                          typeof ( IEnumerable <> )
                                                             .MakeGenericType ( ComponentType )
                                                         ) ;
                        if ( provideValue is IEnumerable e )
                        {
                            foreach ( var o in e )
                            {
                                l.Add ( o ) ;
                            }
                        }

                        return l ;
                    }
                    else
                    {
                        throw new Exception ( "No lifetime scope" ) ;
                    }
                }
                finally
                {
                    GlobalDiagnosticsContext.Remove ( "CurrentOperation" ) ;
                }
            }
        }
        #endregion
    }
}