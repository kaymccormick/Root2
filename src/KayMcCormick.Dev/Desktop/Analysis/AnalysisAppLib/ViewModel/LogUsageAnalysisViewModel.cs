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
using JetBrains.Annotations ;
using KayMcCormick.Dev.Logging ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LogUsageAnalysisViewModel : ILogUsageAnalysisViewModel
      , INotifyPropertyChanged
    {
        private ObservableCollection < LogEventInstance > _events ;
        private LogInvocationCollection                   _logInvocations ;
        private PipelineResult                            _pipelineResult ;

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ILogUsageAnalysisViewModel
        /// <summary>
        /// 
        /// </summary>
        public LogInvocationCollection LogInvocations
        {
            get { return _logInvocations ; }
            set
            {
                _logInvocations = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PipelineResult PipelineResult
        {
            get { return _pipelineResult ; }
            set
            {
                _pipelineResult = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < LogEventInstance > Events
        {
            get { return _events ; }
            set
            {
                _events = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewCurrentItem"></param>
        /// <returns></returns>
        public async Task AnalyzeCommand ( object viewCurrentItem ) { }
        #endregion
    }
}