using System ;
using System.Windows.Controls ;
using System.Windows.Input ;
using NLog ;

namespace WpfApp.Controls
{
    /// <summary>
    ///     Control for displaying known assemblies.
    /// </summary>
    public partial class AssemblyBrowser : UserControl
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        // ReSharper disable once InconsistentNaming
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>
        ///     Parameterless constructor.
        /// </summary>
        public AssemblyBrowser ( ) { InitializeComponent ( ) ; }

        private void LoadAssemblyList ( object sender , ExecutedRoutedEventArgs e )
        {
            throw new NotImplementedException ( ) ;
        }

        private void CanLoadAssemblyList ( object sender , CanExecuteRoutedEventArgs e ) { }
    }
}
