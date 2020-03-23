using System.Threading.Tasks ;

namespace AnalysisAppLib
{
    public interface IAnalyzeCommand
    {
        Task AnalyzeCommandAsync ( IProjectBrowserNode projectNode ) ;
    }
}