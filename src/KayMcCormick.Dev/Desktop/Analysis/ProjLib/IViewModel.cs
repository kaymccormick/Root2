#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IViewModel.cs
// 
// 2020-02-29-2:32 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;

namespace ProjLib
{
    public interface IViewModel
    {
    }

    class WorkspaceViewModel : IViewModel
    {
        private AdhocWorkspace workspace ;

        public WorkspaceViewModel ( ) {
            
        }
    }
}