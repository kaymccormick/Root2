#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IProjectBrowserViewModoel.cs
// 
// 2020-03-01-6:32 PM
// 
// ---
#endregion
namespace AnalysisAppLib.ViewModel
{
    public interface IProjectBrowserViewModel : IViewModel
    {
        IBrowserNodeCollection RootCollection { get ; }
    }
}