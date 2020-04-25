#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// LogEventMisc.cs
// 
// 2020-04-23-8:22 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Text.Json ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Logging ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LogEventMisc : IEventMisc
    {
        /// <inheritdoc />

        public int ThreadId => _inst.ManagedThreadId ;

        /// <inheritdoc />
        [ NotNull ] public string PropKeys
        {
            get
            {
                var o = JsonSerializer.Deserialize < JsonElement > ( RawJson ) ;
                List <string> ks=new List < string > ();
                if ( o.TryGetProperty ( "Properties" , out var props ) )
                {
                    foreach ( var keyValuePair in props.EnumerateObject ( ) )
                    {
                        if ( keyValuePair.NameEquals ( "CallerFilePath" )
                             || keyValuePair.NameEquals ( "CallerMemberName" )
                             || keyValuePair.NameEquals("CallerLineNumber"))
                        {
                            continue ;
                        }
                        ks.Add ( keyValuePair .ToString()) ;
                    }
                }
                if (o.TryGetProperty("MDLC", out var mdlc))
                {
                    foreach (var keyValuePair in mdlc.EnumerateObject())
                    {
                        ks.Add(keyValuePair.ToString());
                    }
                }
                return string.Join (
                                    ";",ks);
            }
        }

        /// <inheritdoc />
        public string File => _inst.CallerFilePath ;

        private readonly LogEventInstance _inst ;
        private          MiscLevel        _level = MiscLevel.INFO ;
        private          string           _rawJson ;
        #region Implementation of IEventMisc
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="rawJson"></param>
        public LogEventMisc (LogEventInstance inst , string rawJson)
        {
            _inst    = inst ;
            _rawJson = rawJson ;
        }

        /// <inheritdoc />
        public object Obj => _inst ;

        /// <inheritdoc />
        public string Message => _inst.FormattedMessage ;

        /// <inheritdoc />
        public MiscLevel Level { get { return _level ; } set { _level = value ; } }

        /// <inheritdoc />
        public string RawJson { get { return _rawJson ; } set { _rawJson = value ; } }
        #endregion
    }
}