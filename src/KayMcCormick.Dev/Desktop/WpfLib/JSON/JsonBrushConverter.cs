#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonBrushConverter.cs
// 
// 2020-03-20-2:55 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows.Media ;
using JetBrains.Annotations ;

// ReSharper disable UnusedVariable

namespace KayMcCormick.Lib.Wpf.JSON
{
    /// <summary>
    /// </summary>
    [ UsedImplicitly ]
    public class JsonBrushConverter : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( Brush ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ NotNull ]
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonBrushConverter1 ( typeToConvert ) ;
        }

        /// <summary>
        /// </summary>
        private sealed class JsonBrushConverter1 : JsonConverter < Brush >
        {
            /// <summary>
            /// </summary>
            /// <param name="typeToConvert"></param>
            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once UnusedParameter.Local
            public JsonBrushConverter1 ( Type typeToConvert ) { }

            #region Overrides of JsonConverter<Brush>
            /// <summary>
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            [ CanBeNull ]
            public override Brush Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            /// <summary>
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="value"></param>
            /// <param name="options"></param>
            public override void Write (
                [ NotNull ] Utf8JsonWriter        writer
              , [ NotNull ] Brush                 value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStartObject ( ) ;
                writer.WriteString ( "Converter" , typeof ( JsonBrushConverter1 ).FullName ) ;
                writer.WriteString ( "BrushType" , value.GetType ( ).FullName ) ;
                switch ( value )
                {
                    // ReSharper disable once UnusedVariable
                    case BitmapCacheBrush bitmapCacheBrush : break ;
                    // ReSharper disable once UnusedVariable
                    case DrawingBrush drawingBrush : break ;
                    case LinearGradientBrush linearGradientBrush :
                        WriteBaseGradient(writer, linearGradientBrush);
                        break ;
                    case RadialGradientBrush radialGradientBrush :
                        WriteBaseGradient(writer, radialGradientBrush);
                        break ;
                    case ImageBrush imageBrush : break ;
                    case SolidColorBrush scb :
                        writer.WritePropertyName("Color");
                        WriteColor ( writer , scb.Color ) ;
                        break ;
                    case VisualBrush visualBrush : break ;
                    case TileBrush tileBrush : break ;
                }

                writer.WriteEndObject ( ) ;
            }

            private static void WriteBaseGradient ( [ NotNull ] Utf8JsonWriter writer , [ NotNull ] GradientBrush gradientBrush )
            {
                writer.WriteNumber ( "ColorInterpolationMode" , ( int ) gradientBrush.ColorInterpolationMode ) ;
                writer.WriteStartArray ( "GradientStops" ) ;
                foreach ( var stop in gradientBrush.GradientStops )
                {
                    writer.WriteStartObject ( ) ;
                    writer.WritePropertyName ( "Color" ) ;
                    WriteColor ( writer , stop.Color ) ;
                    writer.WriteNumber ( "Offset" , stop.Offset ) ;
                    writer.WriteEndObject ( ) ;
                }

                writer.WriteEndArray ( ) ;
            }

            private static void WriteColor ( [ NotNull ] Utf8JsonWriter writer , Color color )
            {
                writer.WriteStartArray ( ) ;
                writer.WriteNumberValue ( color.ScA ) ;
                writer.WriteNumberValue ( color.ScR ) ;
                writer.WriteNumberValue ( color.ScG ) ;
                writer.WriteNumberValue ( color.ScB ) ;
                writer.WriteEndArray ( ) ;
            }
            #endregion
        }
        #endregion
    }
}