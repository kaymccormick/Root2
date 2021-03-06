﻿using System ;
using System.CodeDom.Compiler ;
using System.Collections ;
using System.Collections.Generic ;
using System.Diagnostics.CodeAnalysis ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Xml.Serialization ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.StackTrace ;
using NLog ;
using NLog.Fluent ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// </summary>
    public static class Utils
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private static readonly BinaryFormatter BinaryFormatter = new BinaryFormatter ( ) ;


        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        /// <param name="level"></param>
        /// <param name="logger"></param>
        [ SuppressMessage (
                              "Design"
                            , "CA1031:Do not catch general exception types"
                            , Justification = "<Pending>"
                          ) ]
        // ReSharper disable once UnusedMember.Global
        public static void HandleInnerExceptions (
            [ CanBeNull ] Exception e
          , [ CanBeNull ] LogLevel  level  = null
          , [ CanBeNull ] ILogger   logger = null
        )
        {
            using ( var stringWriter = new StringWriter ( ) )
            {
                TextWriter s = stringWriter ;
                try
                {
                    void DoLog ( Exception exception )
                    {
                        new LogBuilder ( logger ?? Logger )
                           .Level ( level ?? LogLevel.Debug )
                           .Exception ( exception )
                           .Message ( exception.Message )
                           .Write ( ) ;
                    }

                    if ( e == null )
                    {
                        return ;
                    }

                    s.WriteLine ( e.Message ) ;
                    var inner = e.InnerException ;
                    var seen = new HashSet < object > ( ) ;
                    while ( inner != null
                            && ! seen.Contains ( inner ) )
                    {
                        DoLog ( inner ) ;
                        seen.Add ( inner ) ;
                        inner = inner.InnerException ;
                    }
                }
                catch ( Exception ex )
                {
                    DebugUtils.WriteLine ( "Exception: " + ex ) ;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="out"></param>
        public static void PerformLogConfigDump ( TextWriter @out )
        {
            var doDumpConfig = AppLoggingConfigHelper.DoDumpConfig ( s => { } ) ;
            using ( var writer = new IndentedTextWriter ( @out ) )
            {
                DoDump ( writer , doDumpConfig ) ;
            }
        }

        private static void DoDump (
            IndentedTextWriter dumpConfig
          , [ JetBrains.Annotations.NotNull ] IDictionary        doDumpConfig
            // ReSharper disable once UnusedParameter.Local
          , int                depth = 0
        )
        {
            foreach ( var key in doDumpConfig.Keys )
            {
                var @out = key.ToString ( ) ;
                var value = doDumpConfig[ key ] ;
                if ( ! ( value is IDictionary || value is string )
                     && value is IEnumerable e )
                {
                    var d = new Dictionary < int , object > ( ) ;
                    var i = 0 ;
                    foreach ( var yy in e )
                    {
                        d[ i ] = yy ;
                        i ++ ;
                    }

                    value = d ;
                }

                if ( value is IDictionary dict )
                {
                    dumpConfig.WriteLine ( $"{key}:" ) ;
                    dumpConfig.Indent += 1 ;
                    DoDump ( dumpConfig , dict , depth + 1 ) ;
                    dumpConfig.Indent -= 1 ;
                }
                else
                {
                    @out = @out + " = " + value ;
                    dumpConfig.WriteLine ( @out ) ;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static IEnumerable < StackTraceEntry > ParseStackTrace ( [ JetBrains.Annotations.NotNull ] string text )
        {
            return StackTraceParser.Parse (
                                           text
                                         , ( idx , len , txt )
                                               => new StackTraceToken
                                                  {
                                                      Index = idx , Length = len , Text = txt
                                                  }
                                         , ( type , method )
                                               => new StackTraceMethod
                                                  {
                                                      Type = type , Method = method
                                                  }
                                         , ( type , name )
                                               => new StackTraceParameter
                                                  {
                                                      Type = type , Name = name
                                                  }
                                         , ( pl , ps )
                                               => new StackTraceParams
                                                  {
                                                      List = pl , Parameters = ps
                                                  }
                                         , ( file , line )
                                               => new StackTraceSourceLocation
                                                  {
                                                      File = file , Line = line
                                                  }
                                         , ( f , tm , p , fl )
                                               => new StackTraceEntry
                                                  {
                                                      Frame         = f
                                                    , Type          = tm.Type
                                                    , Method        = tm.Method
                                                    , ParameterList = p.List
                                                    , Parameters    = p.Parameters.ToList ( )
                                                    , File          = fl.File
                                                    , Line          = fl.Line
                                                  }
                                          ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="eException"></param>
        public static void LogParsedExceptions ( [ JetBrains.Annotations.NotNull ] Exception eException )
        {
            DebugUtils.WriteLine ( eException.GetType ( ).FullName ) ;
            DebugUtils.WriteLine ( eException.Message ) ;
            DebugUtils.WriteLine ( eException.StackTrace ) ;
            if ( eException is FileNotFoundException )
            {
                    return ;

            }

            if ( eException is FileLoadException
                 || eException is TargetInvocationException
                 || eException is FormatException
                 || eException.StackTrace?.Contains ( "CurrentDomain_FirstChanceException" ) == true )
            {
                return ;
            }

            var s = new MemoryStream ( ) ;
            try
            {
                BinaryFormatter.Serialize ( s , eException ) ;
                s.Flush ( ) ;
                s.Seek ( 0 , SeekOrigin.Begin ) ;

                var bytes = new byte[ s.Length ] ;
                var sLength = ( int ) s.Length ;

                s.Read ( bytes , 0 , sLength ) ;

                var parsed = GenerateParsedException ( eException ) ;

                var serializer = new XmlSerializer ( typeof ( ParsedExceptions ) ) ;
                var sw = new StringWriter ( ) ;
                serializer.Serialize ( sw , parsed ) ;
#if TRACEPROVIDER
                PROVIDER_GUID.EventWriteEXCEPTION_RAISED_EVENT (
                                                                eException
                                                                   .GetType ( )
                                                                   .AssemblyQualifiedName
                                                              , eException.StackTrace
                                                                ?? "No stacktrace"
                                                              , eException.Message
                                                              , ( uint ) s.Length
                                                              , bytes
                                                              , sw.ToString ( )
                                                               ) ;
#endif
                DebugUtils.WriteLine ( eException.ToString ( ) ) ;
            }
            catch ( Exception )
            {
                // ignored
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="eException"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static ParsedExceptions GenerateParsedException ( Exception eException )
        {
            var parsed = new ParsedExceptions ( ) ;

            void HandleException ( Exception exception1 , ParsedExceptions parsedEx )
            {
                var set = new HashSet < Exception > ( ) ;
                while ( exception1 != null
                        && ! set.Contains ( exception1 ) )
                {
                    if ( ! string.IsNullOrEmpty ( exception1.StackTrace ) )
                    {
                        var parsed1 = ParseStackTrace ( exception1.StackTrace ) ;
                        var si = new ParsedStackInfo (
                                                      parsed1
                                                    , exception1.GetType ( ).AssemblyQualifiedName
                                                    , exception1.Message
                                                     ) ;
                        parsedEx.ParsedList.Add ( si ) ;
                    }

                    set.Add ( exception1 ) ;
                    exception1 = exception1.InnerException ;
                }
            }

            if ( eException is AggregateException ag )
            {
                var flattened = ag.Flatten ( ) ;
                Exception ex = flattened ;
                if ( ! string.IsNullOrEmpty ( ex.StackTrace ) )
                {
                    var parsed1 = ParseStackTrace ( ex.StackTrace ) ;
                    var si = new ParsedStackInfo (
                                                  parsed1
                                                , ex.GetType ( ).AssemblyQualifiedName
                                                , ex.Message
                                                 ) ;
                    parsed.ParsedList.Add ( si ) ;
                }

                var ex1 = flattened.InnerExceptions ;
                foreach ( var exception in ex1 )
                {
                    HandleException ( exception , parsed ) ;
                }
            }
            else
            {
                HandleException ( eException , parsed ) ;
            }

            return parsed ;
        }
    }
}