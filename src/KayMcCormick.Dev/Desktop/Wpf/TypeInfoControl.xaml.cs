using System ;
using System.Windows.Controls ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    ///     Interaction logic for TypeInfoControl.xaml
    /// </summary>
    public partial class TypeInfoControl : UserControl
    {
        /// <summary>
        /// </summary>
        public TypeInfoControl ( ) { InitializeComponent ( ) ; }

        /// <summary>
        /// </summary>
        public Type DesignContext { get { return typeof ( TypeInfoControl ) ; } }
    }
}