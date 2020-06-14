#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// AnalyzeResultsViewModel.cs
// 
// 2020-04-17-2:38 PM
// 
// ---
#endregion

#if FINDLOGUSAGE

using System.Runtime.Serialization;
using AnalysisAppLib;
using KayMcCormick.Dev;

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Client2.ViewModel
{
    public class AnalyzeResultsViewModel : IViewModel
    {
        public ##if LogInvocationCollection LogInvocations { get ; } = new LogInvocationCollection ( ) ;

        public string CurrentProject { get ; }

        public string CurrentDocumentPath { get ; }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}
#endif