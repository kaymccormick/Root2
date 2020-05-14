using System.Collections;
using System.Windows.Media.TextFormatting;
using KayMcCormick.Dev.Logging;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public abstract GenericTextRunProperties BaseProps { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract BasicTextRunProperties BasicProps();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolDisplayPart"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public abstract TextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ISymbol symbol);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="InsertionPoint"></param>
        /// <param name="text"></param>
        public abstract void TextInput(int InsertionPoint, string text);
    }
}