using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Autofac.Features.Metadata;
using Microsoft.EntityFrameworkCore.Internal;

namespace AnalysisControls.RibbonModel
{
    /// <inheritdoc />
    public class DerpTab : RibbonModelTab , ISupportInitialize
    {
        private readonly IEnumerable<Meta<RibbonModelGroup>> _groups;

        public DerpTab(IEnumerable<Meta<RibbonModelGroup>> groups)
        {
            _groups = groups;
        }

        /// <inheritdoc />
        public override void BeginInit()
        {
        }

        /// <inheritdoc />
        public override void EndInit()
        {
            if (Header != null || Items.Any())
                return;
            Visibility = Visibility.Visible;
            ContextualTabGroupHeader = "Assemblies";
            Header = "Derp";
            foreach (var ribbonModelGroup in _groups)
            {
                // var props = MetaHelper.GetMetadataProps(ribbonModelGroup.Metadata);
                // DebugUtils.WriteLine($"{props.TabHeader} {ContextualTabGroupHeader}");
                // if (props.TabHeader != null && props.TabHeader.Equals((string)Header))
                // {
                Items.Add(ribbonModelGroup.Value);
                // }
                break;
            }
        }
    }
    
}