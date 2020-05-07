using System.Collections;
using System.Windows.Media.TextFormatting;
using KayMcCormick.Dev.Logging;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    public abstract class AppTextSource : TextSource
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract void Init();
        /// <summary>
        /// 
        /// </summary>
        public abstract int Length { get; protected set; }

        public abstract GenericTextRunProperties BaseProps { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract MyTextRunProperties BasicProps();

        public abstract TextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ISymbol symbol);

        
    }
}