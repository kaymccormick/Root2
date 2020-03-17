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
using NLog ;
using NLog.Layouts ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class MyJsonLayout : Layout
    {
        private JsonSerializerOptions options;

        /// <summary>
        /// 
        /// </summary>
        public MyJsonLayout()
        {
            var jsonSerializerOptions = CreateJsonSerializerOptions ( ) ;
            //options.Converters.Add ( new DictConverterFactory ( ) ) ;
            Options = jsonSerializerOptions ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonSerializerOptions CreateJsonSerializerOptions ( )
        {
            var jsonSerializerOptions = new JsonSerializerOptions ( ) ;
            
            jsonSerializerOptions.Converters.Add ( new LogEventInfoConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonTypeConverter ( ) ) ;
            return jsonSerializerOptions ;
        }

        /// <summary>
        /// 
        /// </summary>
        public JsonSerializerOptions Options { get => options; set => options = value; }

        #region Overrides of Layout
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logEvent"></param>
        /// <returns></returns>
        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
            return JsonSerializer.Serialize(logEvent, Options);
        }
        #endregion
    }
}