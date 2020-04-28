using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using System.Collections.Generic;

namespace AnalysisAppLib
{
    public class Info3
    {
        Category category;
        string group;
        List<Meta<IBaseLibCommand>> commands = new List<Meta<IBaseLibCommand>>();

        public Category Category { get => category; set => category = value; }
        public string Group { get => group; set => group = value; }
    }
}