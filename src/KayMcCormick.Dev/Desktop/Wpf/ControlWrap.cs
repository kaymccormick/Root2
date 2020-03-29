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
    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ControlWrap<T> : IWrap<T>, IWrap1 where T : UIElement
    {
        private object _controlObject ;

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public T Control { get ; }

        /// <summary>
        /// 
        /// </summary>
        public ControlWrap ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public ControlWrap ( T c ) { Control = c ; }

        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        public object ControlObject => Control ;
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IWrap1
    {
        /// <summary>
        /// 
        /// </summary>
        object ControlObject { get ; }
    }
}