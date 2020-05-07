using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentModel
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public ProjectModel Project { get; set; }
        public Document Document { get; set; }
    }
}