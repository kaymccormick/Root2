using System;
using System.ComponentModel.Design;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupItemCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// 
        /// </summary>
        public GroupItemCollectionEditor() : base(typeof(RibbonModelGroupItemCollection))
        {
        }

        /// <inheritdoc />
        protected override Type[] CreateNewItemTypes()
        {
            return new[] {typeof(RibbonModelItemButton), typeof(RibbonModelItemComboBox)};
        }
    }
}