using System.Collections.ObjectModel;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MessagesModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<WorkspaceMessage> Messages { get;  }= new ObservableCollection<WorkspaceMessage>();
    }
}