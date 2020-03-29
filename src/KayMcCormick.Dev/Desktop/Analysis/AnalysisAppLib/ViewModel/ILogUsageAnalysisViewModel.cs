using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Threading.Tasks ;
using KayMcCormick.Dev.Logging ;

namespace AnalysisAppLib.ViewModel
{
    public interface ILogUsageAnalysisViewModel : INotifyPropertyChanged
    {
        LogInvocationCollection LogInvocations { get ; }

        PipelineResult PipelineResult { get ; set ; }

        ObservableCollection < LogEventInstance > Events { get ; }

        Task AnalyzeCommand ( object viewCurrentItem ) ;
    }
}