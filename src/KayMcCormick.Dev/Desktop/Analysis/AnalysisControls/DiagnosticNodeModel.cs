using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DiagnosticNodeModel : PathModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static DiagnosticNodeModel CreateInstance(PathModelKind kind)
        {
            return new DiagnosticNodeModel(kind);
        }

        private Diagnostic _diagnostic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        private DiagnosticNodeModel(PathModelKind kind) : base(kind)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docs"></param>
        public override void Add(PathModel docs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Diagnostic Diagnostic
        {
            get { return _diagnostic; }
            set
            {
                _diagnostic = value;
                Message = _diagnostic.GetMessage();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }
    }
}