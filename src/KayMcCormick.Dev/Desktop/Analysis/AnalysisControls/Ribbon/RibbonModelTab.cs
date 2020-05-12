using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using AnalysisAppLib;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls.RibbonM
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelTab
    {
        public Visibility Visibility { get; set; } = Visibility.Visible;
        public object ContextualTabGroupHeader { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Header { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<RibbonModelItem> Items { get; } = new ObservableCollection<RibbonModelItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public RibbonModelGroup CreateGroup(string @group)
        {
            var r = new RibbonModelGroup() {Header = @group};
            Items.Add(r);
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"RibbonModelTab[{Header}]";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelTabDefinition : IRibbonModelTabDefinition
    {

    }

    [CategoryMetadata(Category.Infrastructure)]
    public class TestRibbonTabDef : RibbonModelTab
    {

        public TestRibbonTabDef([MetadataFilter("Category", Category.Infrastructure)]IEnumerable<RibbonModelGroup> groups)
        {
            Header = Category.Infrastructure;
            foreach (var ribbonModelGroup in groups)
            {
                Items.Add(ribbonModelGroup);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [CategoryMetadata(Category.Infrastructure)]
    public class RibbonModelGroupTest1 : RibbonModelGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public RibbonModelGroupTest1(IEnumerable<IDisplayableAppCommand> commands)
        {
            Header = Category.Infrastructure;
            foreach (var cmd in commands)
            {
                var b = CreateButton(cmd.DisplayName);
                b.Command = cmd.Command;
                //Items.Add(cmd);
            }
        }
    }

    [CategoryMetadata(Category.Infrastructure)]
    public class RibbonModelGroupTest2 : RibbonModelGroup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public RibbonModelGroupTest2(IEnumerable<Meta<Lazy<IBaseLibCommand>>> commands)
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
    public interface IRibbonModelTabDefinition  
    {
    }
}