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
namespace ProjLib
{
    public class BrowserNode : IBrowserNode
    {
        private string _name ;
        #region Implementation of IBrowserNode
        public string Name { get => _name ; set => _name = value ; }
        #endregion
    }
}