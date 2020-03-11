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
using JetBrains.Annotations ;

namespace ProjLib.Interfaces
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
    }
}
