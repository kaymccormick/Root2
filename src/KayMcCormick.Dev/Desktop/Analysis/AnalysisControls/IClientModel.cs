using System.Windows.Controls.Ribbon;
using AnalysisControls.RibbonModel;
using KayMcCormick.Dev.Logging;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClientModel
    {
        /// <summary>
        /// 
        /// </summary>
        PrimaryRibbonModel PrimaryRibbon { get; set; }

        LogEventInstanceObservableCollection LogEntries { get; set; }
        MyRibbon MyRibbon { get; set; }
        Ribbon Ribbon { get; set; }
    }
}