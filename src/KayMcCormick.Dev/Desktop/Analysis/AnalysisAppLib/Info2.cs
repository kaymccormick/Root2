using System.Collections.Generic;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Info2
    {
        string group;
        Category category;
        /// <summary>
        /// 
        /// </summary>
        public string Group { get => group; set => group = value; }
        /// <summary>
        /// 
        /// </summary>
        public List<CommandInfo> Infos { get; set; } = new List<CommandInfo>();
        /// <summary>
        /// 
        /// </summary>
        public Category Category { get => category; set => category = value; }
    }
}