using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using System.Collections.Generic;
using KayMcCormick.Dev.Command;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Info3
    {
        Category category;
        string group;
        List<Meta<IBaseLibCommand>> commands = new List<Meta<IBaseLibCommand>>();

        /// <summary>
        /// 
        /// </summary>
        public Category Category { get => category; set => category = value; }
        /// <summary>
        /// 
        /// </summary>
        public string Group { get => group; set => group = value; }
    }
}