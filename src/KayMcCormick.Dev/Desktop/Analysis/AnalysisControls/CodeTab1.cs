using AnalysisControls.Properties;
using RibbonLib.Model;
using System.Collections.Generic;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeTab1 : IRibbonModelProvider<RibbonModelTab>
    {
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provs"></param>
       

        public CodeTab1()
        { }

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

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }
    }
}