#region header
// Kay McCormick (mccor)
// 
// Deployment
// KayMcCormick.Dev
// MyJsonLayout.cs
// 
// 2020-03-10-4:15 AM
// 
// ---
#endregion
using System.Text.Json ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Serialization ;
using NLog ;
using NLog.Layouts ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// </summary>
    public class MyJsonLayout : Layout
    {
        private JsonSerializerOptions _options ;

        /// <summary>
        /// </summary>
        public MyJsonLayout ( )
        {
            var jsonSerializerOptions = CreateJsonSerializerOptions ( ) ;
            //options.Converters.Add ( new DictConverterFactory ( ) ) ;
            Options = jsonSerializerOptions ;
        }

        /// <summary>
        /// </summary>
        public JsonSerializerOptions Options { get { return _options ; } set { _options = value ; } }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public JsonSerializerOptions CreateJsonSerializerOptions ( )
        {
            var jsonSerializerOptions = new JsonSerializerOptions ( ) ;

            jsonSerializerOptions.Converters.Add ( new JsonConverterLogEventInfo ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonTypeConverterFactory ( ) ) ;
            return jsonSerializerOptions ;
        }

        #region Overrides of Layout
        /// <summary>
        /// </summary>
        /// <param name="logEvent"></param>
        /// <returns></returns>
        protected override string GetFormattedMessage ( LogEventInfo logEvent )
        {
            return JsonSerializer.Serialize ( logEvent , Options ) ;
        }
        #endregion
    }
}