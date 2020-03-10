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
            Options = new JsonSerializerOptions();
            Options.Converters.Add(new LogEventInfoConverter());
            Options.Converters.Add(new JsonTypeConverter());
            //options.Converters.Add ( new DictConverterFactory ( ) ) ;
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