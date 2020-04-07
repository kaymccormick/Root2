﻿#region header
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
    /// <summary>
    /// 
    /// </summary>
    public sealed class ViewerLoggerInfo
    {
        private readonly IDictionary < string , ViewerLoggerInfo > _childrenLoggers =
            new Dictionary < string , ViewerLoggerInfo > ( ) ;

        private string _displayName ;
        private string _loggerName ;
        private string _partName ;

        private ObservableCollection < ViewerLoggerInfo > children =
            new ObservableCollection < ViewerLoggerInfo > ( ) ;

        private bool isExpanded = true ;

        /// <summary>
        /// 
        /// </summary>
        public IDictionary < string , ViewerLoggerInfo > ChildrenLoggers
        {
            get { return _childrenLoggers ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LoggerName { get { return _loggerName ; } set { _loggerName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < ViewerLoggerInfo > Children
        {
            get { return children ; }
            set { children = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PartName { get { return _partName ; } set { _partName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get { return _displayName ; } set { _displayName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsExpanded { get { return isExpanded ; } set { isExpanded = value ; } }
    }
}