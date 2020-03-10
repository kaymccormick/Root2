

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
        #region Overrides of BaseApp
        public override IEnumerable < IModule > GetModules ( ) { yield break ; }
        #if COMMANDLINE
        protected override void OnArgumentParseError ( IEnumerable < object > obj ) { }
#endif
        #endregion
    }
}
