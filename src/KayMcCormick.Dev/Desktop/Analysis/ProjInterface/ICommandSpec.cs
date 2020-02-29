using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjInterface
{
    interface ICommandSpec
    {
        IEnumerable < IBoundCommandOperation > Operations { get ; }
    }

    internal interface IBoundCommandOperation : IBoundOperation, ICommandSource
    {
    }

    internal interface IBoundOperation : IAbstractOperation
    {
    }

    internal interface IAbstractOperation   
    {
    }
}
