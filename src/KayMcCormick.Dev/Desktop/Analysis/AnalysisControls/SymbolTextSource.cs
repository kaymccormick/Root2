using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SymbolTextSource : AppTextSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelsPerDip"></param>
        public SymbolTextSource(double pixelsPerDip)
        {
            PixelsPerDip = pixelsPerDip;
            _typeface = new Typeface(Family, _fontStyle, _fontWeight,
                _fontStretch);
            _fontRendering = FontRendering.CreateInstance(EmSize, TextAlignment.Left, new TextDecorationCollection(), Brushes.Blue,
                _typeface);
            BaseProps = new GenericTextRunProperties(
                _fontRendering,
                PixelsPerDip);
        }

        /// <summary>
        /// 
        /// </summary>
        public CSharpCompilation Compilation { get; set; }
        public override GenericTextRunProperties BaseProps { get; }
        public override int Length { get; protected set; }

        // Used by the TextFormatter object to retrieve a run of text from the text source.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSourceCharacterIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            // Make sure text source index is in bounds.
            if (textSourceCharacterIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(textSourceCharacterIndex), "Value must be greater than 0.");
            if (textSourceCharacterIndex >= Length) return new TextEndOfParagraph(1);

            // Create TextCharacters using the current font rendering properties.
            if (textSourceCharacterIndex < Length)
            {
                var xx = chars[textSourceCharacterIndex];
                // var xx2 = chars2[textSourceCharacterIndex];
                if (col[xx] is CustomTextCharacters tc)
                {
                    var z = textSourceCharacterIndex - tc.Index;
                    var zz = tc.Length - z;

                    var tf = tc.Properties.Typeface;
                    return tc;
                }

                return col[xx];
            }

            // Return an end-of-paragraph if no more text source.
            return new TextEndOfParagraph(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSourceCharacterIndexLimit"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(
            int textSourceCharacterIndexLimit)
        {
            throw new NotImplementedException();
            // CharacterBufferRange cbr = new CharacterBufferRange(_text, 0, textSourceCharacterIndexLimit);
            // return new TextSpan<CultureSpecificCharacterBufferRange>(
            // textSourceCharacterIndexLimit,
            // new CultureSpecificCharacterBufferRange(CultureInfo.CurrentUICulture, cbr)
            // );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSourceCharacterIndex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private static string FullyQualifiedMetadataName(Type myType)
        {
            return (myType.Namespace != null ? myType.Namespace + "." : "") + myType.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override BasicTextRunProperties BasicProps()
        {
            var xx = new BasicTextRunProperties(BaseProps);
            return xx;
        }

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public FontRendering FontRendering { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxNode Node
        {
            get { return _node; }
            set
            {
                _node = value;
            }
        }

        private void GenerateText()
        {
            var parts = Symbol.ToDisplayParts(SymbolDisplayFormat);
            foreach (var symbolDisplayPart in parts)
            {
                SymbolTextCharacters c = new SymbolTextCharacters(symbolDisplayPart.ToString(), 0, symbolDisplayPart.ToString().Length, PropsFor(symbolDisplayPart, Symbol), new TextSpan(), Symbol , symbolDisplayPart);
                col.Add(c);
            }
            
            var i = 0;
            chars.Clear();
            foreach (var textRun in col)
            {
                Length += textRun.Length;
                chars.AddRange(Enumerable.Repeat(i, textRun.Length));
                i++;
            }
        }

        public override TextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ISymbol symbol)
        {
            List<Brush> brushes = new List<Brush>();

            brushes.Add(Brushes.Pink);
            brushes.Add(Brushes.Aqua);
            brushes.Add(Brushes.Blue);
            brushes.Add(Brushes.BlueViolet);
            brushes.Add(Brushes.Crimson);
            brushes.Add(Brushes.Chartreuse);
            brushes.Add(Brushes.DarkOrchid);
            brushes.Add(Brushes.DarkOrange);
            brushes.Add(Brushes.CornflowerBlue);
            brushes.Add(Brushes.DarkMagenta);
            brushes.Add(Brushes.DarkSalmon);
            brushes.Add(Brushes.DodgerBlue);
            brushes.Add(Brushes.Lime);
            brushes.Add(Brushes.Goldenrod);
            brushes.Add(Brushes.DeepSkyBlue);
            brushes.Add(Brushes.MediumSeaGreen);
            brushes.Add(Brushes.IndianRed);
            brushes.Add(Brushes.OliveDrab);
            brushes.Add(Brushes.Tomato);
            brushes.Add(Brushes.Peru);
            brushes.Add(Brushes.MediumPurple);

            var max = (int) SymbolDisplayPartKind.ConstantName + 1;

            var props = BasicProps();
            int flags = 0;
                var kind = (int) symbolDisplayPart.Kind;
                if(kind < brushes.Count)
                props.SetForegroundBrush(brushes[kind]);
            

            switch (symbolDisplayPart.Kind)
                {
                    case SymbolDisplayPartKind.AliasName:
                        break;
                    case SymbolDisplayPartKind.AssemblyName:
                        break;
                    case SymbolDisplayPartKind.ClassName:
                        props.SetFontStyle(FontStyles.Italic);
                        break;
                    case SymbolDisplayPartKind.DelegateName:
                        break;
                    case SymbolDisplayPartKind.EnumName:
                        break;
                    case SymbolDisplayPartKind.ErrorTypeName:
                        break;
                    case SymbolDisplayPartKind.EventName:
                        break;
                    case SymbolDisplayPartKind.FieldName:
                        break;
                    case SymbolDisplayPartKind.InterfaceName:
                        break;
                    case SymbolDisplayPartKind.Keyword:
                        break;
                    case SymbolDisplayPartKind.LabelName:
                        break;
                    case SymbolDisplayPartKind.LineBreak:
                        break;
                    case SymbolDisplayPartKind.NumericLiteral:
                        break;
                    case SymbolDisplayPartKind.StringLiteral:
                        break;
                    case SymbolDisplayPartKind.LocalName:
                        break;
                    case SymbolDisplayPartKind.MethodName:
                        break;
                    case SymbolDisplayPartKind.ModuleName:
                        break;
                    case SymbolDisplayPartKind.NamespaceName:
                        break;
                    case SymbolDisplayPartKind.Operator:
                        break;
                    case SymbolDisplayPartKind.ParameterName:
                        break;
                    case SymbolDisplayPartKind.PropertyName:
                        break;
                    case SymbolDisplayPartKind.Punctuation:
                        break;
                    case SymbolDisplayPartKind.Space:
                        break;
                    case SymbolDisplayPartKind.StructName:
                        break;
                    case SymbolDisplayPartKind.AnonymousTypeIndicator:
                        break;
                    case SymbolDisplayPartKind.Text:
                        break;
                    case SymbolDisplayPartKind.TypeParameterName:
                        break;
                    case SymbolDisplayPartKind.RangeVariableName:
                        break;
                    case SymbolDisplayPartKind.EnumMemberName:
                        break;
                    case SymbolDisplayPartKind.ExtensionMethodName:
                        break;
                    case SymbolDisplayPartKind.ConstantName:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return props;
            }

        public override void TextInput(int InsertionPoint, string text)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        public SymbolDisplayFormat SymbolDisplayFormat { get; set; } = SymbolDisplayFormat.MinimallyQualifiedFormat;

        private void TakeTextRun(TextRun obj)
        {
            
            if (obj is CustomTextCharacters c)
            {
                if (c.Index.HasValue == false) throw new InvalidOperationException("no index");
                _lineBuilder.Append(c.Text);
                var cIndex = c.Index.Value;
                if (currentLineIndex + cIndex > _lineBuilder.Length)
                {
                    var x = currentLineIndex + cIndex - _lineBuilder.Length;
                    _lineBuilder.Append(new string(' ', x));
                }
            }
            else if (obj is TextEndOfLine eol)
            {
                _lines.Add(_lineBuilder.ToString());
            }

            col.Add(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxTree Tree { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public TextRunProperties PropsFor(SyntaxToken token, string text)
        {
            var pp = BasicProps();
            var kind = token.Kind();

            if (token.Parent != null)
            {
                var syntaxKind = token.Parent.Kind();

                DebugUtils.WriteLine(syntaxKind.ToString());
                if(SyntaxFacts.IsName(syntaxKind))
                {
                    pp.SetForegroundBrush(Brushes.Pink);
                } else 
                if (SyntaxFacts.IsTypeSyntax(syntaxKind))
                {
                    pp.SetForegroundBrush(Brushes.Crimson);
                }

                if (syntaxKind == SyntaxKind.MethodDeclaration)
                {
                    if (SyntaxFacts.IsAccessibilityModifier(kind))
                    {
                        pp.SetForegroundBrush(Brushes.Aqua);
                    }
                }
            }

            // pp.SyntaxToken = trivia;
            // pp.Text = text;
            return pp;
        }

        #endregion

        #region Private Fields

        private int lemgth;
        private Type _type;
        private static double _pixelsPerDip;
        private readonly List<int> chars = new List<int>();
        private readonly List<TextRun> col = new List<TextRun>();

        public FontFamily Family { get; set; } = new FontFamily("GlobalMonospace.CompositeFont");

        private readonly FontWeight _fontWeight = FontWeights.Medium;
        private readonly FontStyle _fontStyle = FontStyles.Normal;
        private readonly FontStretch _fontStretch = FontStretches.Normal;
        public double EmSize { get; set; } = 24;
        private IList colx = new ArrayList();
        private List<int> chars2 = new List<int>();
        private SyntaxNode _node;
        private readonly Typeface _typeface;
        private readonly FontRendering _fontRendering;

        private readonly StringBuilder _lineBuilder = new StringBuilder();
        private readonly int currentLineIndex = 0;
        private readonly List<string> _lines = new List<string>();
        public List<TextRun> ErrorRuns { get; } = new List<TextRun>();
        public ISymbol Symbol { get; set; }

        #endregion

        public override void Init()
        {
            GenerateText();
        }
    }
}