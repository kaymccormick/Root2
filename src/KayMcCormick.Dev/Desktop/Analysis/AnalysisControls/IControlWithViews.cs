using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AnalysisControls
{
    public interface IControlWithViews
    {
        public ObservableCollection<ViewSpec> Views { get; set; }
        public ViewSpec CurrentView { get; set; }
    }
}