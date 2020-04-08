using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KayMcCormick.Dev.Command ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// Base command provided by low-level class lib
    /// </summary>
    public interface IBaseLibCommand
    {
        /// <summary>
        /// </summary>
        object Argument { get ; set ; }

        /// <summary>
        /// An Async method to execute the command.
        /// </summary>
        /// <returns></returns>
        Task < IAppCommandResult > ExecuteAsync ( ) ;

        /// <summary>
        /// A method to handle faults.
        /// </summary>
        /// <param name="exception">Exception</param>
        // ReSharper disable once UnusedMemberInSuper.Global
        void OnFault ( AggregateException exception ) ;
    }
}
