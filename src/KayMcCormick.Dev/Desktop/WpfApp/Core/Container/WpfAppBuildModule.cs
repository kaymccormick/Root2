#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// WpfApp
// WpfAppBuildModule.cs
// 
// 2020-03-12-1:58 AM
// 
// ---
#endregion
using System ;
using System.Linq ;
using System.Reflection ;
using System.Windows ;
using Autofac ;
using NLog ;
using Module = Autofac.Module ;

namespace KayMcCormick.Dev
{
    public class WpfAppBuildModule : Module
    {
        private static bool _doTraceConditionalRegistration ;
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public static void WindowReg ( ContainerBuilder builder , Assembly[] toScan )
        {
            builder.RegisterAssemblyTypes ( toScan )
                   .Where ( MainScanningPredicate )
                   .AsImplementedInterfaces ( ) ;
            builder.RegisterAssemblyTypes ( toScan.ToArray ( ) )
                   .Where (
                           delegate ( Type t ) {
                               var isAssignableFrom = typeof ( Window ).IsAssignableFrom ( t ) ;
                               TraceConditionalRegistration ( t , typeof ( Window ) , isAssignableFrom ) ;
                               return isAssignableFrom ;
                           }
                          )
                   .AsSelf ( )
                   .As < Window > ( )
                   .OnActivating (
                                  args => {
                                      var argsInstance = args.Instance ;

                                      if ( argsInstance is IHaveLogger haveLogger )
                                      {
                                          haveLogger.Logger =
                                              args.Context.Resolve < ILogger > (
                                                                                new TypedParameter (
                                                                                                    typeof (
                                                                                                        Type
                                                                                                    )
                                                                                                  , argsInstance
                                                                                                       .GetType ( )
                                                                                                   )
                                                                               ) ;
                                      }
                                  }
                                 ) ;
        }

        private static bool MainScanningPredicate ( Type arg )
        {
            var r = false ; //typeof ( ITabGuest ).IsAssignableFrom ( arg ) ;
            if ( AppBuildModule.DoTraceConditionalRegistration )
            {
                Logger.Trace ( $"Conditional registration for {arg} is {r}" ) ;
            }

            return r ;
        }

        private static void TraceConditionalRegistration (
            Type type
          , Type type1
          , bool isAssignableFrom
        )
        {
            if ( DoTraceConditionalRegistration )
            {
                Logger.Trace (
                                             $"Conditional registration for {type} is {isAssignableFrom} [{type1}]"
                                            ) ;
            }
        }

        public static bool DoTraceConditionalRegistration { get { return _doTraceConditionalRegistration ; } set { _doTraceConditionalRegistration = value ; } }
    }
}