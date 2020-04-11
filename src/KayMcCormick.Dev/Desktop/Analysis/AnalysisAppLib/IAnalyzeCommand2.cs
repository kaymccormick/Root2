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
using AnalysisAppLib.XmlDoc.Project ;

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public interface IAnalyzeCommand2 < TOutput > : IAnalyzeCommand3
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAnalyzeCommand3
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectNode"></param>
        /// <returns></returns>
        Task AnalyzeCommandAsync ( IProjectBrowserNode projectNode ) ;
    }
}