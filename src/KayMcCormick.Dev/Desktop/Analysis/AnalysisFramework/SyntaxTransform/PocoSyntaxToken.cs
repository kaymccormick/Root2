namespace AnalysisFramework.SyntaxTransform
{
    public class PocoSyntaxToken
    {
        private string _kind;
        private int    rawKind;
        private object value;

        public PocoSyntaxToken(string Kind, int rawKind, object value)
        {
            this.Kind    = Kind;
            this.RawKind = rawKind;
            this.Value   = value;
        }

        public string Kind { get => _kind ; set => _kind = value ; }

        public int RawKind { get => rawKind ; set => rawKind = value ; }

        public object Value { get => value ; set => this.value = value ; }
    }
}