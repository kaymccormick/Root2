using System.Collections.Generic;
using AnalysisAppLib;
using Autofac.Features.AttributeFilters;

namespace AnalysisControls.RibbonModel.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [CategoryMetadata(Category.Infrastructure)]
    public class TestRibbonTabDef : RibbonModelTab
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        public TestRibbonTabDef([MetadataFilter("Category", Category.Infrastructure)]IEnumerable<RibbonModelGroup> groups)
        {
            Header = Category.Infrastructure;
            foreach (var ribbonModelGroup in groups)
            {
                Items.Add(ribbonModelGroup);
            }
        }
    }
}