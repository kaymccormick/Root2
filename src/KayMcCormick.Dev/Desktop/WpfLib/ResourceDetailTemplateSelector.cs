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
        [ NotNull ] protected override string TemplatePartName { get { return "Detail" ; } }
        #endregion
        #region Overrides of DataTemplateSelector
        #endregion
    }
}