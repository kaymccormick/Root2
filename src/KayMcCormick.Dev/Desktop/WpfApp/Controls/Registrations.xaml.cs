using System.Windows ;
using System.Windows.Controls ;
using KayMcCormick.Lib.Wpf ;
using NLog ;

namespace WpfApp.Controls
{
    /// <summary>Control for displaying IOC registrations.</summary>
    public partial class Registrations : UserControl
    {
        /// <summary>The lifetime scope property</summary>
        public static readonly DependencyProperty LifetimeScopeProperty =
            AttachedProperties.LifetimeScopeProperty ;

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
