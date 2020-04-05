﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Reflection ;
using KayMcCormick.Dev.Logging ;
using NLog ;

namespace KayMcCormick.Dev.TestLib
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for LogHelper
    public static class LogHelper
    {
        private static bool _executed ;

        /// <summary>
        /// </summary>
        public static bool Executed { get { return _executed ; } set { _executed = value ; } }

        /// <summary>
        /// </summary>
        public static bool DisableLogging { get ; set ; }


        /// <summary>
        /// </summary>
        public static void DisableLoggingConfiguration ( )
        {
            DisableLogging = true ;
            Debug.WriteLine ( $"{typeof ( LogHelper ).FullName} - Logging disabled." ) ;
            Environment.SetEnvironmentVariable (
                                                AppLoggingConfigHelper.DisableLoggingEnvVar
                                              , "yes"
                                               ) ;
            AppLoggingConfigHelper.Performant = true ;
            LogManager.Configuration          = new CodeConfiguration ( ) ;
            Executed                          = true ;
        }

        /// <summary>
        /// </summary>
        /// <param name="param1"></param>
        public static void EnsureLoggingConfigured ( LogDelegates.LogMethod param1 = null )
        {
            if ( Executed )
            {
                return ;
            }

            Executed = true ;
            if ( DisableLogging )
            {
                return ;
            }

            Environment.SetEnvironmentVariable (
                                                "DISABLE_LOG_TARGETS"
                                              , ( Environment.GetEnvironmentVariable (
                                                                                      "DISABLE_LOG_TARGETS"
                                                                                     )
                                                  ?? "" )
                                                + ";log"
                                               ) ;


            AppLoggingConfigHelper.EnsureLoggingConfigured ( param1 , new AppLoggingConfiguration() { MinLogLevel = LogLevel.Trace}) ;
            LogManager.LogFactory.ThrowExceptions = true ;
        }

        /// <summary>
        ///     Supplied structured logging properties for a particular test method
        ///     indicated by method.. Supplied properties "TestMethodName", "TestClass",
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="stage">The stage.</param>
        /// <returns></returns>
        /// Return a dictionary with keys
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for TestMethodProperties
        public static IDictionary TestMethodProperties (
            MethodInfo          method
          , TestMethodLifecycle stage
        )
        {
            IDictionary r = new Dictionary < string , object >
                            {
                                [ "TestMethodName" ] = method.Name
                            } ;
            if ( method.DeclaringType != null )
            {
                r[ "TestClass" ] = method.DeclaringType.ToString ( ) ;
            }

            r[ "TestMethodLifecycle" ] = stage ;
            return r ;
        }
    }
}