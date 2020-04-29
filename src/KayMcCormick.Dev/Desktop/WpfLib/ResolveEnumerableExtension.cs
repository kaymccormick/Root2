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
using Autofac.Features.Metadata;
using KayMcCormick.Dev ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    [ MarkupExtensionReturnType ( typeof ( IEnumerable ) ) ]
    public sealed class ResolveEnumerableExtension : MarkupExtension
    {
        private bool _withMetadata;
        private readonly Type _componentType ;

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
        // ReSharper disable once UnusedMember.Global
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

        public bool WithMetadata { get => _withMetadata; set => _withMetadata = value; }

        #region Overrides of MarkupExtension
        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once AnnotateNotNullTypeMember
        public override object ProvideValue ( IServiceProvider serviceProvider )
        {
            const string resolveEnumerableExtensionName = "ResolveEnumerableExtension" ;
            GlobalDiagnosticsContext.Set ( "CurrentOperation" , resolveEnumerableExtensionName ) ;
            using ( MappedDiagnosticsLogicalContext.SetScoped (
                                                               "CurrentOperation"
                                                             , resolveEnumerableExtensionName
                                                              ) )
            {
                try
                {
                    // ReSharper disable once UnusedVariable
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
                        if (ComponentType ==null )
                            return null;
                        IEnumerable p1;
                        if (ParameterType != null) {
                            var funcType =
                                 typeof(Func<,>).MakeGenericType(
                                                                       ParameterType
                                                                     , ComponentType
                                                                      );
                            var enumerablefuncType =                             typeof(IEnumerable<>).MakeGenericType(funcType);
                            p1 = (IEnumerable)scope.Resolve(enumerablefuncType);
                        }
                        else
                        {
                            var tt = ComponentType;
                            if (WithMetadata)
                            {
                                tt = typeof(Meta<>).MakeGenericType(new[] { tt });
                            }
                            var funcType =
                                typeof(Func<>).MakeGenericType(tt
                                                                       
                                                                      );
                            var enumerablefuncType = 
                            typeof(IEnumerable<>).MakeGenericType(funcType);
                            p1 = (IEnumerable)scope.Resolve(enumerablefuncType);

                        }
                        IList l = new ArrayList ( ) ;
                        foreach ( var x in p1 )
                        {
                            try
                            {
                                DebugUtils.WriteLine($"{x}");
                                var x1 = ( Delegate ) x ;
                                object result = null;
                                if (ParameterType != null)
                                    result = x1.DynamicInvoke(ParameterValue);
                                else
                                    result = x1.DynamicInvoke();
                                DebugUtils.WriteLine($"{result}");
                                l.Add ( result) ;
                            }
                            catch ( Exception ex )
                            {
                                DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                            }
                        }

                        DebugUtils.WriteLine ($"Count is {l.Count}");
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