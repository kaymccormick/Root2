#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// MyProjectLoadProgress.cs
// 
// 2020-03-06-5:14 PM
// 
// ---
#endregion
using System ;
using System.IO ;
using Microsoft.CodeAnalysis.MSBuild ;

namespace ProjLib
{
    public class MyProjectLoadProgress
    {
        public MyProjectLoadProgress (
            string   filePath
          , string   operation
          , string   targetFramework
          , TimeSpan elapsedTime
        )
        {
            FilePath        = filePath ;
            Operation       = operation ;
            TargetFramework = targetFramework ;
            ElapsedTime     = elapsedTime ;
        }

        /// <summary>
        ///     The project for which progress is being reported.
        /// </summary>
        public string FilePath { get ; }

        public string FileName => Path.GetFileNameWithoutExtension ( FilePath ) ;

        /// <summary>
        ///     The operation that has just completed.
        /// </summary>
        public string Operation { get ; }

        /// <summary>
        ///     The target framework of the project being built or resolved. This
        ///     property is only valid for SDK-style projects
        ///     during the <see cref="ProjectLoadOperation.Resolve" /> operation.
        /// </summary>
        public string TargetFramework { get ; }

        /// <summary>
        ///     The amount of time elapsed for this operation.
        /// </summary>
        public TimeSpan ElapsedTime { get ; }
    }
}