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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        AppTypeInfo GetAppTypeInfo ( object identifier ) ;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary < string , object > CollectionMap ( ) ;
    }
}