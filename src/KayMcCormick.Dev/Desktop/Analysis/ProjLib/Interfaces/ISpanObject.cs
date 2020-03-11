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
namespace ProjLib.Interfaces
{
    public interface ISpanObject < T >
    {
        T Instance { get ; set ; }
    }
}