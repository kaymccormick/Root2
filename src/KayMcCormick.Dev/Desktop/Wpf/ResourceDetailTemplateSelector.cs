using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceDetailTemplateSelector : ResourceTemplateSelector

    {
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private string _templatePartName ;
        #region Overrides of DataTemplateSelector
        #endregion

        #region Overrides of ResourceTemplateSelector
        /// <summary>
        /// 
        /// </summary>
        public override string TemplatePartName => "Detail" ;
        #endregion
    }
}