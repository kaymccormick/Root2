#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjLib
// BrowserNodeCollection.cs
// 
// 2020-03-08-8:14 PM
// 
// ---
#endregion
using System ;
using System.Collections.ObjectModel ;

namespace AnalysisAppLib.Project
{
    [Serializable]
    internal class BrowserNodeCollection : ObservableCollection < IBrowserNode >
      , IBrowserNodeCollection
    {
    }
}