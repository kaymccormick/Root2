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
using System.Runtime.Serialization ;
using AnalysisAppLib ;
using KayMcCormick.Dev ;
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace ProjInterface.ViewModel
{
    public class AnalyzeResultsViewModel : IViewModel
    {
        public LogInvocationCollection LogInvocations { get ; } = new LogInvocationCollection ( ) ;

        public string CurrentProject { get ; }

        public string CurrentDocumentPath { get ; }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}