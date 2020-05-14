using System;
using System.Collections.Generic;
using AnalysisAppLib;
using Autofac.Features.AttributeFilters;

namespace AnalysisControls.RibbonModel.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [CategoryMetadata(Category.Infrastructure)]
    public class InfrastructureTab : RibbonModelTab
    {
        /// <summary>
        /// 
        /// </summary>
        public IClientModel ClientModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        public InfrastructureTab([MetadataFilter("Category", Category.Infrastructure)]IEnumerable<RibbonModelGroup> groups)
        {
            Header = Category.Infrastructure;
            foreach (var ribbonModelGroup in groups)
            {
                Items.Add(ribbonModelGroup);
            }

            var debugGroup = CreateGroup("Debug");
            var button = debugGroup.CreateButton(null);
            

        }
    }
}