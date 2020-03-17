#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// WpfApp
// ControlWrap.cs
// 
// 2020-03-12-2:15 AM
// 
// ---
#endregion
using System.IO ;
using System.Windows ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;

namespace KayMcCormick.Lib.Wpf
{
    public class ControlWrap<T> where T : FrameworkElement
    {
        public T Control { get ; }
        public ControlWrap ( T c ) { Control = c ; }

        public BitmapSource Image
        {
            get
            {
                RenderTargetBitmap renderTargetBitmap =
                    new RenderTargetBitmap (
                                            ( int ) Control.ActualWidth
                                          , ( int ) Control.ActualHeight
                                          , 96
                                          , 96
                                          , PixelFormats.Pbgra32
                                           ) ;
                renderTargetBitmap.Render ( Control ) ;
                return renderTargetBitmap ;
            }
        }
    }
}