#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonConverterImage.cs
// 
// 2020-03-19-6:23 PM
// 
// ---
#endregion
using System ;
using System.ComponentModel ;
using System.Reflection ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows.Interop ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Serialization ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    /// <summary>
    /// </summary>
    [ UsedImplicitly ]
    public class JsonImageConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override System.Text.Json.Serialization.JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new MyImageSourceConverter ( typeToConvert , options ) ;
        }
        #endregion
        #region Overrides of JsonConverter
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( ImageSource ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }

        /// <summary>
        /// </summary>
        private class MyImageSourceConverter : JsonConverter < ImageSource >
        {
            private JsonSerializerOptions options ;
            private Type                  typeToConvert ;

            /// <summary>
            /// </summary>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            public MyImageSourceConverter ( Type typeToConvert , JsonSerializerOptions options )
            {
                this.typeToConvert = typeToConvert ;
                this.options       = options ;
            }

            #region Overrides of JsonConverter<ImageSource>
            /// <summary>
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            public override ImageSource Read (
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
                Utf8JsonWriter        writer
              , ImageSource           value
              , JsonSerializerOptions options
            )
            {
                ConverterUtil.WritePreamble ( (JsonConverter)this, writer , value , options ) ;
                writer.WriteStartObject();
                writer.WriteString (
                                    "Converter"
                                  , typeof ( MyImageSourceConverter ).AssemblyQualifiedName
                                   ) ;
                JsonTypeConverter typeConverter = new JsonTypeConverter();
                writer.WritePropertyName("ObjectType");
                typeConverter.Write ( writer , value.GetType ( ) , options ) ;
                writer.WritePropertyName( "ObjectInstance" ) ;
                switch ( value )
                {
                    case D3DImage d3DImage : break ;
                    case InteropBitmap interopBitmap : break ;
                    case DrawingImage drawingImage :
                        JsonDrawingConverter c = new JsonDrawingConverter();
                        c.Write(writer, drawingImage.Drawing, options);
                        break ;
                    case BitmapFrame bitmapFrame : break ;
                    case BitmapImage bitmapImage : break ;
                    case CachedBitmap cachedBitmap : break ;
                    case ColorConvertedBitmap colorConvertedBitmap : break ;
                    case CroppedBitmap croppedBitmap : break ;
                    case FormatConvertedBitmap formatConvertedBitmap : break ;
                    case RenderTargetBitmap renderTargetBitmap : break ;
                    case TransformedBitmap transformedBitmap : break ;
                    case WriteableBitmap writeableBitmap : break ;
                    case BitmapSource bitmapSource : break ;
                    default : throw new ArgumentOutOfRangeException ( nameof ( value ) ) ;
                }
                writer.WriteEndObject();
                
                return ;
                var att = ( ValueSerializerAttribute ) value
                                                      .GetType ( )
                                                      .GetCustomAttribute (
                                                                           typeof (
                                                                               ValueSerializerAttribute
                                                                           )
                                                                          ) ;
                ValueSerializer ser = null ;
                if ( att != null )
                {
                    if ( att.ValueSerializerType != null )
                    {
                        ser = ( ValueSerializer ) Activator.CreateInstance (
                                                                            att.ValueSerializerType
                                                                           ) ;
                    }
                }

                if ( ser != null )
                {
                    var p = TypeDescriptor.GetProvider ( value ) ;
                    var t = p.GetTypeDescriptor ( value ) ;

                    var str = ser.ConvertToString ( value , null ) ;
                    if ( str != null )
                    {
                        writer.WriteStringValue ( str ) ;
                    }
                    else
                    {
                        writer.WriteNullValue ( ) ;
                    }
                }
                else
                {
                    writer.WriteStringValue ( value.ToString ( ) ) ;
                }
            }
            #endregion
        }
        #endregion
    }

    /// <inheritdoc />
    public class JsonDrawingConverter : JsonConverter <Drawing>
    {
        #region Overrides of JsonConverter<Drawing>
        /// <inheritdoc />
        public override Drawing Read ( ref Utf8JsonReader reader , Type typeToConvert , JsonSerializerOptions options ) { return null ; }

        public override void Write (
            Utf8JsonWriter        writer
          , Drawing               value
          , JsonSerializerOptions options
        )
        {
            switch( value )
            {
                case DrawingGroup drawingGroup :
                    foreach ( var drawingGroupChild in drawingGroup.Children )
                    {
                        Write ( writer , value , options ) ;
                    }
                    break ;
                case GeometryDrawing geometryDrawing :
                    writer.WriteStartObject();
                    writer.WritePropertyName ( "Geometry" ) ;
                    var c2 = new JsonGeometryConverter ( ) ;
                    c2.Write(writer, geometryDrawing.Geometry, options);
                    //JsonSerializer.Serialize ( writer , geometryDrawing.Geometry , options ) ;
                    writer.WriteEndObject();
                    break ;
                case GlyphRunDrawing glyphRunDrawing : break ;
                case ImageDrawing imageDrawing : break ;
                case VideoDrawing videoDrawing : break ;
                default : throw new ArgumentOutOfRangeException ( nameof ( value ) ) ;
            }
        }
        #endregion
    }

    /// <inheritdoc />
    public class JsonGeometryConverter :JsonConverter <Geometry>
    {
        #region Overrides of JsonConverter<Geometry>
        /// <inheritdoc />
        public override Geometry Read ( ref Utf8JsonReader reader , Type typeToConvert , JsonSerializerOptions options ) { return null ; }

        /// <inheritdoc />
        public override void Write (
            Utf8JsonWriter        writer
          , Geometry              value
          , JsonSerializerOptions options
        )
        {
            switch ( value )
            {
                case CombinedGeometry combinedGeometry : break ;
                case EllipseGeometry ellipseGeometry : break ;
                case GeometryGroup geometryGroup :
                    break ;

                case LineGeometry lineGeometry :
                    writer.WriteStartObject();
                    writer.WritePropertyName ( "Bounds" ) ;
                    writer.WriteStartArray();
                    writer.WriteNumberValue(lineGeometry.Bounds.Left);
                    writer.WriteNumberValue(lineGeometry.Bounds.Top);
                    writer.WriteNumberValue(lineGeometry.Bounds.Right);
                    writer.WriteNumberValue(lineGeometry.Bounds.Bottom);
                    writer.WriteEndArray();
                    writer.WriteStartArray ( "StartPoint" ) ;
                    writer.WriteNumberValue ( lineGeometry.StartPoint.X ) ;
                    writer.WriteNumberValue(lineGeometry.StartPoint.Y);
                    writer.WriteEndArray();
                    writer.WriteStartArray("EndPoint");
                    writer.WriteNumberValue(lineGeometry.EndPoint.X);
                    writer.WriteNumberValue(lineGeometry.EndPoint.Y);
                    writer.WriteEndArray();
                    writer.WriteEndObject();
                    break ;
                case PathGeometry pathGeometry : break ;
                case RectangleGeometry rectangleGeometry : break ;
                case StreamGeometry streamGeometry : break ;
                default : throw new ArgumentOutOfRangeException ( nameof ( value ) ) ;
            }

        }
        #endregion
    }
}