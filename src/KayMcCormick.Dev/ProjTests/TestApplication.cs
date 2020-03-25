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
        private TaskCompletionSource < bool > _tcs ;
        public  TaskCompletionSource < bool > TCS { get { return _tcs ; } set { _tcs = value ; } }
        #region Overrides of Application
        protected override void OnExit ( ExitEventArgs e )
        {
            base.OnExit ( e ) ;
            _tcs.TrySetResult ( true ) ;
        }

        protected override void OnStartup ( StartupEventArgs e ) { base.OnStartup ( e ) ; }
        #endregion
    }
}