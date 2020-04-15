namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ResourceKeyTemplateSelector: ResourceTemplateSelector
    {
        private string _templatePartName = "Key" ;
        #region Overrides of ResourceTemplateSelector
        /// <inheritdoc />
        public override string TemplatePartName { get { return _templatePartName ; } set { _templatePartName = value ; } }
        #endregion
    }
}