using System.Windows ;
using KayMcCormick.Dev ;
using ProjInterface.ViewModel ;

namespace ProjInterface
{
    /// <summary>
    ///     Interaction logic for AnalyzeResults.xaml
    /// </summary>
    public partial class AnalyzeResults : Window , IView1 , IView < AnalyzeResultsViewModel >
    {
        public AnalyzeResults ( ) { InitializeComponent ( ) ; }

        #region Implementation of IView<out AnalyzeResultsViewModel>
        public AnalyzeResultsViewModel ViewModel { get ; }
        #endregion
    }
}