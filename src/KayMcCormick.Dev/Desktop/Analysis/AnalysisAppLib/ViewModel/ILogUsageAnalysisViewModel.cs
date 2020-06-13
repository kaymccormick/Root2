#if FINDLOGUSAGES
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Threading.Tasks ;
using KayMcCormick.Dev.Logging ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogUsageAnalysisViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        LogInvocationCollection LogInvocations { get ; }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        PipelineResult PipelineResult { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        ObservableCollection < LogEventInstance > Events { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewCurrentItem"></param>
        /// <returns></returns>
        Task AnalyzeCommand ( object viewCurrentItem ) ;
    }
}
#endif