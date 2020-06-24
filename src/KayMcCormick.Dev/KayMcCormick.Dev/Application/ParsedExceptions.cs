using System.Collections.Generic;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    /// </summary>
    public sealed class ParsedExceptions
    {
        private List<ParsedStackInfo> _parsedList = new List<ParsedStackInfo>();

        /// <summary>
        /// </summary>
        public List<ParsedStackInfo> ParsedList
        {
            get { return _parsedList; }
            set { _parsedList = value; }
        }
    }
}