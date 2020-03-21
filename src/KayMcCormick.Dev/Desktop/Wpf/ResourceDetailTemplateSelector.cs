using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceDetailTemplateSelector : ResourceTemplateSelector

    {
        // ReSharper disable once UnusedMember.Local
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private string _templatePartName ;
        #region Overrides of DataTemplateSelector
        #endregion

        #region Overrides of ResourceTemplateSelector
        public override string TemplatePartName => "Detail" ;
        #endregion
    }
}