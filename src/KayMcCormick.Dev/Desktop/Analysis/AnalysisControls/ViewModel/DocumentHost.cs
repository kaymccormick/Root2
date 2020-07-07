using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AnalysisControls.ViewModel
{
    internal class DocumentHost : IDocumentHost, INotifyPropertyChanged
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