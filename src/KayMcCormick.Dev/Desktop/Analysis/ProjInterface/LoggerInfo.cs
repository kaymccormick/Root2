#region header
// Kay McCormick (mccor)
// 
// LogViewer1
// LogViewer1
// LoggerInfo.cs
// 
// 2020-03-16-1:29 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Collections.ObjectModel ;

namespace LogViewer1
{
    public class LoggerInfo
    {
        private bool isExpanded = true ;

        private ObservableCollection<LoggerInfo> children = new ObservableCollection < LoggerInfo > ();
        public IDictionary < string , LoggerInfo > ChildrenLoggers
        {
            get => _childrenLoggers ;
        }

        public string LoggerName { get { return _loggerName ; } set { _loggerName = value ; } }

        public ObservableCollection < LoggerInfo > Children
        {
            get => children ;
            set => children = value ;
        }

        public string PartName { get { return _partName ; } set { _partName = value ; } }

        public string DisplayName { get { return _displayName ; } set { _displayName = value ; } }

        public bool IsExpanded { get => isExpanded ; set => isExpanded = value ; }

        private IDictionary< string, LoggerInfo > _childrenLoggers = new Dictionary < string , LoggerInfo > ();
        private string                            _loggerName ;
        private string                            _partName ;
        private string                            _displayName ;
    }
}