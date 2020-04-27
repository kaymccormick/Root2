using System;
using System.ComponentModel.Composition;

namespace AnalysisAppLib
{
    [MetadataAttribute]
    internal class CategoryMetadataAttribute : Attribute
    {
        private Category logUsage;

        public CategoryMetadataAttribute(Category logUsage)
        {
            this.logUsage = logUsage;
        }
    }
}