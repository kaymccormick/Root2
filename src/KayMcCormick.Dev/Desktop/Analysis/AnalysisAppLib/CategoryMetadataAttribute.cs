using System;
using System.ComponentModel.Composition;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    [MetadataAttribute]
    public class CategoryMetadataAttribute : Attribute
    {
        private Category category;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logUsage"></param>
        public CategoryMetadataAttribute(Category logUsage)
        {
            this.Category = logUsage;
        }

        /// <summary>
        /// 
        /// </summary>
        public Category Category { get => category; set => category = value; }
    }
    /// <summary>
    /// 
    /// </summary>
    [MetadataAttribute]
    public class GroupMetadataAttribute : Attribute
    {
        private string group;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public GroupMetadataAttribute(string  @group)
        {
            this.Group = @group;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Group { get => group; set => group = value; }
    }
}