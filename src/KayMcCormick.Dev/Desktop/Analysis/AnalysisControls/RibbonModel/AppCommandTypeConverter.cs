using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    public class AppCommandTypeConverter:TypeConverter
    {
        private IEnumerable<Meta<Lazy<IAppCommand>>> _commands = Enumerable.Empty<Meta<Lazy<IAppCommand>>>();

        public AppCommandTypeConverter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public AppCommandTypeConverter(IEnumerable<Meta<Lazy<IAppCommand>>> commands)
        {
            _commands = commands.Where(x =>
            {
                var r = x.Metadata.TryGetValue("CommandId", out var cmdId);
                DebugUtils.WriteLine($"Command metadata");
                foreach (var keyValuePair in x.Metadata)
                {
                    DebugUtils.WriteLine($"{keyValuePair.Key} = {keyValuePair.Value}");
                }
                DebugUtils.WriteLine($"result is {r} and {cmdId}");
                return r;
            });
            if (!_commands.Any())
            {
                throw new AppInvalidOperationException("no matchin ccommands");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<IAppCommand> commandList = new List<IAppCommand>();
            
            foreach (var command in _commands)
            {
                if (command.Metadata.TryGetValue("CommandId", out var cmdId))
                {
                    commandList.Add(command.Value.Value);
                }
            }
            StandardValuesCollection x = new StandardValuesCollection(commandList);
            return x;
            return base.GetStandardValues(context);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
            return base.GetStandardValuesSupported(context);
        }
    }
}