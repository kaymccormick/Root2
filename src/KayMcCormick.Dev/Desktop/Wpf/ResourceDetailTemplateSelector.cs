using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public sealed class ResourceDetailTemplateSelector : ResourceTemplateSelector

    {
        #region Overrides of ResourceTemplateSelector
        /// <summary>
        /// </summary>
        [ NotNull ] public override string TemplatePartName { get { return "Detail" ; } }
        #endregion
        #region Overrides of DataTemplateSelector
        #endregion
    }
}