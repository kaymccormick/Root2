#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjLib
// BrowserNode.cs
// 
// 2020-03-08-8:15 PM
// 
// ---
#endregion
namespace AnalysisAppLib.Projects
{
    /// <summary>
    /// 
    /// </summary>
    public class BrowserNode : IBrowserNode
    {
        private string _name ;
        #region Implementation of IBrowserNode
        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return _name ; } set { _name = value ; } }
        #endregion
    }
}