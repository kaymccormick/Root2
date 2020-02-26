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
namespace CodeAnalysisApp1
{
    public class CodeSource : ICodeSource
    {
        public string FilePath { get ; set ; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public CodeSource ( string filePath ) { FilePath = filePath ; }
    }
}