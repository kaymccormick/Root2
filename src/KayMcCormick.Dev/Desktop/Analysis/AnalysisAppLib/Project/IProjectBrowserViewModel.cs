﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IProjectBrowserViewModel.cs
// 
// 2020-03-01-6:32 PM
// 
// ---
#endregion
using KayMcCormick.Dev ;

namespace AnalysisAppLib.Project
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectBrowserViewModel : IViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        IBrowserNodeCollection RootCollection { get ; }
    }
}