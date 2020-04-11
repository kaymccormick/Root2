#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjLib
// IBrowserNodeCollection.cs
// 
// 2020-03-08-8:14 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Collections.Specialized ;
using System.ComponentModel ;

namespace AnalysisAppLib.XmlDoc.Project
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBrowserNodeCollection : ICollection < IBrowserNode >
      , INotifyCollectionChanged
      , INotifyPropertyChanged
    {
    }
}