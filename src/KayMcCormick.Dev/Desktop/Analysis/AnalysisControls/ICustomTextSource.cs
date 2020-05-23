﻿using System;
using System.Collections;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomTextSource
    {
        /// <summary>
        /// 
        /// </summary>
        int Length { get; }

        /// <summary>
        /// 
        /// </summary>
        FontRendering FontRendering { get; set; }

        FontFamily Family { get; set; }
        /// <summary>
        /// 
        /// </summary>
        double EmSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        GenericTextRunProperties BaseProps { get; }

        /// <summary>
        /// 
        /// </summary>
        int EolLength { get; }

    
        /// <summary>
        /// 
        /// </summary>
        double PixelsPerDip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSourceCharacterIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        TextRun GetTextRun(int textSourceCharacterIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSourceCharacterIndexLimit"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(
            int textSourceCharacterIndexLimit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSourceCharacterIndex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        BasicTextRunProperties BasicProps();

        /// <summary>
        /// 
        /// </summary>
        void GenerateText();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        TextRunProperties MakeProperties(object arg, string text);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void TakeTextRun(TextRun obj);

        void Init();
        /// <summary>
        /// 
        /// </summary>
        void UpdateCharMap();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
            
    }
}