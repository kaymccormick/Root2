using System.Collections.ObjectModel;

namespace AnalysisControls.RibbonM
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelItemComboBox : RibbonModelItem
    {
        public RibbonModelItemComboBox()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> Items { get; } = new ObservableCollection<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="combo"></param>
        /// <returns></returns>
        public object CreateGallery()
        {
            var g = RibbonModel.CreateGallery();
            Items.Add(g);
            return g;
        }
    }
}