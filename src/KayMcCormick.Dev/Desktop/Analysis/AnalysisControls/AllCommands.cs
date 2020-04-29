using System.Collections.Generic;
using System.Windows.Controls.Ribbon;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    public class AllCommands : IRibbonComponent
    {
        private IEnumerable<IBaseLibCommand> _commands;

        public AllCommands(IEnumerable<IBaseLibCommand> commands)
        {
            _commands = commands;
        }

        public object GetComponent()
        {

            return new RibbonComboBox() {ItemsSource = _commands};
        }
    }
}