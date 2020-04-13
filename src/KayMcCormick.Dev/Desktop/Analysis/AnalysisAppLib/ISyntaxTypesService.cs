#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// ISyntaxTypesService.cs
// 
// 2020-04-13-3:57 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using AnalysisAppLib.XmlDoc ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib
{
    /// <summary>
    /// </summary>
    public interface ISyntaxTypesService
    {
        AppTypeInfo GetAppTypeInfo(object identifier);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="DebugOut"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, object> CollectionMap(
        ) ;
    }
}