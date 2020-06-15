using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using AnalysisAppLib.Properties;
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

            Rendering = typefaceManager.
                GetRendering(EmSize, TextAlignment.Left, new TextDecorationCollection(),
                Brushes.Black,
                _typeface);
            BaseProps = TextPropertiesManager.GetBasicTextRunProperties(
                PixelsPerDip, Rendering);
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
            DebugUtils.WriteLine($"{nameof(GetTextRun)}: {textSourceCharacterIndex}", DebugCategory.Status);
            // Make sure text source index is in bounds.
            if (textSourceCharacterIndex < 0)
            {
                DebugUtils.WriteLine("out of bounds", DebugCategory.TextFormatting);
                throw new ArgumentOutOfRangeException(nameof(textSourceCharacterIndex),
                    Resources.CustomTextSource3_GetTextRun_Value_must_be_greater_than_0_);
            }

            if (textSourceCharacterIndex >= Length)
            {
                //DebugUtils.WriteLine(Resources.CustomTextSource3_GetTextRun_past_text_source_length, DebugCategory.TextFormatting);
                return new TextEndOfParagraph(2);
            }


            // Create TextCharacters using the current font rendering properties.
            if (textSourceCharacterIndex < Length)
            {
                var xx = chars[textSourceCharacterIndex];
                // var xx2 = chars2[textSourceCharacterIndex];
                if (col[xx] is CustomTextCharacters tc)
                {
                    var z = textSourceCharacterIndex - tc.Index;
                    if (z > 0)
                    {
                        throw new InvalidOperationException("request not aligned on text run boundary:" +
                                                            textSourceCharacterIndex);
                    }
                    var zz = tc.Length - z;

                    var tf = tc.Properties?.Typeface;
                    var name = tf?.FaceNames[XmlLanguage.GetLanguage("en-US")];
//                    DebugUtils.WriteLine($"Typeface: {name}", DebugCategory.TextFormatting);
//                    DebugUtils.WriteLine("Typeface FontFamily: " + tf?.FontFamily, DebugCategory.TextFormatting);
                    return tc;
                }

                DebugUtils.WriteLine($"returning {col[xx]}");
            
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override BasicTextRunProperties BasicProps()
        {
            var xx = new BasicTextRunProperties(BaseProps);
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
        // ReSharper disable once UnusedParameter.Local
        private TextRunProperties PropsFor(in SyntaxTrivia trivia, string text)
        {
            var r = BasicProps();
            DebugUtils.WriteLine($"{CSharpExtensions.Kind(trivia)}", DebugCategory.TextFormatting);
            if (CSharpExtensions.Kind(trivia) == SyntaxKind.SingleLineCommentTrivia)
                r.SetForegroundBrush(Brushes.YellowGreen);
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
            set { _node = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GenerateText()
        {
            if (Tree is VisualBasicSyntaxTree)
            {
                var f1 = new SyntaxTalkVb(col, this, TakeTextRun, MakeProperties);
                if (Node != null) f1.DefaultVisit(Node);
            }
            else
            {
                var f = new SyntaxWalkerF(col, this, TakeTextRun, MakeProperties);
                var token = Node.GetFirstToken();
                DebugUtils.WriteLine(token.Span.ToString());
                var t2 = token.GetNextToken();
                DebugUtils.WriteLine(t2.Span.ToString());
                f.DefaultVisit(Node ?? Tree.GetRoot());
            }

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
                    //DebugUtils.WriteLine(gp.SyntaxToken.ToString(), DebugCategory.TextFormatting);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public TextRunProperties MakeProperties(object arg, string text)
        {
            if (arg is SyntaxTrivia st)
                return PropsFor(st, text);
            if (arg is SyntaxToken t) return PropsFor(t, text);

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="AppInvalidOperationException"></exception>
        public void TakeTextRun(TextRun obj)
        {
            if (obj is CustomTextCharacters c)
            {
                if (c.Index.HasValue == false) throw new AppInvalidOperationException("no index");
                _lineBuilder.Append(c.Text);
                var cIndex = c.Index.Value;
                if (currentLineIndex + cIndex > _lineBuilder.Length)
                {
                    var x = currentLineIndex + cIndex - _lineBuilder.Length;
                    _lineBuilder.Append(new string(' ', x));
                }
            }
            else if (obj is TextEndOfLine)
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
        // ReSharper disable once UnusedParameter.Local
        private TextRunProperties PropsFor(SyntaxToken token, string text)
        {
            var pp = BasicProps();
            var kind = CSharpExtensions.Kind(token);

            if (token.Parent != null)
            {
                var syntaxKind = CSharpExtensions.Kind(token.Parent);

                DebugUtils.WriteLine(syntaxKind.ToString(), DebugCategory.TextFormatting);
                if (SyntaxFacts.IsName(syntaxKind))
                    pp.SetForegroundBrush(Brushes.Pink);
                else if (SyntaxFacts.IsTypeSyntax(syntaxKind)) pp.SetForegroundBrush(Brushes.Crimson);

                if (syntaxKind == SyntaxKind.MethodDeclaration)
                    if (SyntaxFacts.IsAccessibilityModifier(kind))
                        pp.SetForegroundBrush(Brushes.Aqua);
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
        private FontRendering Rendering { get; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
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
                    var next = ch.Text.Substring(ch.Index.Value + ch.Length - InsertionPoint);
                    var t = prev + text + next;
                    Length += text.Length;
                    var customTextCharacters = new CustomTextCharacters(t, BaseProps, new TextSpan());
                    if (ch.PrevTextRun is CustomTextCharacters cc0) cc0.NextTextRun = customTextCharacters;

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

        /// <summary>
        /// 
        /// </summary>
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
                    //DebugUtils.WriteLine(gp.SyntaxToken.ToString(), DebugCategory.TextFormatting);
                }
            }
        }
    }
}