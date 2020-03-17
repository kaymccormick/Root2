using System.Windows.Controls ;
using NLog ;

namespace WpfApp.Controls
{
    /// <summary>Control for displaying IOC registrations.</summary>
    public partial class Registrations : UserControl
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0052:Remove unread private members", Justification = "<Pending>")]
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>Parameterless constructor.</summary>
        public Registrations ( )
        {
            InitializeComponent ( ) ;
        }

    }
}
