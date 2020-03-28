#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// EventLogViewModel.cs
// 
// 2020-03-27-12:31 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Runtime.Remoting.Metadata.W3cXsd2001 ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Xml.Serialization ;
using AnalysisAppLib.ViewModel ;
using KayMcCormick.Dev.Application ;

namespace KayMcCormick.Lib.Wpf.ViewModel
{
    public class EventLogViewModel : IViewModel, ISupportInitialize
    {
        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
        #region Implementation of ISupportInitialize
        public void BeginInit ( ) { }

        public void EndInit ( )
        {
            EventLog eventLog = new EventLog ( "Application" ) ;
            var eventLogEntries = eventLog.Entries.OfType < EventLogEntry > ( )
                                          .Where(( entry , i ) => entry.TimeWritten >= DateTime.Parse("March 27, 2020 8:15:00 PM"))
                                          .Where ( entry => entry.Source == "Kay McCormick" && entry.InstanceId == 8 ).OrderByDescending(entry => entry.TimeWritten) ;
            foreach ( var eventLogEntry in eventLogEntries )
            {
                Debug.WriteLine (eventLogEntry.TimeWritten  );
                var parsedEventLogEntry = new ParsedEventLogEntry(eventLogEntry) ;
                if ( parsedEventLogEntry.Exception1 != null )
                {
                    EventLogEntryCollection.Add ( parsedEventLogEntry ) ;
                }
            }
        }

        public ObservableCollection <ParsedEventLogEntry> EventLogEntryCollection { get ; } = new ObservableCollection < ParsedEventLogEntry > ();
        
        #endregion
    }

    public class ParsedEventLogEntry
    {
        private EventLogEntry _logEntry ;

        public DateTime TimeWritten => _logEntry.TimeWritten ;
        public ParsedEventLogEntry ( EventLogEntry logEntry )
        {
            _logEntry = logEntry ;
            if ( _logEntry.InstanceId == 8 )
            {
                if ( _logEntry.ReplacementStrings.Length >= 6 )
                {
                    var item5 = _logEntry.ReplacementStrings[ 5 ] ;
                    XmlSerializer deSerializer = new XmlSerializer ( typeof ( ParsedExceptions ) ) ;
                    StringReader sr = new StringReader ( item5 ) ;
                    Parsed = ( ParsedExceptions ) deSerializer.Deserialize ( sr ) ;
                    var item4 = _logEntry.ReplacementStrings[ 4 ] ;

                    try
                    {
                        if ( int.TryParse ( _logEntry.ReplacementStrings[ 3 ] , out var nbytes ) )
                        {
                            if ( item4.Length == nbytes * 2 )
                            {
                                byte[] bytes = new byte[ item4.Length / 2 ] ;
                                for ( int i = 0 ; i < item4.Length ; i += 2 )
                                {
                                    bytes[ i / 2 ] =
                                        Convert.ToByte ( item4.Substring ( i , 2 ) , 16 ) ;
                                }

                                MemoryStream ms = new MemoryStream ( bytes ) ;
                                IFormatter f = new BinaryFormatter ( ) ;
                                var exception = f.Deserialize ( ms ) ;
                                Exception1 = exception ;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }

                }
            }

        }

        public object Exception1 { get ; set ; }

        public ParsedExceptions Parsed { get ; set ; }
    }
}