using System.ComponentModel;

namespace AnalysisControls.ViewModel
{
    public interface IContentSelector : INotifyPropertyChanged
    {
        /// <inheritdoc />
        void SetActiveContent(object doc);

        object ActiveContent { get; set; }
    }
}