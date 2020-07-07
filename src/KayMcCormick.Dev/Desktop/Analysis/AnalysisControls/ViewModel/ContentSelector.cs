using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AnalysisControls.ViewModel
{
    public class ContentSelector : IContentSelector, INotifyPropertyChanged
    {
        private object _activeContent;

        /// <inheritdoc />
        public void SetActiveContent(object doc)
        {
            ActiveContent = doc;
        }

        /// <inheritdoc />
        public object ActiveContent
        {
            get { return _activeContent; }
            set
            {
                if (Equals(value, _activeContent)) return;
                _activeContent = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}