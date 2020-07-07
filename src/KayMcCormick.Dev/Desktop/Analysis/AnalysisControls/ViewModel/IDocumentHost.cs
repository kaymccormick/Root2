using System.Collections;
using KayMcCormick.Dev.Interfaces;

namespace AnalysisControls.ViewModel
{
    public interface IDocumentHost : IHaveObjectId
    {
        void AddDocument(object doc);
        IEnumerable Documents { get; }
    }
}