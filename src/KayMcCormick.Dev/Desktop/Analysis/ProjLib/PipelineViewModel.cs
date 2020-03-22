#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// IPipelineViewModel.cs
// 
// 2020-02-29-2:31 PM
// 
// ---
#endregion
using System.Runtime.Serialization ;
using JetBrains.Annotations ;
using ProjLib.Interfaces ;

namespace ProjLib
{
    [ UsedImplicitly ]
    public class PipelineViewModel : IPipelineViewModel
    {
        private Pipeline _pipeline ;
        #region Implementation of IPipelineViewModel
        public Pipeline Pipeline { get => _pipeline ; set => _pipeline = value ; }
        #endregion

        public PipelineViewModel(Pipeline pipeline) {
            this._pipeline = pipeline;
}

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}
