#region header
// Kay McCormick (mccor)
// 
// LogViewer1
// LogViewer1
// AppViewModel.cs
// 
// 2020-03-16-3:36 AM
// 
// ---
#endregion
using System.Collections.ObjectModel ;
using System.Runtime.Serialization ;
using AnalysisAppLib.ViewModel ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib
{

    public class LogViewerAppViewModel : IViewModel


    {
        private ObservableCollection < LogViewModel > _logViewModels =
            new ObservableCollection < LogViewModel > ( ) ;

        public ObservableCollection < LogViewModel > LogViewModels
        {
            get => _logViewModels ;
            set => _logViewModels = value ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}