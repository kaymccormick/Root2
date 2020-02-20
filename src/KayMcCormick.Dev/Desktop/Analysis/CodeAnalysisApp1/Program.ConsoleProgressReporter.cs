#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// CodeAnalysisApp1
// Program.ConsoleProgressReporter.cs
// 
// 2020-02-12-7:20 PM
// 
// ---
#endregion
using System ;
using System.IO ;
using Microsoft.CodeAnalysis.MSBuild ;
 
namespace CodeAnalysisApp1
{
    internal static partial class Program
    {
        private class ConsoleProgressReporter : IProgress < ProjectLoadProgress >
        {
            public void Report ( ProjectLoadProgress loadProgress )
            {
                var projectDisplay = Path.GetFileName ( loadProgress.FilePath ) ;
                if ( loadProgress.TargetFramework != null )
                {
                    projectDisplay += $" ({loadProgress.TargetFramework})" ;
                }

                Console.WriteLine (
                                   // ReSharper disable once LocalizableElement
                                   $"{loadProgress.Operation,- 15} {loadProgress.ElapsedTime,- 15:m\\:ss\\.fffffff} {projectDisplay}"
                                  ) ;
            }
        }
    }
}