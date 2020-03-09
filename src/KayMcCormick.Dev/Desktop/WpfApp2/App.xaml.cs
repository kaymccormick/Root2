

using System.Collections.Generic ;
using Autofac.Core ;
using KayMcCormick.Lib.Wpf ;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : BaseApp
    {
        /// <summary>Initializes a new instance of the <see cref="System.Windows.Application" /> class.</summary>
        /// <exception cref="System.InvalidOperationException">More than one instance of the <see cref="System.Windows.Application" /> class is created per <see cref="System.AppDomain" />.</exception>
        public App ( ) {
        }

        #region Overrides of BaseApp
        public override IEnumerable < IModule > GetModules ( ) { yield break ; }

        protected override void OnArgumentParseError ( IEnumerable < object > obj ) { }
        #endregion
    }
}
