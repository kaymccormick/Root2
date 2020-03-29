#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonFrameworkElementConverter.cs
// 
// 2020-03-23-7:16 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Xunit.Abstractions ;
using XamlWriter = System.Xaml.XamlWriter ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    public enum WriteContext { PropertyName , PropertyValue , InArray }

    public class JsonFrameworkElementConverter : JsonConverterFactory
    {
        public JsonFrameworkElementConverter ( ITestOutputHelper output ) { Output = output ; }

        public ITestOutputHelper Output { get ; }

        #region Overrides of JsonConverter
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( FrameworkElement ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion

        internal class JsonWriter : XamlWriter
        {
            private readonly MemoryStream      _memoryStream ;
            private readonly ITestOutputHelper _output ;
            private readonly Utf8JsonWriter    _writer ;
            private readonly XamlObjectReader  _xamlObjectReader ;

            private readonly Stack < WriteContext > _context = new Stack < WriteContext > ( ) ;
            private          bool                   _inMember ;
            private          bool                   _inObject ;
            private readonly XamlSchemaContext      _schemaContext ;
            private          bool                   _wroteNull ;

            public JsonWriter (
                Utf8JsonWriter    writer
              , XamlSchemaContext schemaContext
              , XamlObjectReader  xamlObjectReader
              , WriteContext      writeContext
              , MemoryStream      memoryStream
              , ITestOutputHelper output
            )
            {
                _context.Push ( writeContext ) ;
                output.WriteLine ( _context.Count.ToString ( ) ) ;
                _writer           = writer ;
                _schemaContext    = schemaContext ;
                _xamlObjectReader = xamlObjectReader ;
                _memoryStream     = memoryStream ;
                _output           = output ;
            }

            #region Overrides of XamlWriter
            public override void WriteGetObject ( )
            {
                throw new NotImplementedException ( ) ;
                switch ( _context.Peek ( ) )
                {
                    case WriteContext.PropertyName :
                    case WriteContext.PropertyValue :
                        break ;
                    case WriteContext.InArray : break ;
                    default :                   throw new ArgumentOutOfRangeException ( ) ;
                }

                _writer.WriteStartArray ( "Get" ) ;
                _writer.WriteEndArray ( ) ;
            }

            public override void WriteStartObject ( XamlType type )
            {
                _writer.Flush ( ) ;
                var str = Encoding.UTF8.GetString (
                                                   _memoryStream.GetBuffer ( )
                                                 , 0
                                                 , ( int ) _memoryStream.Length
                                                  ) ;
                _output.WriteLine ( str ) ;

                if ( type.IsMarkupExtension
                     && type.UnderlyingType == typeof ( NullExtension ) )
                {
                    _output.WriteLine ( _context.Count.ToString ( ) ) ;
                    switch ( _context.Peek ( ) )
                    {
                        case WriteContext.PropertyName : throw new InvalidOperationException ( ) ;
                        case WriteContext.PropertyValue :
                            _writer.WriteNullValue ( ) ;
                            _context.Pop ( ) ;
                            _context.Push ( WriteContext.PropertyName ) ;
                            break ;
                        case WriteContext.InArray :
                            _writer.WriteNullValue ( ) ;
                            break ;
                    }
                }
                else

                {
                    _output.WriteLine ( _context.Count.ToString ( ) ) ;
                    switch ( _context.Peek ( ) )
                    {
                        case WriteContext.PropertyName :
                            _writer.WriteStartObject ( type.ToString ( ) ) ;
                            _context.Push ( WriteContext.PropertyName ) ;
                            break ;
                        case WriteContext.PropertyValue :
                        case WriteContext.InArray :
                            _writer.WriteStartObject ( ) ;
                            _context.Push ( WriteContext.PropertyName ) ;
                            break ;
                        default : throw new ArgumentOutOfRangeException ( ) ;
                    }
                }
            }

            public override void WriteEndObject ( )
            {
                _output.WriteLine ( _context.Count.ToString ( ) ) ;
                switch ( _context.Peek ( ) )
                {
                    case WriteContext.PropertyName :
                        _writer.WriteEndObject ( ) ;
                        _context.Pop ( ) ;
                        break ;
                    case WriteContext.PropertyValue :
                        throw new InvalidOperationException ( ) ;
                        break ;
                    case WriteContext.InArray :
                        throw new InvalidOperationException ( ) ;
                        break ;
                    default : throw new ArgumentOutOfRangeException ( ) ;
                }
            }

            public override void WriteStartMember ( XamlMember xamlMember )
            {
                _output.WriteLine (
                                   $"{nameof ( WriteStartMember )}: {_context.Count.ToString ( )}"
                                  ) ;

                if ( ! _context.Any ( ) )
                {
                    _writer.Flush ( ) ;
                    var str = Encoding.UTF8.GetString (
                                                       _memoryStream.GetBuffer ( )
                                                     , 0
                                                     , ( int ) _memoryStream.Length
                                                      ) ;
                    _output.WriteLine ( str ) ;
                }

                ;
                switch ( _context.Peek ( ) )
                {
                    case WriteContext.PropertyName :
                        _writer.WritePropertyName ( "Member:" + xamlMember.Name ) ;
                        _context.Pop ( ) ;
                        _context.Push ( WriteContext.PropertyValue ) ;
                        break ;
                    case WriteContext.PropertyValue :
                    case WriteContext.InArray :
                        throw new InvalidOperationException ( ) ;
                    default : throw new ArgumentOutOfRangeException ( ) ;
                }
            }

            public override void WriteEndMember ( )
            {
                _output.WriteLine (
                                   $"{nameof ( WriteEndMember )}: {_context.Count.ToString ( )}"
                                  ) ;

                if ( ! _context.Any ( ) )
                {
                    _writer.Flush ( ) ;
                    var str = Encoding.UTF8.GetString (
                                                       _memoryStream.GetBuffer ( )
                                                     , 0
                                                     , ( int ) _memoryStream.Length
                                                      ) ;
                    _output.WriteLine ( str ) ;
                }

                ;
                switch ( _context.Peek ( ) )
                {
                    case WriteContext.PropertyValue :

                    case WriteContext.InArray :
                        throw new InvalidOperationException ( _context.Peek ( ).ToString ( ) ) ;
                    case WriteContext.PropertyName :
                        _writer.WriteEndObject ( ) ;
                        _context.Pop ( ) ;
                        break ;
                }
            }

            public override void WriteValue ( object value )
            {
                _output.WriteLine ( _context.Count.ToString ( ) ) ;
                switch ( _context.Peek ( ) )
                {
                    case WriteContext.InArray :
                    case WriteContext.PropertyName :
                        throw new InvalidOperationException ( ) ;
                    case WriteContext.PropertyValue :
                        _writer.WriteStringValue ( value.ToString ( ) ) ;
                        _context.Pop ( ) ;
                        _context.Push ( WriteContext.PropertyName ) ;
                        break ;

                    default : throw new ArgumentOutOfRangeException ( ) ;
                }
            }

            public override void WriteNamespace ( NamespaceDeclaration namespaceDeclaration )
            {
                _output.WriteLine (
                                   $"{nameof ( WriteNamespace )}: {_context.Count.ToString ( )}"
                                  ) ;


                switch ( _context.Peek ( ) )
                {
                    case WriteContext.PropertyName :
                        _writer.WriteString (
                                             "ns:" + namespaceDeclaration.Prefix
                                           , namespaceDeclaration.Namespace
                                            ) ;
                        _context.Pop ( ) ;
                        _context.Push ( WriteContext.PropertyName ) ;
                        break ;
                    case WriteContext.PropertyValue :
                    case WriteContext.InArray :
                        throw new InvalidOperationException ( ) ;
                        break ;
                }
            }

            public override XamlSchemaContext SchemaContext { get { return _schemaContext ; } }
            #endregion
        }

        #region Overrides of JsonConverterFactory
        public override System.Text.Json.Serialization.JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new FrameworkElementJsonConverter ( typeToConvert , Output ) ;
        }

        private class FrameworkElementJsonConverter : JsonConverter < FrameworkElement >
        {
            private readonly ITestOutputHelper _output ;

            public FrameworkElementJsonConverter ( Type typeToConvert , ITestOutputHelper output )
            {
                _output = output ;
            }

            #region Overrides of JsonConverter<FrameworkElement>
            public override FrameworkElement Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , FrameworkElement      value
              , JsonSerializerOptions options
            )
            {
                // writer.WriteStartObject();
                // writer.WriteString ( "FrameworkElement", value.GetType().AssemblyQualifiedName ) ;
                var xamlSchemaContext = new XamlSchemaContext ( ) ;

                using ( var r = new XamlObjectReader (
                                                      value
                                                    , xamlSchemaContext
                                                    , new XamlObjectReaderSettings ( )
                                                     ) )
                {
                    var memoryStream = new MemoryStream ( ) ;
                    var x = new Utf8JsonWriter ( memoryStream ) ;
                    x.WriteStartObject ( ) ;
                    var xamlWriter = new JsonWriter (
                                                     x
                                                   , xamlSchemaContext
                                                   , r
                                                   , WriteContext.PropertyName
                                                   , memoryStream
                                                   , _output
                                                    ) ;
                    XamlServices.Transform ( r , xamlWriter ) ;
                    writer.WriteEndObject ( ) ;
                }
            }
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProjInterfaceAppConverter : JsonConverter < Application>
    {
        #region Overrides of JsonConverter<ProjInterfaceApp>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override Application  Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (
            Utf8JsonWriter        writer
          , Application      value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( ( string ) "ApplicationType" , ( string ) value.GetType ( ).FullName ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}