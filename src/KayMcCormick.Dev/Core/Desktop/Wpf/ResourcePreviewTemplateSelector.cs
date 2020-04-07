namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public sealed class ResourcePreviewTemplateSelector : ResourceTemplateSelector
    {
        #region Overrides of ResourceTemplateSelector
        /// <summary>
        /// </summary>
        public override string TemplatePartName { get { return "Preview" ; } }
        #endregion
    }
}