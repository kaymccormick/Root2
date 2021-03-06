﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using KayMcCormick.Dev.Interfaces;

namespace KayMcCormick.Dev.Command
{
    /// <summary>
    /// Base command provided by low-level class lib
    /// </summary>
    [TypeConverter(typeof(BaseLibTypeConverter))]
    public interface IBaseLibCommand : IHaveObjectId
    {
        /// <summary>
        /// </summary>
        object Argument { get ; set ; }

        /// <summary>
        /// An Async method to execute the command.
        /// </summary>
        /// <returns></returns>
        Task < IAppCommandResult > ExecuteAsync (object parameter ) ;

        /// <summary>
        /// A method to handle faults.
        /// </summary>
        /// <param name="exception">Exception</param>
        // ReSharper disable once UnusedMemberInSuper.Global
        void OnFault ( AggregateException exception ) ;
    }
}
