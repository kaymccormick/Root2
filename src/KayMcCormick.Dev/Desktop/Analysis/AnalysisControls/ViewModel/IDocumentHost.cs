using System.Collections;
using System.Collections.ObjectModel;
using KayMcCormick.Dev.Interfaces;

namespace AnalysisControls.ViewModel
{
    public interface IDocumentHost : IHaveObjectId
    {
        void AddDocument(object doc);
        void SetActiveDocument(object doc);
        IEnumerable Documents { get; }
    }

    class DocumentHost : IDocumentHost
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
        public void SetActiveDocument(object doc)
        {
        }

        /// <inheritdoc />
        public IEnumerable Documents { get; }

        public object InstanceObjectId { get; set; }
    }
}