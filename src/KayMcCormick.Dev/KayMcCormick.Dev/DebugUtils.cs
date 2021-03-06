﻿using System ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Threading ;
using System.Threading.Tasks ;
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
        /// <param name="cat"></param>
        /// <param name="filename"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="lineno"></param>
        public static void WriteLine(string line, DebugCategory cat = DebugCategory.Misc, [CallerFilePath] string filename = "",
            [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int lineno = 0)

        {
            
            if ((DisplayCatgories & cat) != cat)
                return;
            var fn = Path.GetFileNameWithoutExtension ( filename ) ;
            var id = Thread.CurrentThread.ManagedThreadId ;
            Console.WriteLine(line);
            var taskId = Task.CurrentId ;
            Debug.WriteLine ( $"<KM> [{id};{taskId}]{fn}:{lineno}[{callerMemberName}] {line}" ) ;
        }

        public static DebugCategory DisplayCatgories { get; set; } = DebugCategory.Misc | DebugCategory.DataBinding | DebugCategory.Ribbon;

        /// <summary>Debugs the format.</summary>
        /// <param name="reg">The reg.</param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static string DebugFormat ( [ JetBrains.Annotations.NotNull ] this IComponentRegistration reg )
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
        [ JetBrains.Annotations.NotNull ]
        public static string DebugFormat ( [ JetBrains.Annotations.NotNull ] this Autofac.Core.Service service )
        {
            if ( service == null )
            {
                throw new ArgumentNullException ( nameof ( service ) ) ;
            }

            return service.Description;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        [Obsolete]
        public static void WriteLine ( [ JetBrains.Annotations.NotNull ] object obj )
        {
            WriteLine ( obj?.ToString ( ) ?? "<null>" ) ;
        }

    }
}