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
using System.Diagnostics ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Autofac ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using IEnumerable = System.Collections.IEnumerable ;

namespace KayMcCormick.Lib.Wpf
{
    [ MarkupExtensionReturnType ( typeof ( IEnumerable ) ) ]
    public class ResolveEnumerableExtension : MarkupExtension
    {
        private Type _componentType ;

        public ResolveEnumerableExtension ( ) { }

        // public static readonly DependencyProperty ParameterProperty =
        //     DependencyProperty.Register (
        //                                  "Parameter"
        //                                , typeof ( object )
        //                                , typeof ( ResolveEnumerableExtension )
        //
        //                                 ) ;
        private Type   _parameterType ;
        private object _parameterValue ;

        public ResolveEnumerableExtension ( Type componentType )
        {
            _componentType = componentType ;
        }

        public Type ComponentType
        {
            get { return _componentType ; }
            set { _componentType = value ; }
        }

        public object ParameterValue
        {
            get { return _parameterValue ; }
            set { _parameterValue = value ; }
        }

        public Type ParameterType
        {
            get { return _parameterType ; }
            set { _parameterType = value ; }
        }

        #region Overrides of MarkupExtension
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
                                var x1 = ( Delegate ) x ;
                                var result = x1.DynamicInvoke ( ParameterValue ) ;
                                l.Add ( result ) ;
                            }
                            catch ( Exception ex )
                            {
                                Debug.WriteLine ( ex.ToString ( ) ) ;
                            }
                        }

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
                    GlobalDiagnosticsContext.Remove("CurrentOperation");
                }
            }
        }
        #endregion
    }
}