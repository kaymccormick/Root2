#region header
// Kay McCormick (mccor)
// 
// LogViewer1
// LogViewer1
// ViewerLoggerInfo.cs
// 
// 2020-03-16-1:29 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Collections.ObjectModel ;

namespace ProjInterface
{
    public class ViewerLoggerInfo
    {
        private bool isExpanded = true ;

        private ObservableCollection<ViewerLoggerInfo> children = new ObservableCollection < ViewerLoggerInfo > ();
        public IDictionary < string , ViewerLoggerInfo > ChildrenLoggers
        {
            get => _childrenLoggers ;
        }

        public string LoggerName { get { return _loggerName ; } set { _loggerName = value ; } }

        public ObservableCollection < ViewerLoggerInfo > Children
        {
            get => children ;
            set => children = value ;
        }

        public string PartName { get { return _partName ; } set { _partName = value ; } }

        public string DisplayName { get { return _displayName ; } set { _displayName = value ; } }

        public bool IsExpanded { get => isExpanded ; set => isExpanded = value ; }

        private IDictionary< string, ViewerLoggerInfo > _childrenLoggers = new Dictionary < string , ViewerLoggerInfo > ();
        private string                            _loggerName ;
        private string                            _partName ;
        private string                            _displayName ;
    }
}