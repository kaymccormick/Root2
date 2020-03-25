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
using System ;
using System.Text.Json.Serialization ;
using System.Windows ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;

namespace KayMcCormick.Lib.Wpf
{
    public class ControlWrap<T> : IWrap<T>, IWrap1 where T : UIElement
    {
        private object _controlObject ;

        [JsonIgnore]
        public T Control { get ; }

        public ControlWrap ( ) {
        }

        public ControlWrap ( T c ) { Control = c ; }

        [JsonIgnore]
        public ImageSource Image
        {
            get
            {
                try
                {
                    if ( ( object ) this.Control is FrameworkElement fe )
                    {
                        RenderTargetBitmap renderTargetBitmap =
                            new RenderTargetBitmap (
                                                    ( int ) fe.ActualWidth
                                                  , ( int ) fe.ActualHeight
                                                  , 96
                                                  , 96
                                                  , PixelFormats.Pbgra32
                                                   ) ;
                        renderTargetBitmap.Render ( Control ) ;
                        return renderTargetBitmap ;
                    }
                }
                catch ( Exception )
                {
                
                }
                return new BitmapImage();
                
            }
        }

        #region Implementation of IWrap1
        public object ControlObject => Control ;
        #endregion
    }

    public interface IWrap1
    {
        object ControlObject { get ; }
    }
}