using System;
using System.Collections.Generic;
using AnalysisAppLib;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls.RibbonModel
{
    /// <summary>
    /// 
    /// </summary>
    [CategoryMetadata(Category.Infrastructure)]
    public class BaseLibCommandGroup : RibbonModelGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public BaseLibCommandGroup(IEnumerable<Meta<Lazy<IBaseLibCommand>>> commands)
        {
            Header = Category.Infrastructure;
            foreach (var cmd in commands)
            {
                var props = MetaHelper.GetMetadataProps(cmd.Metadata);
                var b = CreateButton(props.Title);
                b.Command = new LambdaAppCommand(props.Title, command => cmd.Value.Value.ExecuteAsync(), null, null);
                //Items.Add(cmd);
            }
        }
    }
}