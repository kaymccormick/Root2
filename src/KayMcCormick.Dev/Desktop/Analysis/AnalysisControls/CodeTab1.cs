﻿using AnalysisControls.Properties;
using RibbonLib.Model;
using System.Collections.Generic;

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
                Header = "Code",
                ContextualTabGroupHeader = "Code Analysis"
            };

            return tab;
        }
    }
}