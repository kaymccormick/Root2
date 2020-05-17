namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class TextPropertiesManager : ITextPropertiesManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelsPerDip"></param>
        /// <param name="fontRendering"></param>
        /// <returns></returns>
        public static GenericTextRunProperties GetBasicTextRunProperties(double pixelsPerDip,
            FontRendering fontRendering)
        {
            return new GenericTextRunProperties(
                fontRendering,
                pixelsPerDip);
        }
    }
}