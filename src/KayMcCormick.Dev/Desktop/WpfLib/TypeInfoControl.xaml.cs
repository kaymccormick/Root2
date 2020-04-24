using System ;
using System.Windows.Controls ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    ///     Interaction logic for TypeInfoControl.xaml
    /// </summary>
    public sealed partial class TypeInfoControl : UserControl
    {
        /// <summary>
        /// </summary>
        public TypeInfoControl ( ) { InitializeComponent ( ) ; }

        /// <summary>
        /// </summary>
        [ NotNull ] public Type DesignContext { get { return typeof ( TypeInfoControl ) ; } }
    }
}