﻿using System ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using Autofac.Core ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    /// <summary>Extension methods for debug output.</summary>
    public static class DebugUtils
    {
        /// <summary>
        /// Basic debug WriteLine method. Populates caller info and includes in output.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="filename"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="lineno"></param>
        public static void WriteLine (string line, [CallerFilePath] string filename = "", [CallerMemberName] string callerMemberName =  "", [CallerLineNumber]int lineno = 0)

        {
            var fn = Path.GetFileNameWithoutExtension ( filename ) ;
            Debug.WriteLine ( $"<KM> {fn}:{lineno}[{callerMemberName}] {line}" ) ;
        }
        /// <summary>Debugs the format.</summary>
        /// <param name="reg">The reg.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DebugFormat
        [ NotNull ]
        public static string DebugFormat ( [ NotNull ] this IComponentRegistration reg )
        {
            if ( reg == null )
            {
                throw new ArgumentNullException ( nameof ( reg ) ) ;
            }

            return string.Join (
                                ", "
                              , reg.Services.Where ( ( service , i ) => true )
                                   .Select ( ( service ,         i ) => service.DebugFormat ( ) )
                               ) ;
        }

        /// <summary>Debugs the format.</summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DebugFormat
        [ NotNull ]
        public static string DebugFormat ( [ NotNull ] this Autofac.Core.Service service )
        {
            if ( service == null )
            {
                throw new ArgumentNullException ( nameof ( service ) ) ;
            }

            return service.Description;
        }

        [Obsolete]
        public static void WriteLine ( object obj )
        {
            WriteLine ( obj.ToString ( ) ) ;
        }
    }
}