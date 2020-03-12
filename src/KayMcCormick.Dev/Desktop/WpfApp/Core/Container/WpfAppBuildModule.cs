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
using System.Collections.Generic ;
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private                 bool   _doTraceConditionalRegistration ;

        public bool DoTraceConditionalRegistration
        {
            get => _doTraceConditionalRegistration ;
            set => _doTraceConditionalRegistration = value ;
        }

        public List < Assembly > AssembliesForScanning { get ; } = new List < Assembly > ( ) ;

        private void WindowReg ( ContainerBuilder builder , ICollection < Assembly > toScan )
        {
            builder.RegisterAssemblyTypes ( toScan.ToArray ( ) )
                   .Where ( MainScanningPredicate )
                   .AsImplementedInterfaces ( ) ;
            builder.RegisterAssemblyTypes ( toScan.ToArray ( ) )
                   .Where (
                           delegate ( Type t ) {
                               var isAssignableFrom = typeof ( Window ).IsAssignableFrom ( t ) ;
                               TraceConditionalRegistration (
                                                             t
                                                           , typeof ( Window )
                                                           , isAssignableFrom
                                                            ) ;
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
                                                                                                    typeof
                                                                                                    ( Type
                                                                                                    )
                                                                                                  , argsInstance
                                                                                                       .GetType ( )
                                                                                                   )
                                                                               ) ;
                                      }
                                  }
                                 ) ;
        }

        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            WindowReg ( builder , AssembliesForScanning ) ;
        }
        #endregion

        private static bool MainScanningPredicate ( Type arg )
        {
            var r = false ; //typeof ( ITabGuest ).IsAssignableFrom ( arg ) ;
            if ( AppBuildModule.DoTraceConditionalRegistration )
            {
                Logger.Trace ( $"Conditional registration for {arg} is {r}" ) ;
            }

            return r ;
        }

        private  void TraceConditionalRegistration (
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
    }
}