namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HierarchicalTemplateSelector : ResourceTemplateSelector
    {
        private string _templatePartName ="treeViewNode";
        #region Overrides of ResourceTemplateSelector
        /// <summary>
        /// 
        /// </summary>
        protected override string TemplatePartName { get { return _templatePartName ; } set { _templatePartName = value ; } }
        #endregion
    }
}