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
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Input ;
using System.Xml.Serialization ;
using AnalysisAppLib.ViewModel ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Lib.Wpf.View ;

namespace KayMcCormick.Lib.Wpf.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class EventLogViewModel : IViewModel , ISupportInitialize
    {
        /// <summary>
        /// 
        /// </summary>
        public ICollectionView EventLogCollectionView
        {
            get { return CollectionViewSource.GetDefaultView ( EventLogEntryCollection ) ; }
        }

        private readonly PaneService   _panelService ;
        private readonly LayoutService _layoutService ;
        private          EventLogView  _view ;
        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panelService"></param>
        /// <param name="layoutService"></param>
        public EventLogViewModel ( PaneService panelService , LayoutService layoutService )
        {
            _panelService  = panelService ;
            _layoutService = layoutService ;
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public void EndInit ( )
        {
            EventLog eventLog = new EventLog ( "Application" ) ;
            var eventLogEntries = eventLog.Entries.OfType < EventLogEntry > ( )
                                          .Where (
                                                  ( entry , i )
                                                      => entry.TimeWritten
                                                         >= DateTime.Parse (
                                                                            "March 27, 2020 8:15:00 PM"
                                                                           )
                                                 )
                                          .Where (
                                                  entry => entry.Source        == "Kay McCormick"
                                                           && entry.InstanceId == 8
                                                 )
                                          .OrderByDescending ( entry => entry.TimeWritten ) ;
            foreach ( var eventLogEntry in eventLogEntries )
            {
                Debug.WriteLine ( eventLogEntry.TimeWritten ) ;
                var parsedEventLogEntry = new ParsedEventLogEntry ( eventLogEntry ) ;
                if ( parsedEventLogEntry.Exception1 != null )
                {
                    EventLogEntryCollection.Add ( parsedEventLogEntry ) ;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < ParsedEventLogEntry > EventLogEntryCollection { get ; } =
            new ObservableCollection < ParsedEventLogEntry > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public EventLogView View
        {
            get => _view ;
            set
            {
                _view = value ;
                _view.AddHandler (
                                  CommandManager.PreviewCanExecuteEvent
                                , new CanExecuteRoutedEventHandler ( Target )
                                 ) ;
                _view.AddHandler (
                                  CommandManager.PreviewExecutedEvent
                                , new ExecutedRoutedEventHandler ( PreviewExecuted )
                                 ) ;
                foreach ( var routedEvent in EventManager.GetRoutedEvents ( ) )
                {
                    Debug.WriteLine ( routedEvent.OwnerType.FullName ) ;
                    Debug.WriteLine ( routedEvent.RoutingStrategy ) ;
                    Debug.WriteLine ( routedEvent.Name ) ;
                }
            }
        }

        private void Target ( object sender , CanExecuteRoutedEventArgs e )
        {
            return ;
            if ( e.Command is RoutedUICommand rc )
            {
                Debug.WriteLine($"PreviewCanExecute - {rc.Text} - {rc.Name}");
            } else if ( e.Command is RoutedCommand rc2 )
            {
                Debug.WriteLine ($"PreviewCanExecute - {rc2.Name}"  );
            }
            else
            {
                Debug.WriteLine ( $"PreviewCanExecute - {e.Command}" ) ;
            }
            
        }

        private void PreviewExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            if ( e.Command == ApplicationCommands.Open )
            {
                var paneWrapper = _panelService.GetPane ( ) ;
                var parsedEventLogEntry = ( ParsedEventLogEntry )
                    ( ( ListView ) e.OriginalSource )
                   .SelectedItem ;
                var exception1 = parsedEventLogEntry.Exception1 ;
                ExceptionUserControl uc = new ExceptionUserControl
                                          {
                                              DataContext =
                                                  new ExceptionDataInfo
                                                  {
                                                      Exception =
                                                          exception1
                                                          , ParsedExceptions = parsedEventLogEntry.Parsed
                                                  }
                                          } ;

                // paneWrapper.AddChild(uc);
                // _layoutService.AddToLayout ( paneWrapper ) ;
                Window w = new Window { Content = uc } ;
                w.ShowDialog ( ) ;
                Debug.WriteLine(e.OriginalSource);
                Debug.WriteLine ( e.Source ) ;
                Debug.WriteLine(sender);
                Debug.WriteLine(EventLogCollectionView.CurrentPosition);
            }
            if (e.Command is RoutedUICommand rc)
            {
                Debug.WriteLine($"PreviewExecuted - {rc.Text} - {rc.Name}");
            }
            else if (e.Command is RoutedCommand rc2)
            {
                Debug.WriteLine($"PreviewExecuted- {rc2.Name}");
            }
            else
            {
                Debug.WriteLine($"PreviewExecuted- {e.Command}");
            }


        }

    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ParsedEventLogEntry
    {
        private readonly EventLogEntry _logEntry ;

        /// <summary>
        /// 
        /// </summary>
        public DateTime TimeWritten
        {
            get { return _logEntry.TimeWritten ; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logEntry"></param>
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
                                Exception1 = ( Exception ) exception ;

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

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception1 { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public ParsedExceptions Parsed { get ; set ; }
    }
}