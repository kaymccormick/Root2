using KayMcCormick.Dev ;
using KayMcCormick.Dev.Interfaces ;

namespace ProjLib.Interfaces
{
    public interface IPipelineViewModel : IViewModel
    {
        Pipeline Pipeline { get ; }

    }
}