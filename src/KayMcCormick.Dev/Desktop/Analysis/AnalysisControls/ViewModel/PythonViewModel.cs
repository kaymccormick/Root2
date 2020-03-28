using System ;
using System.Collections.Generic ;
using System.Collections.Specialized ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Text.Json.Serialization ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Documents ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.Scripting ;
using Autofac ;
using IronPython.Hosting ;
using JetBrains.Annotations ;
using Microsoft.Scripting.Hosting ;

namespace AnalysisControls.ViewModel
{
    public sealed class PythonViewModel : DependencyObject
      , IViewModel
      , INotifyPropertyChanged
      , ISupportInitialize
    {
        [JsonIgnore]
        public ICollectionView linesCollectionView
        {
            get { return CollectionViewSource.GetDefaultView ( Lines ) ; }
        }

        [JsonIgnore]
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
                Debug.WriteLine ($"populating variale {pythonVariable.VariableName}"  );
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

        [JsonIgnore]
        public FlowDocument FlowDOcument { get ; set ; } = new FlowDocument();

        public static readonly DependencyProperty InputLineProperty =
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
            Debug.WriteLine (
                             $"input line changed. old = {e.OldValue}, new = {e.NewValue}");

        }

        public static readonly DependencyProperty LinesProperty =
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
        public  static readonly DependencyProperty ResultsProperty =
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
            Debug.WriteLine ( "Lines changed" ) ;
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
            Debug.WriteLine($"In {nameof(OnLinesCOllectionChanged)}");
            if ( e.Action == NotifyCollectionChangedAction.Add )
            {
                var new1 = e.NewStartingIndex + e.NewItems.Count - 1 ;
                Debug.WriteLine ( $"Moving current to ${new1}" ) ;
                linesCollectionView.MoveCurrentTo (new1 ) ;
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
            Lines.Add ( "") ;
            linesCollectionView.MoveCurrentToLast ( ) ;
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
        
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        public void HistoryDown ( )
        {
            linesCollectionView.MoveCurrentToNext ( ) ;
        }

        [JsonIgnore]
        public StringObservableCollection Lines
        {
            get { return ( StringObservableCollection ) GetValue ( LinesProperty ) ; }
            
            set { SetValue ( LinesProperty , value ) ; }
        }

        [JsonIgnore]
        public string InputLine
        {
            get { return ( string ) GetValue ( InputLineProperty ) ; }
            set { SetValue ( InputLineProperty , value ) ; }
        }

        
        public void BeginInit ( ) { }

        [JsonIgnore]
        public DynamicObservableCollection Results
        {
            get { return ( DynamicObservableCollection ) GetValue ( ResultsProperty ) ; }
            
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

            Lines.Add ( "#test" ) ;
            // var bindingBase = new Binding ( )
                              // {
                                  // Source = linesCollectionView
                                // , Path   = new PropertyPath ( "CurrentItem" ),
                                  // Mode   = BindingMode.TwoWay
                              // } ;
            // Debug.WriteLine ( $"binding is {bindingBase}" ) ;
            // BindingOperations.SetBinding (
                                          // this
                                        // , InputLineProperty
                                        // , bindingBase
                                         // ) ;
            var il = GetValue ( InputLineProperty ) ;
            Debug.WriteLine ( $"value of input line is {il}" ) ;
            linesCollectionView.MoveCurrentToLast ( ) ;
            Debug.WriteLine ( linesCollectionView.CurrentItem ) ;
        }
        

        public void ExecutePythonScript ( string textEditorText )
        {
            var objectHandle = _py.ExecuteAndWrap ( textEditorText ) ;

        }
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