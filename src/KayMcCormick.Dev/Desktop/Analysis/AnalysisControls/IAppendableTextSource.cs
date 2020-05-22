using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppendableTextSource:ICustomTextSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eNewItems"></param>
        void AppendRange(IEnumerable eNewItems);
        void SetSource(IEnumerable source);
    }
}