﻿#region header
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
using KayMcCormick.Dev ;

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// 
    /// </summary>
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