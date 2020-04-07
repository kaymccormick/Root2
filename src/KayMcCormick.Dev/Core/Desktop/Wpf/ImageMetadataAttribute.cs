using System;
using System.Composition ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    [MetadataAttribute]
    public sealed class ImageMetadataAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string ImageSource { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageSource"></param>
        public ImageMetadataAttribute ( string imageSource ) { ImageSource = imageSource ; }
    }
}
