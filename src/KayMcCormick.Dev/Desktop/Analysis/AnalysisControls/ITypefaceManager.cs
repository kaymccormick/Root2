using System.Windows;
using System.Windows.Media;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITypefaceManager
    {
        Typeface GetDefaultTypeface();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emSize"></param>
        /// <param name="left"></param>
        /// <param name="textDecorationCollection"></param>
        /// <param name="brush"></param>
        /// <param name="typeface"></param>
        /// <returns></returns>
        FontRendering GetRendering(double emSize, TextAlignment left, TextDecorationCollection textDecorationCollection, Brush brush, Typeface typeface);
    }
}