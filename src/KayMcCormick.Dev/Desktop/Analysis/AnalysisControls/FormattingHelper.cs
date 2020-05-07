﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class FormattingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="currentRendering"></param>
        /// <param name="_emSize"></param>
        /// <param name="emSize"></param>
        /// <param name="typeface"></param>
        /// <param name="textDest"></param>
        /// <param name="textStore"></param>
        /// <param name="pixelsPerDip"></param>
        /// <param name="lineInfos"></param>
        /// <param name="infos"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <param name="textLineAction"></param>
        public static void UpdateFormattedText(double width, ref FontRendering currentRendering, double emSize,
            [NotNull] Typeface typeface, DrawingGroup textDest, CustomTextSource3 textStore, double pixelsPerDip,
            IList<LineInfo> lineInfos, List<RegionInfo> infos, ref double maxX, out double maxY,
            Action<TextLine> textLineAction)
        {
            if (typeface == null) throw new ArgumentNullException(nameof(typeface));
            if (currentRendering == null)
                currentRendering = new FontRendering(
                    emSize,
                    TextAlignment.Left,
                    null,
                    Brushes.Black,
                    typeface);

            var dc0 = textDest.Open();

            var formatter = TextFormatter.Create();

            // Format each line of text from the text store and draw it.
            TextLineBreak prev = null;
            var pos = new Point(0, 0);
            var lineContext = new LineContext();

            while (lineContext.TextStorePosition < textStore.Length)
            {
                lineContext.CurCellRow++;
                using (var myTextLine = formatter.FormatLine(
                    textStore,
                    lineContext.TextStorePosition,
                    width,
                    new GenericTextParagraphProperties(currentRendering, pixelsPerDip),
                    prev))
                {
                    HandleTextLine(infos, lineContext, dc0, out var lineInfo);
                    lineInfos.Add(lineInfo);
                }
            }

            dc0.Close();
            maxY = lineContext.LineOriginPoint.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineInfos"></param>
        /// <param name="infos"></param>
        /// <param name="maxX"></param>
        /// <param name="textLineAction"></param>
        /// <param name="myTextLine"></param>
        /// <param name="linePosition"></param>
        /// <param name="curCellRow"></param>
        /// <param name="lineNumber"></param>
        /// <param name="textStorePosition"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="maxX"></param>
        /// <param name="textLineAction"></param>
        /// <param name="myTextLine"></param>
        /// <param name="linePosition"></param>
        /// <param name="curCellRow"></param>
        /// <param name="lineNumber"></param>
        /// <param name="textStorePosition"></param>
        /// <param name="dc"></param>
        /// <param name="lineInfo"></param>
        /// <param name="context"></param>
        /// <param name="lineInfos"></param>
        /// <returns></returns>
        public static void HandleTextLine(List<RegionInfo> infos, LineContext lineContext, DrawingContext dc,
            out LineInfo lineInfo)
        {
            lineContext.LineParts.Clear();
            lineContext.TextLineAction?.Invoke(lineContext.MyTextLine);
            var dd = SaveDrawingGroup(lineContext);

            lineInfo = new LineInfo
            {
                Offset = lineContext.TextStorePosition,
                Length = lineContext.MyTextLine.Length,
                Size = new Size(lineContext.MyTextLine.WidthIncludingTrailingWhitespace,
                    lineContext.MyTextLine.Height),
                Origin = new Point(lineContext.LineOriginPoint.X, lineContext.LineOriginPoint.Y)
            };
            DebugUtils.WriteLine($"{lineInfo}");

            var location = lineContext.LineOriginPoint;

            var textRunSpans = lineContext.MyTextLine.GetTextRunSpans();
            var cell = lineContext.LineOriginPoint;
            var cellColumn = 0;
            var characterOffset = lineContext.TextStorePosition;
            var regionOffset = lineContext.TextStorePosition;
            var lineBuilder = new StringBuilder();
            var eol = lineContext.MyTextLine.GetTextRunSpans().Select(xx => xx.Value).OfType<TextEndOfLine>();
            if (eol.Any())
                dc.DrawRectangle(Brushes.Aqua, null,
                    new Rect(
                        lineContext.LineOriginPoint.X + lineContext.MyTextLine.WidthIncludingTrailingWhitespace + 2,
                        lineContext.LineOriginPoint.Y + 2, 10, 10));
            DebugUtils.WriteLine("no end of line");
            foreach (var textRunSpan in lineContext.MyTextLine.GetTextRunSpans())
            {
                if (textRunSpan.Value is CustomTextCharacters c)
                {
                    lineContext.Offsets.Add(c.Index.GetValueOrDefault());
                    lineContext.LineParts.Add(c.Text);
                }

                DebugUtils.WriteLine(textRunSpan.Value.ToString());
            }


            var lineRegions = new List<RegionInfo>();

            var lineString = "";
            var group = 0;
            var regionNumber = 0;
            var indexedGlyphRuns = lineContext.MyTextLine.GetIndexedGlyphRuns();
            if (indexedGlyphRuns != null)
                foreach (var rect in indexedGlyphRuns)
                {
                    regionNumber++;
                    var rectGlyphRun = rect.GlyphRun;
                    if (rectGlyphRun == null) continue;
                    var size = new Size(0, 0);
                    var cellBounds =
                        new List<CharacterCell>();
                    var renderingEmSize = rectGlyphRun.FontRenderingEmSize;

                    for (var i = 0; i < rectGlyphRun.Characters.Count; i++)
                    {
                        var advanceWidth = rectGlyphRun.AdvanceWidths[i];
                        size.Width += advanceWidth;
                        var gi = rectGlyphRun.GlyphIndices[i];
                        var c = rectGlyphRun.Characters[i];
                        lineString += c;
                        var advWidth = rectGlyphRun.GlyphTypeface.AdvanceWidths[gi];
                        var advHeight = rectGlyphRun.GlyphTypeface.AdvanceHeights[gi];

                        var width = advWidth * renderingEmSize;

                        var height = (advHeight
                                      + rectGlyphRun.GlyphTypeface.BottomSideBearings[gi])
                                     * renderingEmSize;
                        var s = new Size(width,
                            height);

                        var topSide = rectGlyphRun.GlyphTypeface.TopSideBearings[gi];
                        var bounds = new Rect(new Point(cell.X, cell.Y + topSide), s);

                        cellBounds.Add(new CharacterCell(bounds, new Point(cellColumn, lineContext.CurCellRow), c));
                        cell.Offset(advanceWidth, 0);

                        cellColumn++;
                        characterOffset++;
                    }

                    size.Height += lineContext.MyTextLine.Height;
                    var r = new Rect(location, size);
                    location.Offset(size.Width, 0);
                    if (@group < textRunSpans.Count)
                    {
                        var textSpan = textRunSpans[@group];
                        var textSpanValue = textSpan.Value;
                        SyntaxNode node = null;
                        SyntaxToken? token = null;
                        SyntaxTrivia? trivia = null;
                        if (textSpanValue is SyntaxTokenTextCharacters stc)
                        {
                            node = stc.Node;
                            token = stc.Token;
                        }
                        else if (textSpanValue is SyntaxTriviaTextCharacters stc2)
                        {
                            trivia = stc2.Trivia;
                        }

                        var tuple = new RegionInfo(textSpanValue, r, cellBounds)
                        {
                            Key = $"{lineContext.LineNumber}.{regionNumber}",
                            Offset = regionOffset,
                            Length = textSpan.Length,
                            SyntaxNode = node,
                            SyntaxToken = token,
                            Trivia = trivia
                        };
                        lineRegions.Add(tuple);
                        infos.Add(tuple);
                    }

                    @group++;
                    regionOffset = characterOffset;
                }

            lineInfo.Text = String.Join("", lineContext.LineParts);
            lineInfo.Regions = lineRegions;

            // Draw the formatted text into the drawing context.
            lineContext.MyTextLine.Draw(dc, lineContext.LineOriginPoint, InvertAxes.None);
            // ReSharper disable once UnusedVariable
            var p = new Point(lineContext.LineOriginPoint.X + lineContext.MyTextLine.WidthIncludingTrailingWhitespace,
                lineContext.LineOriginPoint.Y);
            var textLineBreak = lineContext.MyTextLine.GetTextLineBreak();
            if (textLineBreak != null) DebugUtils.WriteLine(textLineBreak.ToString());
            lineContext.LineNumber++;

            // Update the index position in the text store.
            lineContext.TextStorePosition += lineContext.MyTextLine.Length;
            // Update the line position coordinate for the displayed line.
            lineContext.LineOriginPoint.Offset(0, lineContext.MyTextLine.Height);
            if (lineContext.MyTextLine.Width >= lineContext.MaxX) lineContext.MaxX = lineContext.MyTextLine.Width;
            lineContext.ReturnValue = lineInfo;
        }

        private static DrawingGroup SaveDrawingGroup(LineContext lineContext)
        {
            var dd = new DrawingGroup();
            var dc1 = dd.Open();
            dc1.DrawRectangle(Brushes.White, null,
                new Rect(0, 0, lineContext.MyTextLine.WidthIncludingTrailingWhitespace, lineContext.MyTextLine.Height));
            lineContext.MyTextLine.Draw(dc1, new Point(0, 0), InvertAxes.None);
            dc1.Close();
            var imgWidth = (int) dd.Bounds.Width;
            var imgHeight = (int) dd.Bounds.Height;
            if (imgWidth > 0 && imgHeight > 0)
                SaveImage(dd, lineContext.LineNumber.ToString(),
                    imgWidth, imgHeight);
            return dd;
        }

        private static void SaveImage(DrawingGroup drawingGroup,
            string filePrefix, int width, int height)
        {
            DebugUtils.WriteLine("Creating image " + $"({width},{height}) {filePrefix}.png");
            var v = new DrawingVisual();
            var dc = v.RenderOpen();
            var bounds = drawingGroup.Bounds;

            var brush = new DrawingBrush(drawingGroup);
            dc.DrawRectangle(
                brush, null, bounds);
            dc.Close();
            var rtb = new RenderTargetBitmap(width, height, 96,
                96,
                PixelFormats.Pbgra32);
            rtb.Render(v);

            var png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
            var fname = $"{filePrefix}.png";
            using (var s = File.Create("C:\\temp\\" + fname))
            {
                png.Save(s);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="compilation"></param>
        /// <param name="syntaxTree"></param>
        /// <param name="pixelsPerDip"></param>
        /// <param name="emSize"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static SyntaxNodeCustomTextSource UpdateTextSource(SyntaxNode node, CSharpCompilation compilation,
            SyntaxTree syntaxTree,
            double pixelsPerDip, double emSize)
        {
            if (compilation != null && compilation.SyntaxTrees.Contains(syntaxTree) == false)
                throw new InvalidOperationException("Compilation does not contain syntax tree.");

            if (ReferenceEquals(node.SyntaxTree, syntaxTree) == false)
                throw new InvalidOperationException("Node is not within syntax tree");
            var store = new SyntaxNodeCustomTextSource(pixelsPerDip)
            {
                EmSize = emSize,
                Compilation = compilation,
                Tree = syntaxTree,
                Node = node
            };
            store.Init();
            return store;
        }
    }
}