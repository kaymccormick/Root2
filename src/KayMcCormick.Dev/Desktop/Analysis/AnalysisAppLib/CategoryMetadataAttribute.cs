using System;
using System.ComponentModel.Composition;

namespace AnalysisAppLib
{
    [MetadataAttribute]
    public class CategoryMetadataAttribute : Attribute
    {
        private Category category;

        public CategoryMetadataAttribute(Category logUsage)
        {
            this.Category = logUsage;
        }

        public Category Category { get => category; set => category = value; }
    }
    [MetadataAttribute]
    public class GroupMetadataAttribute : Attribute
    {
        private string group;

        public GroupMetadataAttribute(string  logUsage)
        {
            this.Group = logUsage;
        }

        public string Group { get => group; set => group = value; }
    }
}