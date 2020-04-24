using System.Windows;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class HierarchicalTemplateSelector : ResourceTemplateSelector
    {
        private string _templatePartName ="treeViewNode";

        public override string DefaultTemplateName => "TreeViewNode";
        #region Overrides of ResourceTemplateSelector
        /// <summary>
        /// 
        /// </summary>
        protected override string TemplatePartName { get { return _templatePartName ; } set { _templatePartName = value ; } }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return base.SelectTemplate(item, container);
        }

        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
    }
}
