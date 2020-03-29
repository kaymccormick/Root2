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

namespace AnalysisAppLib
{
    public class ViewerLoggerInfo
    {
        private readonly IDictionary < string , ViewerLoggerInfo > _childrenLoggers =
            new Dictionary < string , ViewerLoggerInfo > ( ) ;

        private string _displayName ;
        private string _loggerName ;
        private string _partName ;

        private ObservableCollection < ViewerLoggerInfo > children =
            new ObservableCollection < ViewerLoggerInfo > ( ) ;

        private bool isExpanded = true ;

        public IDictionary < string , ViewerLoggerInfo > ChildrenLoggers
        {
            get { return _childrenLoggers ; }
        }

        public string LoggerName { get { return _loggerName ; } set { _loggerName = value ; } }

        public ObservableCollection < ViewerLoggerInfo > Children
        {
            get { return children ; }
            set { children = value ; }
        }

        public string PartName { get { return _partName ; } set { _partName = value ; } }

        public string DisplayName { get { return _displayName ; } set { _displayName = value ; } }

        public bool IsExpanded { get { return isExpanded ; } set { isExpanded = value ; } }
    }
}