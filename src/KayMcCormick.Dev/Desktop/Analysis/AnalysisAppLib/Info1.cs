using System.Collections.Generic;

namespace AnalysisAppLib
{

    public class BaseInfo
    {

    }
    public class Info1
    {
        public Info1 ()
        {

        }
        public Category Category {get;  set; }
        public Dictionary<string, Info2> Infos { get; internal set; } = new Dictionary<string, Info2>();
        public List<CommandInfo> Ungrouped { get; internal set; } = new List<CommandInfo>();
    }
}