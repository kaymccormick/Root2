using System.Collections;

namespace AnalysisControls.ViewModel
{
    public interface IDocumentHost
    {
        void AddDocument(object doc);
        void SetActiveDocument(object doc);
        IEnumerable Documents { get; }
    }
}