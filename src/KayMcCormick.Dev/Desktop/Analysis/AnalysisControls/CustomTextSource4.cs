using AnalysisAppLib.Properties;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis.CSharp;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;
using SyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using SyntaxFacts = Microsoft.CodeAnalysis.CSharp.SyntaxFacts;
using SyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomTextSource4 : AppTextSource, ICustomTextSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelsPerDip"></param>
        /// <param name="typefaceManager"></param>
        public CustomTextSource4(double pixelsPerDip, ITypefaceManager typefaceManager)
        {
            PixelsPerDip = pixelsPerDip;
            _typeface = typefaceManager.GetDefaultTypeface();

            Rendering = typefaceManager.GetRendering(EmSize, TextAlignment.Left, new TextDecorationCollection(),
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
            void TakeToken()
            {
                var includeDocumentationComments = true;
                var includeDirectives = true;
                var includeSkipped = true;
                var includeZeroWidth = true;
                this.token = this.token.HasValue
                    ? this.token.Value.GetNextToken(includeZeroWidth, includeSkipped, includeDirectives,
                        includeDocumentationComments)
                    : Node?.GetFirstToken(includeZeroWidth, includeSkipped, includeDirectives,
                        includeDocumentationComments);

                if (this.token.HasValue)
                    _starts.Push(Tuple.Create(this.token.Value.Span, this.token.Value));
            }

            if (textSourceCharacterIndex == 0)
            {
                if (Length == 0)
                {
                    return new TextEndOfParagraph(2);
                }
                this.token = null;
                _starts.Clear();
            }

            var childInPos = Node.ChildThatContainsPosition(textSourceCharacterIndex);
            if (childInPos.IsNode)
            {
                var n = childInPos.AsNode();
                if (textSourceCharacterIndex < n.SpanStart)
                {
                    foreach (var syntaxTrivia in n.GetLeadingTrivia())
                    {
                        if (textSourceCharacterIndex >= syntaxTrivia.SpanStart &&
                            textSourceCharacterIndex <= syntaxTrivia.Span.End)
                        {
                            DebugUtils.WriteLine("In trivia " + syntaxTrivia);
                            if (textSourceCharacterIndex > syntaxTrivia.SpanStart)
                            {
                                DebugUtils.WriteLine("In middle of trivia");
                            }

                            var characterString = syntaxTrivia.ToFullString();
                            return new SyntaxTriviaTextCharacters(characterString,
                                PropsFor(syntaxTrivia, characterString), syntaxTrivia.FullSpan, syntaxTrivia);
                        }
                    }
                }
                
            }

            if (this.token.HasValue && CSharpExtensions.Kind(this.token.Value) == SyntaxKind.None)
                return new TextEndOfParagraph(2);
            var token1 = this.token;
            // DebugUtils.WriteLine("Index = " + textSourceCharacterIndex);
            if (!token1.HasValue)
            {
                TakeToken();
                if (!this.token.HasValue) return new TextEndOfParagraph(2);
                token1 = this.token;
            }

            var token = token1.Value;
            var k = _starts.Peek().Item1;

            if (textSourceCharacterIndex < k.Start)
            {
                var len = k.Start - textSourceCharacterIndex;
                var buf = new char[len];
                _text.CopyTo(textSourceCharacterIndex, buf, 0, len);
                if (len == 2 && buf[0] == '\r' && buf[1] == '\n') return new CustomTextEndOfLine(2);

                var t = string.Join("", buf);
                return new CustomTextCharacters(t, MakeProperties(SyntaxKind.None, t));
            }
            else if (textSourceCharacterIndex >= k.End)
            {
                TakeToken();
                return GetTextRun(textSourceCharacterIndex);
            }
            else
            {
                if (CSharpExtensions.Kind(token) == SyntaxKind.EndOfLineTrivia) return new CustomTextEndOfLine(2);
                var len = k.Length;
                if (len == 0)
                {
                    TakeToken();
                    return GetTextRun(textSourceCharacterIndex);
                }

                TakeToken();
                if (token.Text.Length != len)
                {
                }

                return new CustomTextCharacters(token.Text, MakeProperties(token, token.Text));
            }

            DebugUtils.WriteLine($"{nameof(GetTextRun)}: {textSourceCharacterIndex}", DebugCategory.Status);
            // Make sure text source index is in bounds.
            if (textSourceCharacterIndex < 0)
            {
                DebugUtils.WriteLine("out of bounds", DebugCategory.TextFormatting);
                throw new ArgumentOutOfRangeException(nameof(textSourceCharacterIndex),
                    Resources.CustomTextSource3_GetTextRun_Value_must_be_greater_than_0_);
            }

            if (textSourceCharacterIndex >= Length)
                //DebugUtils.WriteLine(Resources.CustomTextSource3_GetTextRun_past_text_source_length, DebugCategory.TextFormatting);
                return new TextEndOfParagraph(2);


            // Create TextCharacters using the current font rendering properties.
            if (textSourceCharacterIndex < Length)
            {
                var xx = chars[textSourceCharacterIndex];
                // var xx2 = chars2[textSourceCharacterIndex];
                if (col[xx] is CustomTextCharacters tc)
                {
                    var z = textSourceCharacterIndex - tc.Index;
                    if (z > 0)
                        throw new InvalidOperationException("request not aligned on text run boundary:" +
                                                            textSourceCharacterIndex);
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
#if false
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
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public TextRunProperties MakeProperties(object arg, string text)
        {
            TextRunProperties textRunProperties = null;
            if (arg == (object) SyntaxKind.None)
                textRunProperties = PropsFor(text);
            else if (arg is SyntaxTrivia st)
                textRunProperties = PropsFor(st, text);
            else if (arg is SyntaxToken t)
                textRunProperties = PropsFor(t, text);
            else
                textRunProperties = PropsFor(text);
            if (textRunProperties != null)
            {
                if (textRunProperties is BasicTextRunProperties b)
                    if (!b.HasCustomization)
                    {
                        // b.SetBackgroundBrush(Brushes.LightBlue);
                        var kind = "";

                        var nodeKind = "";
                        var tkind = "";
                        if (arg is SyntaxToken stk)
                        {
                            var syntaxKind = CSharpExtensions.Kind(stk.Parent);
                            nodeKind = syntaxKind.ToString();
                            kind = CSharpExtensions.Kind(stk).ToString();
                            if (SyntaxFacts.IsTrivia(syntaxKind))
                            {
                                var pt = stk.Parent.ParentTrivia;
                                tkind = CSharpExtensions.Kind(pt).ToString();
                            }
                        }

                        DebugUtils.WriteLine(
                            $"no customizations for {arg} - {text} {arg.GetType().Name} {kind} {nodeKind} [{tkind}]");
                    }

                return textRunProperties;
            }

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
        public SyntaxTree Tree
        {
            get { return _tree; }
            set
            {
                _tree = value;
                if (_tree != null)
                {
                    _text = _tree.GetText();
                    Length = _text.Length;
                }
            }
        }

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

            if (token.ContainsDiagnostics)
            {
                var sevB = new System.Windows.Media.Brush[4] {null, Brushes.LightBlue, Brushes.BlueViolet, Brushes.Red};
                var s = token.GetDiagnostics().Max(diagnostic => diagnostic.Severity);
                pp.SetBackgroundBrush(sevB[(int) s]);
            }
            if (token.Parent != null)
            {
                if (token.Parent.ContainsDiagnostics)
                {
                    var sevB = new System.Windows.Media.Brush[4] { null, Brushes.LightBlue, Brushes.BlueViolet, Brushes.Red };
                    var s = token.Parent.GetDiagnostics().Max(diagnostic => diagnostic.Severity);
                    pp.SetBackgroundBrush(sevB[(int)s]);
                }
                var syntaxKind = CSharpExtensions.Kind(token.Parent);
                var tkind = "";
                var zz = token.Parent.FirstAncestorOrSelf<SyntaxNode>(
                    z => SyntaxFacts.IsTrivia(CSharpExtensions.Kind((SyntaxNode) z)), false);

                if (zz != null)
                {
                    var pt = zz.ParentTrivia;
                    var syntaxKind1 = CSharpExtensions.Kind(pt);
                    tkind = syntaxKind1.ToString();

                    switch (syntaxKind1)
                    {
                        case SyntaxKind.EndOfLineTrivia:
                            break;
                        case SyntaxKind.WhitespaceTrivia:
                            break;
                        case SyntaxKind.SingleLineCommentTrivia:
                            pp.SetForegroundBrush(Brushes.LightGray);
                            break;
                        case SyntaxKind.MultiLineCommentTrivia:
                            pp.SetForegroundBrush(Brushes.LightGray);
                            break;
                        case SyntaxKind.DocumentationCommentExteriorTrivia:
                            pp.SetBackgroundBrush(Brushes.Aqua);
                            break;
                        case SyntaxKind.SingleLineDocumentationCommentTrivia:
                            pp.SetForegroundBrush(Brushes.LightGray);
                            break;
                        case SyntaxKind.MultiLineDocumentationCommentTrivia:
                            pp.SetForegroundBrush(Brushes.LightGray);
                            break;
                        case SyntaxKind.DisabledTextTrivia:
                            pp.SetForegroundBrush(Brushes.LightGray);
                            break;
                        case SyntaxKind.PreprocessingMessageTrivia:
                            break;
                        case SyntaxKind.IfDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.ElifDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.ElseDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.EndIfDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.RegionDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.EndRegionDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.DefineDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.UndefDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.ErrorDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.WarningDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.LineDirectiveTrivia:
                            pp.SetForegroundBrush(Brushes.BurlyWood);
                            break;
                        case SyntaxKind.PragmaWarningDirectiveTrivia:
                            break;
                        case SyntaxKind.PragmaChecksumDirectiveTrivia:
                            break;
                        case SyntaxKind.ReferenceDirectiveTrivia:
                            break;
                        case SyntaxKind.BadDirectiveTrivia:
                            break;
                        case SyntaxKind.SkippedTokensTrivia:
                            break;
                        case SyntaxKind.ConflictMarkerTrivia:
                            break;
                        case SyntaxKind.NullableDirectiveTrivia:
                            break;
                    }
                }

                if (SyntaxFacts.IsPredefinedType(kind))
                {
                    pp.SetForegroundBrush(Brushes.Gold);
                }
                else if (SyntaxFacts.IsKeywordKind(kind))
                {
                    pp.SetForegroundBrush(Brushes.CornflowerBlue);
                    return pp;
                }
                else if (SyntaxFacts.IsLiteralExpression(kind))
                {
                    pp.SetForegroundBrush(Brushes.Brown);
                    pp.SetFontStyle(FontStyles.Italic);
                }

                DebugUtils.WriteLine(syntaxKind.ToString(), DebugCategory.TextFormatting);
                // if (SyntaxFacts.IsName(syntaxKind))
                // pp.SetForegroundBrush(Brushes.Pink);
                // if (SyntaxFacts.IsTypeSyntax(syntaxKind)) pp.SetForegroundBrush(Brushes.Crimson);

                if (syntaxKind == SyntaxKind.MethodDeclaration)
                {
                    if (SyntaxFacts.IsAccessibilityModifier(kind))
                        pp.SetForegroundBrush(Brushes.Aqua);
                    else if (SyntaxFacts.IsKeywordKind(kind))
                        pp.SetFontStyle(FontStyles.Italic);
                }
            }

            // pp.SyntaxToken = trivia;
            // pp.Text = text;

            return pp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        // ReSharper disable once UnusedParameter.Local
        private TextRunProperties PropsFor(string text)
        {
            var pp = BasicProps();

            if (text.Trim().Length == 0)
            {
                return pp;
            }

            pp.SetForegroundBrush(Brushes.Fuchsia);
            pp.SetBackgroundBrush(Brushes.Black);
            return pp;
        }

#endregion

#region Private Fields

private Type _type;
private readonly List<int> chars = new List<int>();
        private readonly List<TextRun> col = new List<TextRun>();

        public FontFamily Family { get; set; } = new FontFamily("GlobalMonospace.CompositeFont");

        public double EmSize { get; set; } = 24;
        private IList colx = new ArrayList();
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
        private SyntaxTree _tree;
        private SourceText _text;
        private Stack<Tuple<TextSpan, SyntaxToken>> _starts = new Stack<Tuple<TextSpan, SyntaxToken>>();
        private SyntaxToken? token;
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
            //GenerateText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insertionPoint"></param>
        /// <param name="text"></param>
        public override void TextInput(int insertionPoint, string text)
        {
            DebugUtils.WriteLine($"Insertion point is {insertionPoint}.");
            DebugUtils.WriteLine($"Input text is \"{text}\"");
            TextChange change = new TextChange(new TextSpan(insertionPoint, 0), text);
            var newText = _text.WithChanges(change);
            if(newText.Length != _text.Length + text.Length)
            {
                DebugUtils.WriteLine($"Unexpected length");
            }
            var newTree = Tree.WithChangedText(newText);
            Compilation = CSharpCompilation.Create("edit", new[]{newTree}, new[]{MetadataReference.CreateFromFile(typeof(object).Assembly.Location)});
            foreach (var diagnostic in Compilation.GetParseDiagnostics())
            {
                DebugUtils.WriteLine(diagnostic.ToString());
            }

            Model = Compilation.GetSemanticModel(newTree);
            var chL = newTree.GetChangedSpans(Tree);
            foreach (var textSpan in chL)
            {
                DebugUtils.WriteLine("Changed region " + textSpan);
            }
            Tree = newTree;
            Node = newTree.GetRoot();
            
            return;


            var t = Node.GetFirstToken(true, true, true, true);
            _starts.Push(new Tuple<TextSpan, SyntaxToken>(t.Span, t));
            var q = _starts.Where(z => z.Item1.End >= insertionPoint || z.Item1.Start >= insertionPoint);
            var syntaxToken = q.First().Item2;
            var p = syntaxToken.Parent;
            DebugUtils.WriteLine(p.ToString());
            var syntaxToken1 = SyntaxFactory.ParseToken(text);
            DebugUtils.WriteLine(syntaxToken1.ToString());
            DebugUtils.WriteLine(CSharpExtensions.Kind(syntaxToken1).ToString());
            var syntaxTokens = new[] {syntaxToken1};
            try
            {
                SyntaxNode? n;
                if (syntaxToken.Span.End <= insertionPoint)
                {
                    n = p.InsertTokensAfter(syntaxToken, syntaxTokens);
                }
                else
                {
                    n = p.InsertTokensBefore(syntaxToken, syntaxTokens);
                }


                Node = Node.ReplaceNode(p, n);
            }
            catch (Exception ex)
            {
                var tr = SyntaxFactory.ParseSyntaxTree(syntaxToken1.Text);
                Tree = tr;
                Node = tr.GetRoot();
                _starts.Clear();
            }
            // var newNode = Node.ReplaceNode(p, n);
            // Node = newNode;

            var t2 = Node.GetFirstToken(true, true, true, true);

            // DebugUtils.WriteLine($"{t2.Text} [{t2.Span}]");
            _starts.Push(new Tuple<TextSpan, SyntaxToken>(t2.Span, t2));

            //
            // Node.Repl
            // if (chars.Count > InsertionPoint)
            // {
            //     var xx = chars[InsertionPoint];
            //     var x = col[xx];
            //     if (x is CustomTextCharacters ch)
            //     {
            //         var prev = ch.Text.Substring(0, InsertionPoint - ch.Index.Value);
            //         var next = ch.Text.Substring(ch.Index.Value + ch.Length - InsertionPoint);
            //         var t = prev + text + next;
            //         Length += text.Length;
            //         var customTextCharacters = new CustomTextCharacters(t, BaseProps, new TextSpan());
            //         if (ch.PrevTextRun is CustomTextCharacters cc0) cc0.NextTextRun = customTextCharacters;
            //
            //         ch.PrevTextRun = null;
            //         ch.NextTextRun = null;
            //         ch.Invalid = true;
            //         customTextCharacters.PrevTextRun = ch.PrevTextRun;
            //         customTextCharacters.Index = ch.Index;
            //         col[xx] = customTextCharacters;
            //
            //         UpdateCharMap();
            //     }
            // }
            // else
            // {
            //     var customTextCharacters =
            //         new CustomTextCharacters(text, BaseProps, new TextSpan()) {Index = InsertionPoint};
            //     // customTextCharacters.PrevTextRun = ch.PrevTextRun;
            //     col.Add(customTextCharacters);
            //
            //     UpdateCharMap();
            // }
        }

        public SemanticModel Model { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateCharMap()
        {
#if false
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
#endif
        }

        public int EnterLineBreak(int insertionPoint)
        {
            TextInput(insertionPoint, "\r\n");
            return insertionPoint + 2;
        }
    }
}