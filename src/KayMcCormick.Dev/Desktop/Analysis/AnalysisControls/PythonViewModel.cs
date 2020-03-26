using AnalysisAppLib.ViewModel ;
using IronPython.Hosting ;
using Microsoft.Scripting.Hosting ;
using System ;
using System.Collections.Generic ;
using System.Collections.Specialized ;
using System.ComponentModel ;
using System.IO ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Documents ;
using Autofac ;
using JetBrains.Annotations ;
using Vanara.Extensions.Reflection ;

namespace AnalysisControls
{
    public sealed class PythonViewModel : DependencyObject
      , IViewModel
      , INotifyPropertyChanged
      , ISupportInitialize
    {
        public ICollectionView linesCollectionView
        {
            get { return CollectionViewSource.GetDefaultView ( Lines ) ; }
        }

        public ILifetimeScope Scope { get ; }

        private readonly ScriptEngine _py ;
        private readonly ScriptScope  _scope ;


        public PythonViewModel ( ILifetimeScope scope , [ NotNull ] IEnumerable < IPythonVariable > vars )
        {
            Scope = scope ;
            _py   = Python.CreateEngine ( ) ;

            _scope = _py.CreateScope ( ) ;

            _scope.SetVariable ( "viewModel" , this ) ;
            _scope.SetVariable ( "scope" ,     scope ) ;

            foreach ( var pythonVariable in vars )
            {
                if ( string.IsNullOrEmpty ( pythonVariable.VariableName )
                     || _scope.TryGetVariable ( pythonVariable.VariableName , out _ ) )
                {
                    continue ;
                }

                _scope.SetVariable (
                                    pythonVariable.VariableName
                                  , pythonVariable.GetVariableValue ( )
                                   ) ;
            }

            var ms = new MemoryStream ( ) ;

            var outputWr = new EventRaisingStreamWriter ( ms ) ;
            outputWr.StringWritten += sWr_StringWritten ;
            _py.Runtime.IO.SetOutput ( ms , outputWr ) ;
        }

        public FlowDocument FlowDOcument { get ; set ; } = new FlowDocument();

        private static readonly DependencyProperty InputLineProperty =
            DependencyProperty.Register (
                                         nameof ( InputLine )
                                       , typeof ( string )
                                       , typeof ( PythonViewModel )
                                       , new FrameworkPropertyMetadata (
                                                                        ""
                                                                      , FrameworkPropertyMetadataOptions
                                                                           .None
                                                                      , OnInputLineChanged
                                                                       )
                                        ) ;

        private static void OnInputLineChanged (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
        }

        private static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register (
                                         "Lines"
                                       , typeof ( StringObservableCollection )
                                       , typeof ( PythonViewModel )
                                       , new FrameworkPropertyMetadata (
                                                                        null
                                                                      , FrameworkPropertyMetadataOptions
                                                                           .None
                                                                      , OnLinesChanged
                                                                       )
                                        ) ;
        private static readonly DependencyProperty ResultsProperty =
            DependencyProperty.Register(
                                        "Results"
                                      , typeof(DynamicObservableCollection)
                                      , typeof(PythonViewModel)
                                      , new FrameworkPropertyMetadata(
                                                                      null
                                                                    , FrameworkPropertyMetadataOptions
                                                                         .None
                                                                    , OnResultsChanged
                                                                     )
                                       );

        private static void OnResultsChanged ( DependencyObject d , DependencyPropertyChangedEventArgs e ) { }

        private static void OnLinesChanged (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
            var x = ( PythonViewModel ) d ;
            var old = ( StringObservableCollection ) e.OldValue ;
            if ( old != null )
            {
                old.CollectionChanged -= x.OnLinesCOllectionChanged ;
            }

            var @new = ( StringObservableCollection ) e.NewValue ;
            if ( @new != null ) { @new.CollectionChanged += x.OnLinesCOllectionChanged ; }
        }

        private void OnLinesCOllectionChanged (
            object                                       sender
          , [ NotNull ] NotifyCollectionChangedEventArgs e
        )
        {
            if ( e.Action == NotifyCollectionChangedAction.Add )
            {
                linesCollectionView.MoveCurrentTo ( e.NewStartingIndex + e.NewItems.Count - 1 ) ;
            }
        }


        private void sWr_StringWritten ( object sender , [ NotNull ] MyEvtArgs < string > e )
        {
            FlowWrite ( e.Value ) ;
        }

        private void FlowWrite ( string eValue )
        {
            FlowDOcument.Blocks.Add ( new Paragraph ( new Run ( eValue ) ) ) ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        public void TakeLine ( string text )
        {
            Lines.Add ( text ) ;
            //linesCollectionView.MoveCurrentToLast ( ) ;
            FlowDOcument.Blocks.Add ( new Paragraph ( new Run ( text ) ) ) ;
            string strRep = null ;
            dynamic result = null ;
            try
            {
                result = _py.Execute ( text , _scope ) ;
                Results.Add(result);
            }
            catch ( Exception ex )
            {
                strRep = ex.Message ;
            }

            if ( result != null )
            {
                try
                {
                    strRep = result?.__repr__ ( result ) ?? "None" ;

                }
                catch ( Exception ex )

                {
                    try
                    {
                        strRep = result.ToString ( ) ;
                    }
                    catch ( Exception ex2)
                    {
                        strRep = result.__name__ ;
                    }
                }
            }
            if ( strRep != null )
            {
                FlowDOcument.Blocks.Add ( new Paragraph ( new Run ( strRep ) ) ) ;
            }
        }

        public void HistoryUp ( )
        {
            linesCollectionView.MoveCurrentToPrevious ( ) ;
            //TextInput = history[ historyPos.Value ] ;
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        // ReSharper disable once UnusedMember.Local
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        public void HistoryDown ( )
        {
            linesCollectionView.MoveCurrentToNext ( ) ;
        }

        public StringObservableCollection Lines
        {
            get { return ( StringObservableCollection ) GetValue ( LinesProperty ) ; }
            // ReSharper disable once UnusedMember.Global
            set { SetValue ( LinesProperty , value ) ; }
        }

        public string InputLine
        {
            get { return ( string ) GetValue ( InputLineProperty ) ; }
            set { SetValue ( InputLineProperty , value ) ; }
        }

        #region Implementation of ISupportInitialize
        public void BeginInit ( ) { }

        public DynamicObservableCollection Results
        {
            get { return ( DynamicObservableCollection ) GetValue ( ResultsProperty ) ; }
            // ReSharper disable once UnusedMember.Global
            set { SetValue ( ResultsProperty , value ) ; }
        }

        public void EndInit ( )
        {
            if ( Lines == null )
            {
                SetCurrentValue ( LinesProperty , new StringObservableCollection ( ) ) ;
            }

            if ( Results == null )
            {
                SetCurrentValue(ResultsProperty, new DynamicObservableCollection());
            }

            BindingOperations.SetBinding (
                                          this
                                        , InputLineProperty
                                        , new Binding ( )
                                          {
                                              Source = linesCollectionView
                                            , Path   = new PropertyPath ( "/" )
                                          }
                                         ) ;
            linesCollectionView.MoveCurrentToLast ( ) ;
        }
        #endregion
    }

    public sealed class PythonVariable : IPythonVariable
    {
        #region Implementation of IPythonVariable
        public string VariableName { get ; set ; }

        public Func < dynamic > ValueLambda { get ; set ; }

        [ CanBeNull ] public dynamic GetVariableValue ( ) { return ValueLambda?.Invoke ( ) ; }
        #endregion
    }
}