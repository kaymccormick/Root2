#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// CodeAnalysisApp1
// CodeSource.cs
// 
// 2020-02-26-8:51 AM
// 
// ---
#endregion
namespace AnalysisFramework
{
    public class CodeSource : ICodeSource
    {
        public string FilePath { get ; set ; }

        
        public CodeSource ( string filePath ) { FilePath = filePath ; }
    }
}