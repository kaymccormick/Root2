using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Threading.Tasks ;
using KayMcCormick.Dev.Logging ;

namespace AnalysisAppLib.XmlDoc.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogUsageAnalysisViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        LogInvocationCollection LogInvocations { get ; }

        /// <summary>
        /// 
        /// </summary>
        PipelineResult PipelineResult { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        ObservableCollection < LogEventInstance > Events { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewCurrentItem"></param>
        /// <returns></returns>
        Task AnalyzeCommand ( object viewCurrentItem ) ;
    }
}