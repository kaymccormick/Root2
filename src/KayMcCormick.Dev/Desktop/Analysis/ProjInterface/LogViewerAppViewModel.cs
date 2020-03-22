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
using System ;
using System.Collections.ObjectModel ;
using System.Diagnostics ;
using System.Linq ;
using System.Runtime.Serialization ;
using System.Threading.Tasks ;
using System.Windows.Media.Animation ;
using KayMcCormick.Dev ;


namespace ProjInterface
{
#pragma warning disable DV2002 // Unmapped types
    public class LogViewerAppViewModel : IViewModel
#pragma warning restore DV2002 // Unmapped types

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