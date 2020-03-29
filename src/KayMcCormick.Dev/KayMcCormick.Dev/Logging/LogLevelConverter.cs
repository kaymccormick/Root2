using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    ///     NLog.LogLevel converter for  System.Text.JSON.
    ///     Converts the log level to a string and for writing and vicee versa for
    ///     reading.
    ///     Current format used orginal log levels output.
    /// </summary>
    //KM2020-3-26 ReSharper disable once UnusedType.Global
    public class LogLevelConverter : JsonConverter < LogLevel >

    {
        #region Overrides of JsonConverter<LogLevel>
        /// <summary>
        ///     Read implementation
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override LogLevel Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return LogLevel.FromString ( reader.GetString ( ) ) ;
        }


        /// <summary>
        ///     WRite implementation
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void Write (
            [ NotNull ] Utf8JsonWriter writer
          , [ NotNull ] LogLevel       value
          , JsonSerializerOptions      options
        )
        {
            if ( writer == null )
            {
                throw new ArgumentNullException ( nameof ( writer ) ) ;
            }

            if ( value == null )
            {
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            writer.WriteStringValue ( value.Name ) ;
        }
        #endregion
    }
}