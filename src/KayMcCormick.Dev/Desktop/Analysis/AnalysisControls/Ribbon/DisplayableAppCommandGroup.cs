using System.Collections.Generic;
using AnalysisAppLib;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    [CategoryMetadata(Category.Infrastructure)]
    public class DisplayableAppCommandGroup : RibbonModelGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public DisplayableAppCommandGroup(IEnumerable<IDisplayableAppCommand> commands)
        {
            Header = Category.Infrastructure;
            foreach (var cmd in commands)
            {
                var b = CreateButton(cmd.DisplayName);
                b.Command = cmd.Command;
                //Items.Add(cmd);
            }
        }
    }
}