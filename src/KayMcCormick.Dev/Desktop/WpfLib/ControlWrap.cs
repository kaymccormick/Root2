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
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ControlWrap < T > : IWrap < T > , IWrap1
        where T : UIElement
    {
#pragma warning disable 169
        private object _controlObject ;
#pragma warning restore 169

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public ControlWrap ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="c"></param>
        public ControlWrap ( T c ) { Control = c ; }

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public T Control { get ; }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [ JsonIgnore ] [ NotNull ] public ImageSource Image
        {
            get
            {
                try
                {
                    if ( ( object ) Control is FrameworkElement fe )
                    {
                        var renderTargetBitmap = new RenderTargetBitmap (
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
                    // ignored
                }

                return new BitmapImage ( ) ;
            }
        }

        #region Implementation of IWrap1
        /// <summary>
        /// </summary>
        [JsonIgnore]
        public object ControlObject { get { return Control ; } }
        #endregion
    }

    /// <summary>
    /// </summary>
    public interface IWrap1
    {
        /// <summary>
        /// </summary>
        object ControlObject { get ; }
    }
}