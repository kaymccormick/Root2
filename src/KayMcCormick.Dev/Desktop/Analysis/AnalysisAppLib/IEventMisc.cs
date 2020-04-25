#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// IEventMisc.cs
// 
// 2020-04-23-8:21 PM
// 
// ---
#endregion
namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventMisc
    {
        /// <summary>
        /// 
        /// </summary>
        int       ThreadId { get ; }
        /// <summary>
        /// 
        /// </summary>
        object    Obj      { get; }
        /// <summary>
        /// 
        /// </summary>
        string    Message  { get; }
        /// <summary>
        /// 
        /// </summary>
        MiscLevel Level    { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string    RawJson  { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string File     { get  ; }
        /// <summary>
        /// 
        /// </summary>
        string PropKeys { get ; }
    }
}