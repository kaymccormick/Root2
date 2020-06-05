using System.Collections.Generic;
using AnalysisControls.Properties;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.CodeAnalysis.CSharp;
using RibbonLib.Model;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeTab1 : IRibbonModelProvider<RibbonModelTab>
    {
        private IEnumerable<IRibbonModelProvider<RibbonModelGroup>> _provs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provs"></param>
        public CodeTab1(IEnumerable<IRibbonModelProvider<RibbonModelGroup>> provs)
        {
            _provs = provs;
        }


        /// <inheritdoc />
        public RibbonModelTab ProvideModelItem()
        {
            var tab = new CodeContextualTab
            {
                Header = "Code", ContextualTabGroupHeader = RibbonResources.ContextualTabGroupHeader_CodeAnalysis
            };

            return tab;
        }
    }
}