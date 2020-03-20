using System.Windows ;
using System.Windows.Controls ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourcePreviewTemplateSelector : ResourceTemplateSelector
    {
        private string _templatePartName ;
        #region Overrides of ResourceTemplateSelector
        public override string TemplatePartName => "Preview" ;
        #endregion
    }
}