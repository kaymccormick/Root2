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
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    [ UsedImplicitly ]
    public class LogViewerAppViewModel : IViewModel


    {
        private ObservableCollection < LogViewModel > _logViewModels =
            new ObservableCollection < LogViewModel > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < LogViewModel > LogViewModels
        {
            get { return _logViewModels ; }
            set { _logViewModels = value ; }
        }

        public object InstanceObjectId { get; set; }

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}