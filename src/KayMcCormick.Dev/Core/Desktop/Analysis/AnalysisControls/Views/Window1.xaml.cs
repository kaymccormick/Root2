using Autofac ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls.Views
{
    /// <summary>
    ///     Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : AppWindow
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifetimeScope"></param>
        public Window1 ( ILifetimeScope lifetimeScope ) : base ( lifetimeScope )
        {
            InitializeComponent ( ) ;
        }
    }
}