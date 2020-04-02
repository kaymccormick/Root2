using System;
using System.Collections.Generic;
using System.Composition ;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media ;

namespace KayMcCormick.Lib.Wpf
{
    [MetadataAttribute]
    public class ImageMetadataAttribute : Attribute
    {
        public string ImageSource { get ; set ; }

        public ImageMetadataAttribute ( string imageSource ) { ImageSource = imageSource ; }
    }
}
