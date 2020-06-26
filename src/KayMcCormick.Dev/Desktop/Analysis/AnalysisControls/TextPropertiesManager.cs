using System;
using JetBrains.Annotations;

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
        /// <param name="rendering"></param>
        /// <returns></returns>
        public static GenericTextRunProperties GetBasicTextRunProperties([NotNull] FontRendering rendering)
        {
            if (rendering == null) throw new ArgumentNullException(nameof(rendering));
            return new GenericTextRunProperties(
                rendering
                );
        }
    }
}