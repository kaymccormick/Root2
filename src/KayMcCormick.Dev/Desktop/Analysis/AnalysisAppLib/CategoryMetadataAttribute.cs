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
        // ReSharper disable once MemberCanBePrivate.Global
        public Category Category { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [MetadataAttribute]
    public class GroupMetadataAttribute : Attribute
    {
        private string _group;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        public GroupMetadataAttribute(string  @group)
        {
            this.Group = group;
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public string Group { get => _group; set => _group = value; }
    }
}