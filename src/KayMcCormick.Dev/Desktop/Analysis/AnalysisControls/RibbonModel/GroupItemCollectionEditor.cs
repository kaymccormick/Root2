using System;
using System.ComponentModel.Design;

namespace AnalysisControls.RibbonModel
{
    public class GroupItemCollectionEditor : CollectionEditor
    {
        public GroupItemCollectionEditor() : base(typeof(RibbonModelGroupItemCollection))
        {
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new[] {typeof(RibbonModelItemButton), typeof(RibbonModelItemComboBox)};
        }
    }
}