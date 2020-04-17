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
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Globalization ;
using System.IO ;
using System.Linq ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Binary ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Input ;
using System.Xml.Serialization ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Lib.Wpf.View ;

namespace KayMcCormick.Lib.Wpf.ViewModel
{
    /// <summary>
    /// </summary>
    public sealed class EventLogViewModel : IViewModel , ISupportInitialize
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly LayoutService _layoutService ;

        private readonly PaneService  _panelService ;
        private          EventLogView _view ;

        /// <summary>
        /// </summary>
        /// <param name="panelService"></param>
        /// <param name="layoutService"></param>
        public EventLogViewModel ( PaneService panelService , LayoutService layoutService )
        {
            _panelService  = panelService ;
            _layoutService = layoutService ;
        }

        /// <summary>
        /// </summary>
        public ICollectionView EventLogCollectionView
        {
            get { return CollectionViewSource.GetDefaultView ( EventLogEntryCollection ) ; }
        }

        /// <summary>
        /// </summary>
        public ObservableCollection < ParsedEventLogEntry > EventLogEntryCollection { get ; } =
            new ObservableCollection < ParsedEventLogEntry > ( ) ;

        /// <summary>
        /// </summary>
        public EventLogView View
        {
            // ReSharper disable once UnusedMember.Global
            get { return _view ; }
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
                    DebugUtils.WriteLine ( routedEvent.OwnerType.FullName ) ;
                    DebugUtils.WriteLine ( routedEvent.RoutingStrategy.ToString() ) ;
                    DebugUtils.WriteLine ( routedEvent.Name ) ;
                }
            }
        }

        /// <summary>
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// </summary>
        public void EndInit ( )
        {
            var eventLog = new EventLog ( "Application" ) ;
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
                DebugUtils.WriteLine ( eventLogEntry.TimeWritten.ToString( CultureInfo.CurrentCulture ) ) ;
                var parsedEventLogEntry = new ParsedEventLogEntry ( eventLogEntry ) ;
                if ( parsedEventLogEntry.Exception1 != null )
                {
                    EventLogEntryCollection.Add ( parsedEventLogEntry ) ;
                }
            }
        }

        #region Implementation of ISerializable
        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        private void Target ( object sender , CanExecuteRoutedEventArgs e )
        {
            return ;
            if ( e.Command is RoutedUICommand rc )
            {
                DebugUtils.WriteLine ( $"PreviewCanExecute - {rc.Text} - {rc.Name}" ) ;
            }
            else if ( e.Command is RoutedCommand rc2 )
            {
                DebugUtils.WriteLine ( $"PreviewCanExecute - {rc2.Name}" ) ;
            }
            else
            {
                DebugUtils.WriteLine ( $"PreviewCanExecute - {e.Command}" ) ;
            }
        }

        private void PreviewExecuted ( object sender , [ NotNull ] ExecutedRoutedEventArgs e )
        {
            if ( e.Command == ApplicationCommands.Open )
            {
                // ReSharper disable once UnusedVariable
                var paneWrapper = _panelService.GetPane ( ) ;
                var parsedEventLogEntry =
                    ( ParsedEventLogEntry ) ( ( ListView ) e.OriginalSource ).SelectedItem ;
                var exception1 = parsedEventLogEntry.Exception1 ;
                var uc = new ExceptionUserControl
                         {
                             DataContext = new ExceptionDataInfo
                                           {
                                               Exception        = exception1
                                             , ParsedExceptions = parsedEventLogEntry.Parsed
                                           }
                         } ;

                // paneWrapper.AddChild(uc);
                // _layoutService.AddToLayout ( paneWrapper ) ;
                var w = new Window { Content = uc } ;
                w.ShowDialog ( ) ;
                DebugUtils.WriteLine ( e.OriginalSource.ToString() ) ;
                DebugUtils.WriteLine ( e.Source.ToString() ) ;
                DebugUtils.WriteLine ( sender.ToString() ) ;
                DebugUtils.WriteLine ( EventLogCollectionView.CurrentPosition.ToString() ) ;
            }

            if ( e.Command is RoutedUICommand rc )
            {
                DebugUtils.WriteLine ( $"PreviewExecuted - {rc.Text} - {rc.Name}" ) ;
            }
            else if ( e.Command is RoutedCommand rc2 )
            {
                DebugUtils.WriteLine ( $"PreviewExecuted- {rc2.Name}" ) ;
            }
            else
            {
                DebugUtils.WriteLine ( $"PreviewExecuted- {e.Command}" ) ;
            }
        }
    }

    /// <summary>
    /// </summary>
    public sealed class ParsedEventLogEntry
    {
        private readonly EventLogEntry _logEntry ;

        /// <summary>
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
                    var deSerializer = new XmlSerializer ( typeof ( ParsedExceptions ) ) ;
                    var sr = new StringReader ( item5 ) ;
                    Parsed = ( ParsedExceptions ) deSerializer.Deserialize ( sr ) ;
                    var item4 = _logEntry.ReplacementStrings[ 4 ] ;

                    try
                    {
                        if ( int.TryParse ( _logEntry.ReplacementStrings[ 3 ] , out var nbytes ) )
                        {
                            if ( item4.Length == nbytes * 2 )
                            {
                                var bytes = new byte[ item4.Length / 2 ] ;
                                for ( var i = 0 ; i < item4.Length ; i += 2 )
                                {
                                    bytes[ i / 2 ] =
                                        Convert.ToByte ( item4.Substring ( i , 2 ) , 16 ) ;
                                }

                                var ms = new MemoryStream ( bytes ) ;
                                IFormatter f = new BinaryFormatter ( ) ;
                                var exception = f.Deserialize ( ms ) ;
                                Exception1 = ( Exception ) exception ;
                            }
                        }
                    }
                    catch ( Exception ex )
                    {
                        DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public DateTime TimeWritten { get { return _logEntry.TimeWritten ; } }

        /// <summary>
        /// </summary>
        public Exception Exception1 { get ; }

        /// <summary>
        /// </summary>
        public ParsedExceptions Parsed { get ; }
    }
}