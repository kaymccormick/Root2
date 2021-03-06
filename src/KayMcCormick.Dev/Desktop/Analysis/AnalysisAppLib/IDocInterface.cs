﻿#region header
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
        ICodeElementDocumentation GetTypeDocumentation ( Type type ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodDocumentation"></param>
        void CollectDoc ( ICodeElementDocumentation methodDocumentation ) ;
    }
}