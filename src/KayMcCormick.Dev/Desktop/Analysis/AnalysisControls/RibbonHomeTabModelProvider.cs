using System;
using System.Collections.Generic;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Lib.Wpf.Command;
using RibbonLib.Model;

namespace AnalysisControls
{
    [UsedImplicitly]
    class RibbonHomeTabModelProvider : RibbonModelTabProvider1
    {
        private readonly IEnumerable<Meta<Lazy<IAppCommand>>> _commands;
        public RibbonHomeTabModelProvider(Func<RibbonModelTab> factory, [MetadataFilter("TabHeader" , "Home")] IEnumerable<Meta<Lazy<IAppCommand>>> commands) : this(factory)
        {
            _commands = commands;
        }
        /// <inheritdoc />
        public override RibbonModelTab ProvideModelItem()
        {
            var r = Factory();
            r.Header = "Home";
            return r;
        }

        /// <inheritdoc />
        public RibbonHomeTabModelProvider(Func<RibbonModelTab> factory) : base(factory)
        {
        }
    }
}