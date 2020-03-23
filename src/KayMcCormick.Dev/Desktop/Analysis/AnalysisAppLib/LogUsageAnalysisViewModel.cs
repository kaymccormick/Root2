#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ILogUsageAnalysisViewModel.cs
// 
// 2020-03-17-4:24 PM
// 
// ---
#endregion
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Threading.Tasks ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib
{
    public sealed class LogUsageAnalysisViewModel : ILogUsageAnalysisViewModel , INotifyPropertyChanged
    {
        private LogInvocationCollection                   _logInvocations ;
        private PipelineResult                            _pipelineResult ;
        private ObservableCollection < LogEventInstance > _events ;

        #region Implementation of ILogUsageAnalysisViewModel
        public LogInvocationCollection LogInvocations
        {
            get { return _logInvocations ; }
            set
            {
                _logInvocations = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public PipelineResult PipelineResult
        {
            get { return _pipelineResult ; }
            set
            {
                _pipelineResult = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public ObservableCollection < LogEventInstance > Events
        {
            get { return _events ; }
            set
            {
                _events = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public async Task AnalyzeCommand ( object viewCurrentItem ) { }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}