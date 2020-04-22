#region header
// Kay McCormick (mccor)
// 
// Analysis
// ProjTests
// MyXmlWriter.cs
// 
// 2020-04-07-12:56 AM
// 
// ---
#endregion
using System.Text.Json ;
using System.Xml ;

namespace ProjTests
{
    public sealed class MyXmlWriter : XmlWriter
    {
        private readonly Utf8JsonWriter _writer ;
        public MyXmlWriter ( Utf8JsonWriter writer ) { _writer = writer ; }
        #region Overrides of XmlWriter
        public override void WriteStartDocument ( ) { _writer.WriteStartObject ( ) ; }

        public override void WriteStartDocument ( bool standalone )
        {
            _writer.WriteStartObject ( ) ;
        }

        public override void WriteEndDocument ( ) { _writer.WriteEndObject ( ) ; }

        public override void WriteDocType (
            string name
          , string pubid
          , string sysid
          , string subset
        )
        {
        }

        public override void WriteStartElement ( string prefix , string localName , string ns )
        {
            _writer.WriteStartObject ( ) ;
            //writer.WriteStartObject ( localName ) ;
        }

        public override void WriteEndElement ( ) { _writer.WriteEndObject ( ) ; }

        public override void WriteFullEndElement ( ) { _writer.WriteEndObject ( ) ; }

        public override void WriteStartAttribute ( string prefix , string localName , string ns )
        {
            _writer.WriteStartObject ( localName ) ;
        }

        public override void WriteEndAttribute ( ) { }

        public override void WriteCData ( string text ) { }

        public override void WriteComment ( string text ) { }

        public override void WriteProcessingInstruction ( string name , string text ) { }

        public override void WriteEntityRef ( string name ) { }

        public override void WriteCharEntity ( char ch ) { }

        public override void WriteWhitespace ( string ws ) { }

        public override void WriteString ( string text ) { _writer.WriteStringValue ( text ) ; }

        public override void WriteSurrogateCharEntity ( char lowChar , char highChar ) { }

        public override void WriteChars ( char[] buffer , int index , int count ) { }

        public override void WriteRaw ( char[] buffer , int index , int count ) { }

        public override void WriteRaw ( string data ) { }

        public override void WriteBase64 ( byte[] buffer , int index , int count ) { }

        public override void Flush ( ) { }

        public override string LookupPrefix ( string ns ) { return null ; }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public override WriteState WriteState { get ; }
        #endregion
    }
}