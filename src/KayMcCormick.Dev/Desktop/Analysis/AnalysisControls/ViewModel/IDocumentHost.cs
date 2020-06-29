using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using KayMcCormick.Dev.Interfaces;

namespace AnalysisControls.ViewModel
{
    public interface IDocumentHost : IHaveObjectId
    {
        void AddDocument(object doc);
        IEnumerable Documents { get; }
    }

    public interface IContentSelector
    {
        /// <inheritdoc />
        void SetActiveContent(object doc);

        object ActiveContent { get; set; }
    }

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

    class DocumentHost : IDocumentHost, INotifyPropertyChanged
    {
        

        public DocumentHost()
        {
            Documents = DocumentsCollection;
        }

        /// <inheritdoc />
        public void AddDocument(object doc)
        {
            DocumentsCollection.Add(doc);
        }

        public ObservableCollection<object> DocumentsCollection { get; set; } = new ObservableCollection<object>(
            );

        /// <inheritdoc />
        public IEnumerable Documents { get; }

        public object InstanceObjectId { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}