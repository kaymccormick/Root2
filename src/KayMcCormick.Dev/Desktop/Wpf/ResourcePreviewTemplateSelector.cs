using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    internal sealed class ResourcePreviewTemplateSelector : ResourceTemplateSelector
    {
        #region Overrides of ResourceTemplateSelector
        /// <summary>
        /// </summary>
        [ NotNull ] protected override string TemplatePartName { get { return "Preview" ; } }
        #endregion
    }
}