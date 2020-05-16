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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;
using SyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using SyntaxFacts = Microsoft.CodeAnalysis.CSharp.SyntaxFacts;
using SyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;

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
        Compilation Compilation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int Length { get; }

        /// <summary>
        /// 
        /// </summary>
        FontRendering FontRendering { get; set; }

        /// <summary>
        /// 
        /// </summary>
        SyntaxNode Node { get; set; }

        /// <summary>
        /// 
        /// </summary>
        SyntaxTree Tree { get; set; }

        FontFamily Family { get; set; }
        double EmSize { get; set; }
        GenericTextRunProperties BaseProps { get; }
        List<CompilationError> Errors { get; set; }
        List<TextRun> ErrorRuns { get; }
        int EolLength { get; }

        /// <summary>
        /// 
        /// </summary>
        FontRendering Rendering { get; set; }

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

        TextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ISymbol symbol);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trivia"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        TextRunProperties PropsFor(in SyntaxTrivia trivia, string text);

        void GenerateText();
        TextRunProperties MakeProperties(object arg, string text);
        void TakeTextRun(TextRun obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        TextRunProperties PropsFor(SyntaxToken token, string text);

        void Init();
        void UpdateCharMap();
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomTextSource3 : AppTextSource, ICustomTextSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelsPerDip"></param>
        /// <param name="typefaceManager"></param>
        public CustomTextSource3(double pixelsPerDip, ITypefaceManager typefaceManager)
        {
            PixelsPerDip = pixelsPerDip;
            _typeface = typefaceManager.GetDefaultTypeface();

            Rendering = typefaceManager.GetRendering(EmSize, TextAlignment.Left, new TextDecorationCollection(), Brushes.Black,
                _typeface);
            BaseProps = TextPropertiesManager.GetBasicTextRunProperties(PixelsPerDip, Rendering);
        }

        /// <summary>
        /// 
        /// </summary>
        public Compilation Compilation { get; set; }

        /// <summary>
        /// 
        /// </summary>
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
            if (textSourceCharacterIndex >= Length) return new TextEndOfParagraph(2);

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

        public override TextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ISymbol symbol)
        {
            throw new NotImplementedException();
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
            DebugUtils.WriteLine($"{CSharpExtensions.Kind(trivia)}");
            if(CSharpExtensions.Kind(trivia) == SyntaxKind.SingleLineCommentTrivia)
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

        public void GenerateText()
        {
            if (this.Tree is VisualBasicSyntaxTree tree1)
            {
                var f1 = new SyntaxTalkVb(col, this, TakeTextRun, MakeProperties);
                if (Node != null) f1.DefaultVisit(Node);
            }
            else
            {
                var f = new SyntaxWalkerF(col, this, TakeTextRun, MakeProperties);
                if (Node != null) f.DefaultVisit(Node);
            }

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

        public TextRunProperties MakeProperties(object arg, string text)
        {
            if (arg is SyntaxTrivia st)
                return PropsFor(st, text);
            if (arg is SyntaxToken t) return PropsFor(t, text);

            return null;
        }

        public void TakeTextRun(TextRun obj)
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
            var kind = CSharpExtensions.Kind(token);

            if (token.Parent != null)
            {
                var syntaxKind = CSharpExtensions.Kind(token.Parent);

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
        public override GenericTextRunProperties BaseProps { get; }

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

        /// <summary>
        /// 
        /// </summary>
        public FontRendering Rendering { get; set; }

        #endregion

        public override void Init()
        {
            GenerateText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InsertionPoint"></param>
        /// <param name="text"></param>
        public override void TextInput(int InsertionPoint, string text)
        {
            if (chars.Count > InsertionPoint)
            {
                var xx = chars[InsertionPoint];
                var x = col[xx];
                if (x is CustomTextCharacters ch)
                {
                    var prev = ch.Text.Substring(0, InsertionPoint - ch.Index.Value);
                    var next = ch.Text.Substring((ch.Index.Value + ch.Length) - InsertionPoint);
                    var t = prev + text + next;
                    Length += text.Length;
                    var customTextCharacters = new CustomTextCharacters(t, BaseProps, new TextSpan());
                    if (ch.PrevTextRun is CustomTextCharacters cc0)
                    {
                        cc0.NextTextRun = customTextCharacters;
                    }

                    ch.PrevTextRun = null;
                    ch.NextTextRun = null;
                    ch.Invalid = true;
                    customTextCharacters.PrevTextRun = ch.PrevTextRun;
                    customTextCharacters.Index = ch.Index;
                    col[xx] = customTextCharacters;

                    UpdateCharMap();
                }
            }
            else
            {
                var customTextCharacters =
                    new CustomTextCharacters(text, BaseProps, new TextSpan()) {Index = InsertionPoint};
                // customTextCharacters.PrevTextRun = ch.PrevTextRun;
                col.Add(customTextCharacters);

                UpdateCharMap();
            }
        }

        public void UpdateCharMap()
        {
            var i = 0;
            chars.Clear();
            //var model =  Compilation?.GetSemanticModel(Tree);
            foreach (var textRun in col)
            {
                Length += textRun.Length;
                chars.AddRange(Enumerable.Repeat(i, textRun.Length));
                i++;

                if (textRun.Properties is GenericTextRunProperties)
                {
                    //DebugUtils.WriteLine(gp.SyntaxToken.ToString());
                }
            }

        }
    }
}