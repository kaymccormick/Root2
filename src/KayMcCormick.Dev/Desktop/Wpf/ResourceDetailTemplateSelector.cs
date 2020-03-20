using System.Collections.Generic ;
using System.Diagnostics ;
using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceDetailTemplateSelector : ResourceTemplateSelector

    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private string _templatePartName ;
        #region Overrides of DataTemplateSelector
        #endregion

        #region Overrides of ResourceTemplateSelector
        public override string TemplatePartName => "Detail" ;
        #endregion
    }
}