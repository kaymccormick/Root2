#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// IEventMisc2.cs
// 
// 2020-04-23-8:22 PM
// 
// ---
#endregion
namespace AnalysisAppLib
{
    public interface IEventMisc2< out T>:IEventMisc
    {
        T Instance { get ; }
    }
}