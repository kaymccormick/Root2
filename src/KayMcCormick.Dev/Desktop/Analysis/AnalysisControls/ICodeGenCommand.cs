using System.Threading.Tasks;
using Autofac;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;

namespace AnalysisControls
{
    public interface ICodeGenCommand
    {
        string PocoPrefix { get; }


    }
}