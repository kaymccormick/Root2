using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev.Logging;
using RibbonLib.Model;

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