using System.Windows.Input;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelAppMenuItem : RibbonModelAppMenuElement
    {
        public ICommand Command { get; set; }
    }
}