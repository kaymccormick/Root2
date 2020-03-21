#region header
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
using System ;
using System.Collections.ObjectModel ;
using System.Diagnostics ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Windows.Media.Animation ;


namespace ProjInterface
{
    public class AppViewModel

    {
        private ObservableCollection < LogViewModel > _logViewModels =
            new ObservableCollection < LogViewModel > ( ) ;

        public ObservableCollection < LogViewModel > LogViewModels
        {
            get => _logViewModels ;
            set => _logViewModels = value ;
        }

        // public async Task EnumerateAllServicesFromAllHosts ( )
        // {
        //     var domains = await ZeroconfResolver.BrowseDomainsAsync ( ) ;
        //     foreach ( var grouping in domains )
        //     {
        //         Debug.WriteLine (
        //                          $"KEy = [{grouping.Key}]: ({string.Join ( ", " , domains[ grouping.Key ] )})"
        //                         ) ;
        //     }
        //
        //     var responses = await ZeroconfResolver.ResolveAsync ( domains.Select ( g => g.Key ) ) ;
        //     foreach ( var resp in responses )
        //     {
        //         Debug.WriteLine ( "1: " + resp.DisplayName ) ;
        //         foreach ( var keyValuePair in resp.Services )
        //         {
        //             Debug.WriteLine ( $"Key is {keyValuePair.Key}" ) ;
        //             Debug.WriteLine ( keyValuePair.Value.Port) ;
        //         }
        //     }
        // }
    }
}