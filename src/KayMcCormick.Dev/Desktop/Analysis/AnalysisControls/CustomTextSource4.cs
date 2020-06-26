﻿using AnalysisAppLib.Properties;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
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
    public class CustomTextSource4 : AppTextSource, ICustomTextSource,INotifyPropertyChanged
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
            _prev = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public Compilation Compilation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override int Length { get; protected set; }

        IEnumerable<SyntaxInfo> GetSyntaxInfos()
        {
            if (Node == null)
                yield break;
            var token1 = Node.GetFirstToken(true, true, true, true);
            while (CSharpExtensions.Kind(token1) != SyntaxKind.None)
            {
                if (token1.HasLeadingTrivia)
                {
                    foreach (var syntaxTrivia in token1.LeadingTrivia)
                    {
                        yield return new SyntaxInfo(syntaxTrivia);
                    }
                }

                if (CSharpExtensions.Kind(token1)== SyntaxKind.EndOfFileToken)
                    yield break;
                yield return new SyntaxInfo(token1);
                if (token1.HasTrailingTrivia)
                {
                    foreach (var syntaxTrivia in token1.TrailingTrivia)
                    {
                        yield return new SyntaxInfo(syntaxTrivia);
                    }
                }

                token1 = token1.GetNextToken(true, true, true, true);
            }

            yield break;
        }
        // Used by the TextFormatter object to retrieve a run of text from the text source.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textSourceCharacterIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            DebugUtils.WriteLine($"GetTextRun(textSourceCharacterIndex = {textSourceCharacterIndex})");
            if (!SyntaxInfos.MoveNext())
            {
                return new TextEndOfParagraph(2);
            }

            var si = SyntaxInfos.Current;
            while (si.Span1.End <= textSourceCharacterIndex || si.Text.Length == 0)
            {
                if (!SyntaxInfos.MoveNext())
                {
                    return new TextEndOfParagraph(2);
                }
                _prev = si;
                si = SyntaxInfos.Current;
            }

            if (si.Span1.Start != textSourceCharacterIndex)
            {

            }

            _prev = si;
            
            if (si.SyntaxTrivia.HasValue)
            {
                if (CSharpExtensions.Kind(si.SyntaxTrivia.Value) == SyntaxKind.EndOfLineTrivia)
                {
                    return new CustomTextEndOfLine(2);
                }
                    return new SyntaxTriviaTextCharacters(si.Text, PropsFor(si.SyntaxTrivia.Value, si.Text), si.Span1, si.SyntaxTrivia.Value);
            } else if (si.SyntaxToken.HasValue)
            {
                return new SyntaxTokenTextCharacters(si.Text, si.Text.Length, PropsFor(si.SyntaxToken.Value, si.Text),
                    si.SyntaxToken.Value, si.SyntaxToken.Value.Parent);

            }

            DebugUtils.WriteLine($"index: {textSourceCharacterIndex}");
            TextSpan? TakeToken()
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
                {
                    if (!_starts.Any() && this.token.Value.SpanStart != 0)
                    {

                    }

                    StartInfo tuple = new StartInfo(this.token.Value.Span, this.token.Value);
                    _starts.Add(tuple);
                    DumpStarts();
                    return this.token.Value.Span;
                    
                }

                return null;
            }

            TextSpan? span = null;
            if (textSourceCharacterIndex == 0)
            {
                if (Length == 0)
                {
                    return new TextEndOfParagraph(2);
                }

                _curStart = 0;

                if (_starts.Any())
                {
                    var startInfo = _starts.First();
                    this.token = startInfo.Token;
                    this.trivia = startInfo.SyntaxTrivia;
                    span = startInfo.TextSpan;
                    if(this.token.HasValue) CheckToken(this.token);
                }

                // _starts.Clear();
                DumpStarts();
                
            }
            else
            {
                var startInfo = _starts[_curStart];
                this.token = startInfo.Token;
                this.trivia = startInfo.SyntaxTrivia;
                span = startInfo.TextSpan;
                if (this.token.HasValue) CheckToken(this.token);
            }

            try
            {
                var childInPos = Node.ChildThatContainsPosition(textSourceCharacterIndex);
                if (childInPos.IsNode)
                {
                    var n = childInPos.AsNode();
                    if (textSourceCharacterIndex < n.SpanStart)
                    {
                        foreach (var syntaxTrivia in n.GetLeadingTrivia())
                        {
                            if (textSourceCharacterIndex >= syntaxTrivia.SpanStart &&
                                textSourceCharacterIndex < syntaxTrivia.Span.End)
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
            }
            catch (Exception ex)
            {

            }

            
            var token1 = this.token;
            // DebugUtils.WriteLine("Index = " + textSourceCharacterIndex);
            // if (!token1.HasValue)
            // {
                // span = TakeToken();
                // if (!this.token.HasValue) return new TextEndOfParagraph(2);
                // token1 = this.token;

            // }

//            var token = token1.Value;
            if (!span.HasValue)
            {
		throw new InvalidOperationException();
            }
            var k = span.Value;

            if (textSourceCharacterIndex < k.Start)
            {
                var len = k.Start - textSourceCharacterIndex;
                var buf = new char[len];
                Text.CopyTo(textSourceCharacterIndex, buf, 0, len);
                if (len == 2 && buf[0] == '\r' && buf[1] == '\n') return new CustomTextEndOfLine(2);

                var t = string.Join("", buf);
                return new CustomTextCharacters(t, MakeProperties(SyntaxKind.None, t));
            }
            else if (textSourceCharacterIndex >= k.End && k.Length != 0)
            {
                TakeToken();
                return GetTextRun(textSourceCharacterIndex);
            }
            else
            {

                if (this.trivia.HasValue)
                {
                    var syntaxTrivia1 = this.trivia.Value;
                    var q = syntaxTrivia1.Token.LeadingTrivia
                        .SkipWhile(syntaxTrivia => syntaxTrivia != syntaxTrivia1)
                        .Skip(1);
                    if (q.Any())
                    {
                        _curStart++;
                        var startInfo = new StartInfo(q.First());
                        if (_starts.Count <= _curStart)
                        {
                            
                            _starts.Add(startInfo);
                        }
                        else
                        {
                            _starts[_curStart] = startInfo;
                        }
                    }
                    else
                    {
                        var t2 = syntaxTrivia1.Token.GetNextToken(true, true, true, true);
                        if (t2.HasLeadingTrivia)
                        {
                            var st = new StartInfo(t2.LeadingTrivia.First());
                            _curStart++;
                            if (_starts.Count <= _curStart)
                            {

                                _starts.Add(st);
                            }
                            else
                            {
                                _starts[_curStart] = st;
                            }

                        }
                        else if(CSharpExtensions.Kind(t2) != SyntaxKind.None)
                        {
                            var st = new StartInfo(t2.Span, t2);
                            _curStart++;
                            if (_starts.Count <= _curStart)
                            {

                                _starts.Add(st);
                            }
                            else
                            {
                                _starts[_curStart] = st;
                            }

                        }
                    }
                    var t=syntaxTrivia1.ToFullString();
                    return new SyntaxTriviaTextCharacters(t,PropsFor(trivia.Value,t),span.Value,syntaxTrivia1);
                }
                if (this.token.HasValue && (CSharpExtensions.Kind(this.token.Value) == SyntaxKind.None || CSharpExtensions.Kind(this.token.Value) == SyntaxKind.EndOfFileToken))
                    return new TextEndOfParagraph(2);
                var token0 = this.token.Value;
                if (CSharpExtensions.Kind(token0) == SyntaxKind.EndOfLineTrivia) return new CustomTextEndOfLine(2);
                var len = k.Length;
                if (len == 0)
                {
                    TakeToken();
                    return GetTextRun(textSourceCharacterIndex);
                }

                TakeToken();
                if (token0.Text.Length != len)
                {
                }

                return new CustomTextCharacters(token0.Text, MakeProperties(token, token0.Text));
            }

        
        }

        private void CheckToken(SyntaxToken? syntaxToken)
        {
            if (!syntaxToken.HasValue || syntaxToken.Value.SyntaxTree != Tree)
            {
                // throw new InvalidOperationException();
            }
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
            var syntaxKind = CSharpExtensions.Kind(trivia);
            DebugUtils.WriteLine($"{syntaxKind}", DebugCategory.TextFormatting);
            if (syntaxKind == SyntaxKind.SingleLineCommentTrivia || syntaxKind == SyntaxKind.MultiLineCommentTrivia)
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
            set
            {
                if (Equals(value, _node)) return;
                _node = value;
                SyntaxInfos = GetSyntaxInfos().GetEnumerator();
                OnPropertyChanged();
            }
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

                        // DebugUtils.WriteLine(
                            // $"no customizations for {arg} - {text} {arg.GetType().Name} {kind} {nodeKind} [{tkind}]");
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
                    Text = _tree.GetText();
                    Length = Text.Length;
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
        private List<StartInfo> _starts = new List<StartInfo>();
        private SyntaxToken? token;
        private int _curStart;
        private SyntaxTree _newTree;
        private SyntaxTrivia? trivia;
        private SyntaxInfo _prev;
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
            var newText = Text.WithChanges(change);
            if(newText.Length != Text.Length + text.Length)
            {
                DebugUtils.WriteLine($"Unexpected length");
            }
            var newTree = Tree.WithChangedText(newText);
            _newTree = newTree;
            // Compilation = CSharpCompilation.Create("edit", new[]{newTree}, new[]{MetadataReference.CreateFromFile(typeof(object).Assembly.Location)});
            // foreach (var diagnostic in Compilation.GetParseDiagnostics())
            // {
                // DebugUtils.WriteLine(diagnostic.ToString());
            // }

            // Model = Compilation.GetSemanticModel(newTree);

            var chL = newTree.GetChangedSpans(Tree);
            var syntaxNode = newTree.GetRoot();
#if false
            foreachs (var textSpan in chL)
            {
                var sn = syntaxNode.ChildThatContainsPosition(textSpan.Start);
                var istoken = sn.IsToken;
                SyntaxToken? token00 = istoken ?sn.AsToken():(SyntaxToken?) null;
                var fs = sn.FullSpan;
                bool finished=false;
                if (sn.HasLeadingTrivia)
                {
                    var lt = sn.GetLeadingTrivia();
                    foreach (var syntaxTrivia in lt)
                    {
                        var syntaxTriviaSpan = syntaxTrivia.FullSpan;
                        if (syntaxTriviaSpan.IntersectsWith(textSpan))
                        {
                               
                            var ii = _starts.FindIndex(z => z.TextSpan.OverlapsWith(syntaxTriviaSpan));
                            
                            var startInfo = new StartInfo(syntaxTrivia);
                            if (ii == -1)
                            {
                                if (!_starts.Any())
                                {
                                    _starts.Add(startInfo);

                                    _curStart = 0;
                                }
                                else
                                {
                                    throw new InvalidOperationException();
                                }
                            }
                            else
                            {
                                _curStart = ii;
                                _starts[_curStart] = startInfo;
                            }

                            DebugUtils.WriteLine($"[{_curStart}]: {_starts[_curStart]}");
                            finished = true;
                            break;
                            // foreach (var (textSpan1, syntaxToken) in _starts)
                            // {
                                // if (textSpan1.Value.OverlapsWith(syntaxTriviaSpan))
                                // {

                                // }
                            // }
                        }
                    }

                    if (finished)
                        break;
                    var lastLt1 = lt.Last();
                    var lastlt = lastLt1.FullSpan;
                    if (lastLt1.Span.IntersectsWith(textSpan))
                    {
                        var (i0, span0, token0) = SearchStarts(textSpan);
                        if (i0 != -1)
                        {
                            _starts[i0] = new StartInfo(lastLt1.Token.Span, lastLt1.Token);
                            _curStart = i0;
                            break;

                        }
                    }

                    var k = CSharpExtensions.Kind(lastLt1);
                    if (k != SyntaxKind.EndOfLineTrivia)
                    {

                    }
                }

                var (i1, span, token1) = SearchStarts(textSpan);

                SyntaxNodeOrToken sn2=null;
                if (span != null)
                {
                    sn2 = sn.Parent.ChildThatContainsPosition(span.Value.Start);
                }

                var syntaxKind = CSharpExtensions.Kind(sn2);
                if (SyntaxFacts.IsTrivia(syntaxKind))
                {

                }

                if (syntaxKind == SyntaxKind.EndOfFileToken)
                {

                }
                var xx = _starts.TakeWhile((tuple, i) => tuple.TextSpan.Start < textSpan.Start && tuple.TextSpan.End < textSpan.Start);
                var c = xx.Count();
                _starts = xx.ToList();
                if (_starts.Any())
                {
                    this.token = _starts[_starts.Count - 1].Token;
                    if (CSharpExtensions.Kind(this.token.Value) == SyntaxKind.EndOfFileToken)
                    {

                    }
                }
                else
                {
                    this.token = null;
                }
                DumpStarts();
                // this.token = sn.IsNode ? sn.AsNode().GetFirstToken(true, true, true, true) : sn.AsToken();
                // this.token = this.token.Value.GetPreviousToken(true, true, true, true);
                DebugUtils.WriteLine("Changed region " + textSpan);
            }
#endif
            Tree = newTree;
            _newTree = null;
            Node = syntaxNode;
            // foreach (var syntaxInfo in GetSyntaxInfos())
            // {
                // DebugUtils.WriteLine(syntaxInfo.ToString());
            // }

            SyntaxInfos = GetSyntaxInfos().GetEnumerator();
            
            return;
#if false

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
#endif
        }

        private IEnumerator<SyntaxInfo> SyntaxInfos { get; set; }

        public (int i, TextSpan? span, SyntaxToken? token1) SearchStarts(TextSpan textSpan)
        {
            if (!_starts.Any())
                return (-1, null, null);
            var i = 0;
            TextSpan? span = null;
            SyntaxToken? token1 = null;
            for (; i < _starts.Count; i++)
            {
                (span, token1) = _starts[i];
                if (span.Value.IntersectsWith(textSpan))
                {
                    break;
                }
            }

            return (i, span, token1);
        }

        private void DumpStarts()
        {
            DebugUtils.WriteLine($"Starts: {string.Join(", ", _starts.Select(z => $"{z.TextSpan} {z.Token}"))}");
        }

        public SemanticModel Model { get; set; }

        public SourceText Text
        {
            get { return _text; }
            set
            {
                if (Equals(value, _text)) return;
                _text = value;
                OnPropertyChanged();
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class SyntaxInfo
    {
        public SyntaxTrivia? SyntaxTrivia { get; }
        public SyntaxToken? SyntaxToken { get; }

        public SyntaxInfo(in SyntaxTrivia syntaxTrivia)
        {
            SyntaxTrivia = syntaxTrivia;
            Span1 = syntaxTrivia.FullSpan;
            Text = syntaxTrivia.ToFullString();
        }

        public TextSpan Span1 { get; set; }

        public SyntaxInfo(in SyntaxToken syntaxToken)
        {
            SyntaxToken = syntaxToken;
            Span1 = syntaxToken.Span;
            Text = syntaxToken.Text;
        }

        public string Text { get; set; }

        public override string ToString()
        {
            return $"{Span1} " + (SyntaxTrivia.HasValue
                ? "SyntaxTrivia " + CSharpExtensions.Kind(SyntaxTrivia.Value)
                : "SyntaxToken " + CSharpExtensions.Kind(SyntaxToken.Value)) + " " + Text;
        }
    }

    internal class StartInfo
    {
        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{nameof(SyntaxTrivia)}: {SyntaxTrivia}, {nameof(Token)}: {Token}, {nameof(TextSpan)}: {TextSpan}]";
        }

        public SyntaxTrivia? SyntaxTrivia { get; }

        public StartInfo(TextSpan textSpan, SyntaxToken? token = null)
        {
            Token = token;
            TextSpan = textSpan;
        }

        public StartInfo(in SyntaxTrivia syntaxTrivia)
        {
            SyntaxTrivia = syntaxTrivia;
            TextSpan = syntaxTrivia.FullSpan;
        }

        public SyntaxToken? Token { get; set; }
        public TextSpan TextSpan { get; set; }

        public void Deconstruct(out TextSpan? span, out SyntaxToken? token1)
        {
            span = TextSpan;
            token1 = Token;
        }
    }
}