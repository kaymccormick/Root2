using System;
using System.Collections.Generic;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;

namespace AnalysisAppLib
{
    public static class MetaHelper
    {
        public static Props GetProps(Meta<Lazy<IBaseLibCommand>> meta)
        {
            var metaMetadata = meta.Metadata;
            return GetMetadataProps(metaMetadata);
        }

        public static Props GetMetadataProps(IDictionary<string, object> metaMetadata)

        {
            var props = new Props();
            if (metaMetadata.TryGetValue("Title", out var title))
            {
                props.Title = title.ToString();
            }

            if (metaMetadata.TryGetValue("Category", out var category))
            {
                Category c = (Category) category;
                props.Category = c;
            }

            if (metaMetadata.TryGetValue("TabHeader", out var tabHeader))
            {
                props.TabHeader = tabHeader;
            }

            return props;
        }


    }
}