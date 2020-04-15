#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// IDocInterface.cs
// 
// 2020-04-13-4:47 AM
// 
// ---
#endregion
using System ;
using AnalysisAppLib.XmlDoc ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDocInterface
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        TypeDocInfo GetTypeDocumentation ( Type type ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodDocumentation"></param>
        void CollectDoc ( CodeElementDocumentation methodDocumentation ) ;
    }
}