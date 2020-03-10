#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// BuildResults.cs
// 
// 2020-03-04-11:03 AM
// 
// ---
#endregion
using System.Collections.Generic ;
// ReSharper disable UnusedMember.Global

namespace ProjLib
{
    public class BuildResults
    {
        private string sourceDir ;

        public List < string > SolutionsFilesList { get ; set ; } = new List < string > ( ) ;

        public string SourceDir { get => sourceDir ; set => sourceDir = value ; }
    }
}