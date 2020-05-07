﻿using System;
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
    public class CustomTextSource3 : TextSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelsPerDip"></param>
        public CustomTextSource3(double pixelsPerDip)
        {
            PixelsPerDip = pixelsPerDip;
            _typeface = new Typeface(Family, _fontStyle, _fontWeight,
                _fontStretch);
            _fontRendering = new FontRendering(EmSize, TextAlignment.Left, new TextDecorationCollection(), Brushes.Blue,
                _typeface);
            BaseProps = new GenericTextRunProperties(
                _fontRendering,
                PixelsPerDip);
        }

        /// <summary>
        /// 
        /// </summary>
        public CSharpCompilation Compilation { get; set; }

        public int Length { get; set; }

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

        public TextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ITypeSymbol tt)
        {
            var xx = BasicProps();
            // xx.SymbolDisplaYPart = trivia;
            // xx.TypeSymbol = tt;
            return xx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MyTextRunProperties BasicProps()
        {
            var xx = new MyTextRunProperties(BaseProps);
            return xx;
        }

        private static TypeSyntax M(Type arg)
        {
            var t = DevTypeControl.MyTypeName(arg);
            var identifierNameSyntax = SyntaxFactory.ParseTypeName(t);
            if (arg.IsGenericType)
                return SyntaxFactory.GenericName(SyntaxFactory.Identifier(arg.Name),
                    SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList<TypeSyntax>()
                        .AddRange(arg.GenericTypeArguments.Select(M))));
            return identifierNameSyntax;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trivia"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public TextRunProperties PropsFor(in SyntaxTrivia trivia, string text)
        {
            var r = BasicProps();
            DebugUtils.WriteLine($"{trivia.Kind()}");
            if(trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
            {
                r.SetForegroundBrush(Brushes.YellowGreen);
            }
            // r.SyntaxTrivia = trivia;
            // r.Text = text;
            return r;
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
            var f = new SyntaxWalkerF(col, this, TakeTextRun, MakeProperties);
            
            f.DefaultVisit(Node);
            var i = 0;
            chars.Clear();
            //var model =  Compilation?.GetSemanticModel(Tree);
            foreach (var textRun in col)
            {
                Length += textRun.Length;
                chars.AddRange(Enumerable.Repeat(i, textRun.Length));
                i++;

                if (textRun.Properties is GenericTextRunProperties gp)
                {
                    //DebugUtils.WriteLine(gp.SyntaxToken.ToString());
                }
            }
        }

        private TextRunProperties MakeProperties(object arg, string text)
        {
            if (arg is SyntaxTrivia st)
                return PropsFor(st, text);
            if (arg is SyntaxToken t) return PropsFor(t, text);

            return null;
        }

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
        public GenericTextRunProperties BaseProps { get; }

        public List<CompilationError> Errors
        {
            get { return _errors1; }
            set
            {
                _errors1 = value;
                if (_errors1 != null && _errors1.Any())
                    foreach (var compilationError in _errors1)
                        ErrorRuns.Add(new CustomTextCharacters(compilationError.Message, BaseProps, new TextSpan()));
            }
        }

        private readonly StringBuilder _lineBuilder = new StringBuilder();
        private readonly int currentLineIndex = 0;
        private readonly List<string> _lines = new List<string>();
        public List<TextRun> ErrorRuns { get; } = new List<TextRun>();
        private List<CompilationError> _errors1;
        public int EolLength { get; } = 2;

        #endregion

        public void Init()
        {
            GenerateText();
        }
    }
}