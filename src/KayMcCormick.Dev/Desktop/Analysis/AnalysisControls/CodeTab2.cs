using System.Collections.Generic;
using AnalysisControls.Properties;
using RibbonLib.Model;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeTab2 : IRibbonModelProvider<RibbonModelTab>
    {
        private IEnumerable<IRibbonModelProvider<RibbonModelGroup>> _provs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provs"></param>
        public CodeTab2(IEnumerable<IRibbonModelProvider<RibbonModelGroup>> provs)
        {
            _provs = provs;
        }


        /// <inheritdoc />
        public RibbonModelTab ProvideModelItem()
        {
            var tab = new CodeContextualTab
            {
                Header = "Code2", ContextualTabGroupHeader = RibbonResources.ContextualTabGroupHeader_CodeAnalysis
            };

            return tab;
        }
    }
}