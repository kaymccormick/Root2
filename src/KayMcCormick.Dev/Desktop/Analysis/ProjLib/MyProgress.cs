#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// MyProgress.cs
// 
// 2020-03-06-5:15 PM
// 
// ---
#endregion
using System ;
using Microsoft.CodeAnalysis.MSBuild ;

namespace ProjLib
{
    public class MyProgress : IProgress < ProjectLoadProgress >
    {
        private readonly WorkspacesViewModel _workspacesViewModel ;

        public MyProgress ( WorkspacesViewModel workspacesViewModel )
        {
            _workspacesViewModel = workspacesViewModel ;
        }

        /// <summary>Reports a progress update.</summary>
        /// <param name="value">The value of the updated progress.</param>
        public void Report ( ProjectLoadProgress value )
        {
            _workspacesViewModel.CurrentProgress = new MyProjectLoadProgress (
                                                                              value.FilePath
                                                                            , value
                                                                             .Operation.ToString ( )
                                                                            , value.TargetFramework
                                                                            , value.ElapsedTime
                                                                             ) ;
        }
    }
}