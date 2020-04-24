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
    public interface IEventMisc
    {
        int       ThreadId { get ; }
        object    Obj      { get; }
        string    Message  { get; }
        MiscLevel Level    { get; set; }
        string    RawJson  { get; set; }

        string File     { get  ; }
        string PropKeys { get ; }
    }
}