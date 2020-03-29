#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// IAnalyzeCommand2.cs
// 
// 2020-03-25-2:09 PM
// 
// ---
#endregion
using System.Threading.Tasks ;

namespace AnalysisAppLib
{
    public interface IAnalyzeCommand2 < TOutput > : IAnalyzeCommand3
    {
    }

    public interface IAnalyzeCommand3
    {
        Task AnalyzeCommandAsync ( IProjectBrowserNode projectNode ) ;
    }
}