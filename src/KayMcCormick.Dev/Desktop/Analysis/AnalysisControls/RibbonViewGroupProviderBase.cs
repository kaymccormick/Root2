﻿using AnalysisControls.Properties;
using Autofac;
using RibbonLib.Model;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RibbonViewGroupProviderBase : IRibbonModelProvider<RibbonModelGroup>
    {
        /// <inheritdoc />
        public abstract RibbonModelGroup ProvideModelItem();

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class RibbonContextualTabGroupProviderBase : IRibbonModelProvider<RibbonModelContextualTabGroup>
    {
        /// <inheritdoc />
        public abstract RibbonModelContextualTabGroup ProvideModelItem();

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CodeAnalysisContextualTabGroupProvider : RibbonContextualTabGroupProviderBase
    {
        public override RibbonModelContextualTabGroup ProvideModelItem()
        {
            return new RibbonModelContextualTabGroup
            {
                Header = "Code Analysis"
            };
        }
    }
}
