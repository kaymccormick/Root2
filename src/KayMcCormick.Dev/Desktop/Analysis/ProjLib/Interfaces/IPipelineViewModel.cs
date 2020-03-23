using AnalysisAppLib ;
using KayMcCormick.Dev ;

namespace ProjLib.Interfaces
{
    public interface IPipelineViewModel : IViewModel
    {
        Pipeline Pipeline { get ; }

    }
}