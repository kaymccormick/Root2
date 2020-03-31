#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ISpanObject.cs
// 
// 2020-02-27-5:22 AM
// 
// ---
#endregion
namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpanObject < T >
    {
        /// <summary>
        /// 
        /// </summary>
        T Instance { get ; set ; }
    }
}