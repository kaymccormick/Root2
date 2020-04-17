namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class HierarchicalTemplateSelector : ResourceTemplateSelector
    {
        private string _templatePartName ="treeViewNode";
        #region Overrides of ResourceTemplateSelector
        /// <summary>
        /// 
        /// </summary>
        public override string TemplatePartName { get { return _templatePartName ; } set { _templatePartName = value ; } }
        #endregion
    }
}