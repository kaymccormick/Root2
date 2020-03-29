#region header
// Kay McCormick (mccor)
// 
// Analysis
// ProjTests
// TestApplication.cs
// 
// 2020-03-24-9:09 PM
// 
// ---
#endregion
using System.Threading.Tasks ;
using System.Windows ;

namespace ProjTests
{
    public class TestApplication : Application
    {
        public TaskCompletionSource < bool > TCS { get ; set ; }

        #region Overrides of Application
        protected override void OnExit ( ExitEventArgs e )
        {
            base.OnExit ( e ) ;
            TCS.TrySetResult ( true ) ;
        }

        protected override void OnStartup ( StartupEventArgs e ) { base.OnStartup ( e ) ; }
        #endregion
    }
}