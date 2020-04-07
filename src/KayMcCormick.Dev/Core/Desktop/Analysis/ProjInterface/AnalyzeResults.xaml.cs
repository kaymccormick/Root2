using System.Runtime.Serialization ;
using System.Windows ;
using KayMcCormick.Dev ;

namespace ProjInterface
{
    /// <summary>
    ///     Interaction logic for AnalyzeResults.xaml
    /// </summary>
    public partial class AnalyzeResults : Window, IView1, IView <AnalyzeResultsViewModel>
    {
        public AnalyzeResults ( ) { InitializeComponent ( ) ; }

        #region Implementation of IView<out AnalyzeResultsViewModel>
        public AnalyzeResultsViewModel ViewModel { get ; }
        #endregion
    }

    public class AnalyzeResultsViewModel : IViewModel
    {
        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}