#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IAppState.cs
// 
// 2020-02-29-7:48 AM
// 
// ---
#endregion
namespace ProjLib.Interfaces
{
    public interface IAppState
    {
        bool Processing { get ; set ; }

        string CurrentProject { get ; set ; }

        string CurrentDocumentPath { get ; set ; }
    }
}