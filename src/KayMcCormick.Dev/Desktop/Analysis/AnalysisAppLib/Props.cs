namespace AnalysisAppLib
{
    public class Props
    {
        public string Title { get; set; }
        public Category Category { get; set; }

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}";
        }
    }
}