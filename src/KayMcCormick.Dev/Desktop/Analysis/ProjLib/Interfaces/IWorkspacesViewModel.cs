#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// IWorkspacesViewModel.cs
// 
// 2020-02-19-11:29 AM
// 
// ---
#endregion
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Threading.Tasks ;
using KayMcCormick.Dev ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace ProjLib.Interfaces
{
    public interface IWorkspacesViewModel : INotifyPropertyChanged , IAppState , ILogUsageAnalysisViewModel
    {
        IProjectBrowserViewModel ProjectBrowserViewModel { get ; }

        string ApplicationMode { get ; }

        AdhocWorkspace Workspace { get ; set ; }

        ObservableCollection < LogEventInfo > EventInfos { get ; }
    }
}