using System;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Props
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object TabHeader { get; set; }

        public Type TypeHint { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}";
        }
    }
}