namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class ResourceKeyTemplateSelector: ResourceTemplateSelector
    {
        private string _templatePartName = "Key" ;
        #region Overrides of ResourceTemplateSelector
        /// <inheritdoc />
        protected override string TemplatePartName { get { return _templatePartName ; } set { _templatePartName = value ; } }
        #endregion
    }
}