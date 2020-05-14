using System.Windows.Input;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RibbonModelItem
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract ControlKind Kind { get; }
        /// <summary>
        /// 
        /// </summary>
        public object Label { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommand Command { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object CommandTarget { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object CommandParameter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object LargeImageSource
        {
            get;
            set;
        }

        public object SmallImageSource { get; set; }
        public override string ToString()
        {
            return "RibbonModelItem (Kind=" + Kind + ")";
        }
    }
}