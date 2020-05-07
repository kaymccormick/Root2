using System;
using System.ComponentModel.Composition;

namespace KayMcCormick.Dev
{
    [MetadataAttribute]
    public class ShortKeyMetadataAttribute : Attribute
    {
        public string ShortKey { get; }

        public ShortKeyMetadataAttribute(string shortKey)
        {
            ShortKey = shortKey;
        }
    }
}