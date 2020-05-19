using System.Collections.Generic;

namespace AnalysisAppLib
{

    /// <summary>
    /// 
    /// </summary>
    public class BaseInfo
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public class Info1
    {
        /// <summary>
        /// 
        /// </summary>
        public Info1 ()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public Category Category {get;  set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Info2> Infos { get; internal set; } = new Dictionary<string, Info2>();
        /// <summary>
        /// 
        /// </summary>
        public List<CommandInfo> Ungrouped { get; internal set; } = new List<CommandInfo>();
    }
}