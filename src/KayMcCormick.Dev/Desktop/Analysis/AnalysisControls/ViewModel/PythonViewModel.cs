
using System ;
using System.Collections.Generic ;
using System.Collections.Specialized ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Text.Json.Serialization ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Documents ;
using AnalysisControls.Scripting ;
using Autofac ;
using IronPython.Hosting ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.Scripting.Hosting ;

namespace AnalysisControls.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PythonViewModel : DependencyObject
      , IViewModel
      , INotifyPropertyChanged
      , ISupportInitialize
    {
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ResultsProperty =
            DependencyProperty.Register (
                                         "Results"
                                       , typeof ( DynamicObservableCollection )
                                       , typeof ( PythonViewModel )
                                       , new FrameworkPropertyMetadata (
                                                                        null
                                                                      , FrameworkPropertyMetadataOptions
                                                                           .None
                                                                      , OnResultsChanged
                                                                       )
                                        ) ;

        private ScriptEngine _py ;
        private ScriptScope  _pyScope ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="vars"></param>
        public PythonViewModel (
            ILifetimeScope                              scope
          , [ NotNull ] IEnumerable < IPythonVariable > vars
        )
        {
            Scope = scope ;
            PythonInit ( scope , vars ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        [ JsonIgnore ]
        public ICollectionView LinesCollectionView
        {
            get { return CollectionViewSource.GetDefaultView ( Lines ) ; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ JsonIgnore ]
        public ILifetimeScope Scope { get ; }

        /// <summary>
        /// 
        /// </summary>
        [ JsonIgnore ]
        public FlowDocument FlowDocument { get ; set ; } = new FlowDocument ( ) ;

        /// <summary>
        /// 
        /// </summary>
        [ JsonIgnore ]
        public StringObservableCollection Lines
        {
            get { return ( StringObservableCollection ) GetValue ( LinesProperty ) ; }
            set { SetValue ( LinesProperty , value ) ; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ JsonIgnore ]
        public string InputLine
        {
            get { return ( string ) GetValue ( InputLineProperty ) ; }
            set { SetValue ( InputLineProperty , value ) ; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ JsonIgnore ]
        public DynamicObservableCollection Results
        {
            get { return ( DynamicObservableCollection ) GetValue ( ResultsProperty ) ; }
            set { SetValue ( ResultsProperty , value ) ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;


        /// <summary>
        /// 
        /// </summary>
        public void BeginInit ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public void EndInit ( )
        {
            if ( Lines == null )
            {
                SetCurrentValue ( LinesProperty , new StringObservableCollection ( ) ) ;
            }

            if ( Results == null )
            {
                SetCurrentValue ( ResultsProperty , new DynamicObservableCollection ( ) ) ;
            }

            Lines.Add ( "#test" ) ;
            // var bindingBase = new Binding ( )
            // {
            // Source = linesCollectionView
            // , Path   = new PropertyPath ( "CurrentItem" ),
            // Mode   = BindingMode.TwoWay
            // } ;
            // DebugUtils.WriteLine ( $"binding is {bindingBase}" ) ;
            // BindingOperations.SetBinding (
            // this
            // , InputLineProperty
            // , bindingBase
            // ) ;
            var il = GetValue ( InputLineProperty ) ;
            DebugUtils.WriteLine ( $"value of input line is {il}" ) ;
            LinesCollectionView.MoveCurrentToLast ( ) ;
            DebugUtils.WriteLine ( LinesCollectionView.CurrentItem.ToString() ) ;

            //Task<int>.Run((state) => RunPython(state), CancellationToken.None));
        }

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        private void PythonInit ( ILifetimeScope scope , [ NotNull ] IEnumerable < IPythonVariable > vars )
        {
            _py = Python.CreateEngine ( ) ;

            _pyScope = _py.CreateScope ( ) ;
            _pyScope.SetVariable ( "viewModel" , this ) ;
            _pyScope.SetVariable ( "scope" ,     scope ) ;

            foreach ( var pythonVariable in vars.Where ( pythonVariable => ! string.IsNullOrEmpty ( pythonVariable.VariableName ) && ! _pyScope.TryGetVariable ( pythonVariable.VariableName , out _ ) ) )
            {
                DebugUtils.WriteLine ( $"populating variable {pythonVariable.VariableName}" ) ;
                _pyScope.SetVariable (
                                      pythonVariable.VariableName
                                    , pythonVariable.GetVariableValue ( )
                                     ) ;
            }

            var ms = new MemoryStream ( ) ;

            var outputWr = new EventRaisingStreamWriter ( ms ) ;
            outputWr.StringWritten += sWr_StringWritten ;
            _py.Runtime.IO.SetOutput ( ms , outputWr ) ;
        }

        private static void OnInputLineChanged (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
            DebugUtils.WriteLine ( $"input line changed. old = {e.OldValue}, new = {e.NewValue}" ) ;
        }

        private static void OnResultsChanged (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
        }

        private static void OnLinesChanged (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
            DebugUtils.WriteLine ( "Lines changed" ) ;
            var x = ( PythonViewModel ) d ;
            var old = ( StringObservableCollection ) e.OldValue ;
            if ( old != null )
            {
                old.CollectionChanged -= x.OnLinesCollectionChanged ;
            }

            var @new = ( StringObservableCollection ) e.NewValue ;
            if ( @new != null ) { @new.CollectionChanged += x.OnLinesCollectionChanged ; }
        }

        private void OnLinesCollectionChanged (
            object                                       sender
          , [ NotNull ] NotifyCollectionChangedEventArgs e
        )
        {
            DebugUtils.WriteLine ( $"In {nameof ( OnLinesCollectionChanged )}" ) ;
            if ( e.Action != NotifyCollectionChangedAction.Add )
            {
                return ;
            }

            var new1 = e.NewStartingIndex + e.NewItems.Count - 1 ;
            DebugUtils.WriteLine ( $"Moving current to ${new1}" ) ;
            LinesCollectionView.MoveCurrentTo ( new1 ) ;
        }


        private void sWr_StringWritten ( object sender , [ NotNull ] MyEvtArgs < string > e )
        {
            FlowWrite ( e.Value ) ;
        }

        private void FlowWrite ( string eValue )
        {
            FlowDocument.Blocks.Add ( new Paragraph ( new Run ( eValue ) ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void TakeLine ( string text )
        {
            Lines.Add ( "" ) ;
            LinesCollectionView.MoveCurrentToLast ( ) ;
            FlowDocument.Blocks.Add ( new Paragraph ( new Run ( text ) ) ) ;
            string strRep = null ;
            dynamic result = null ;
            try
            {
                result = _py.Execute ( text , _pyScope ) ;
                Results.Add ( result ) ;
            }
            catch ( Exception ex )
            {
                strRep = ex.Message ;
            }

            if ( result != null )
            {
                try
                {
                    strRep = result.__repr__ ( result ) ?? "None" ;
                }
                catch ( Exception )

                {
                    try
                    {
                        strRep = result.ToString ( ) ;
                    }
                    catch ( Exception )
                    {
                        strRep = result.__name__ ;
                    }
                }
            }

            if ( strRep != null )
            {
                FlowDocument.Blocks.Add ( new Paragraph ( new Run ( strRep ) ) ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void HistoryUp ( )
        {
            LinesCollectionView.MoveCurrentToPrevious ( ) ;
            //TextInput = history[ historyPos.Value ] ;
        }

        [ NotifyPropertyChangedInvocator ]
        // ReSharper disable once UnusedMember.Local
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public void HistoryDown ( ) { LinesCollectionView.MoveCurrentToNext ( ) ; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="textEditorText"></param>
        public void ExecutePythonScript ( string textEditorText )
        {
            // ReSharper disable once UnusedVariable
            var objectHandle = _py.ExecuteAndWrap ( textEditorText ) ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PythonVariable : IPythonVariable
    {
        #region Implementation of IPythonVariable
        /// <summary>
        /// 
        /// </summary>
        public string VariableName { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public Func < dynamic > ValueLambda { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ CanBeNull ] public dynamic GetVariableValue ( ) { return ValueLambda?.Invoke ( ) ; }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyEvtArgs<T> : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public T Value
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public MyEvtArgs(T value) => this.Value = value;
    }

    /// <summary>
    /// 
    /// </summary>
    public class NullStream : Stream
    {

        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override long Length
        {
            get { return 0; }
        }


        /// <summary>
        /// 
        /// </summary>
        public override long Position
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < buffer.Length; i++) buffer[i] = 0;
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class EventRaisingStreamWriter : StreamWriter
    {
        public event EventHandler<MyEvtArgs<string>> StringWritten;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public EventRaisingStreamWriter(Stream s) : base(s) { }

        private void LaunchEvent(string txtWritten)
        {
            StringWritten?.Invoke(this, new MyEvtArgs<string>(txtWritten));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void Write(string value)
        {
            LaunchEvent(value);
        }
    }

}
