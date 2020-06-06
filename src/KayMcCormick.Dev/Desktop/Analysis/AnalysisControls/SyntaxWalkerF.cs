using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.VisualBasic;
using NLog;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;
using SyntaxKind = Microsoft.CodeAnalysis.CSharp.SyntaxKind;
using VisualBasicExtensions = Microsoft.CodeAnalysis.VisualBasic.VisualBasicExtensions;

namespace AnalysisControls
{
    internal class SyntaxWalkerF : CSharpSyntaxWalker
    {
        private readonly IList<TextRun> _lrun;
        private readonly Object _source3;
        private readonly Action<TextRun> _takeTextRun;
        
        private readonly Object s3;
        private Func<object, string, TextRunProperties> _propertiesFunc;
        public int CurPos { get; set; }
        private Stack<SyntaxNode> nodes=new Stack<SyntaxNode>();
        private List<NodeRuns> runs= new List<NodeRuns>();
        private Stack<List<NodeRuns>> _nodeStack = new Stack<List<NodeRuns>>();
        private List<TextRun> _textRuns = new List<TextRun>();
        private TextRun _prevTextRun;

        public SyntaxWalkerF( IList<TextRun> lrun, 
            
            Object source3, 
            Action<TextRun> takeTextRun,
            Func<object, string, TextRunProperties> propertiesFunc,
            SyntaxWalkerDepth depth = SyntaxWalkerDepth.StructuredTrivia) : base(depth)
        {
            _lrun = lrun;
            _source3 = source3;
            _takeTextRun = takeTextRun;
            _propertiesFunc = propertiesFunc;
        }

        public override void VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        {
            base.VisitDocumentationCommentTrivia(node);
        }

        public override void VisitToken(SyntaxToken token)
        {
            VisitLeadingTrivia(token);
            DoToken(token);
            VisitTrailingTrivia(token);
        }

        public override  void VisitLeadingTrivia(SyntaxToken token)
        {
            foreach (var syntaxTrivia in token.LeadingTrivia)
            {
                DoTrivia(syntaxTrivia);
            }
        }

        private void DoTrivia(SyntaxTrivia syntaxTrivia)
        {
            DebugUtils.WriteLine("At " + CSharpExtensions.Kind(syntaxTrivia).ToString(), DebugCategory.Syntax);
            if (syntaxTrivia.HasStructure)
            {
                var w = new TriviaWalker(_lrun, _source3, _takeTextRun, _propertiesFunc,
                    SyntaxWalkerDepth.StructuredTrivia);
                // Debug.WriteLine($"Curpos is {CurPos}");
                w.CurPos = CurPos;
                w.Visit(syntaxTrivia.GetStructure());
                // Debug.WriteLine($"Curpos is now {w.CurPos}");
                CurPos = w.CurPos;
                return;
            }

            if (syntaxTrivia.Span.Start != CurPos)
            {
                if (CurPos < syntaxTrivia.Span.Start)
                {
                    
                }
                //throw new AppInvalidOperationException($"{syntaxTrivia.Span.Start} != {_curPos}");
            }
            if (CSharpExtensions.Kind(syntaxTrivia) == SyntaxKind.EndOfLineTrivia)
            {
                Take(new CustomTextEndOfLine(2, syntaxTrivia.Span));
                return;
            }

            if (syntaxTrivia.HasStructure)
            {
                //syntaxTrivia.GetStructure()
            }
         //   RecordLocation(syntaxTrivia.GetLocation());

            var text = syntaxTrivia.ToFullString();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (CSharpExtensions.Kind(syntaxTrivia) == SyntaxKind.WhitespaceTrivia)
            {
                // if (text.Contains("\r"))
                // {
                    // return;
                // }
            }


            var i = 0;
            var start = 0;
            while (i != -1 && start < text.Length)
            {
                i = text.IndexOf("\r\n", start);
                if (i != -1)
                {
                    var len = i - start;
                    var line = text.Substring(start, len);
                    if (len != 0)
                    {
                        Insert(syntaxTrivia, line, start, true, false);
                    }

                    start = i + 2;
                    Take(new CustomTextEndOfLine(2){Partial=true});
                }
                else
                {
                    if (start == 0)
                    {
                        Insert(syntaxTrivia, text);
                    }
                    else
                    {
                        Insert(syntaxTrivia, text.Substring(start), start, true, true);
                    }
                    

                }
            }

            
            //Insert(syntaxTrivia, text);

        }

        private void Take(TextRun run)
        {

            TextSpan? span = null;
            bool partial = false;
            if (run is CustomTextCharacters cc)
            {
                cc.PrevTextRun = _prevTextRun;
                
                span = cc.Span;
                partial = cc.Partial;
            }
            if (_prevTextRun is CustomTextCharacters cc1)
            {
                cc1.NextTextRun = run;
            }
            _textRuns.Add(run);
            //_nodeStack.Peek().TakeTextRun(run);
            var l = run.Length;
            CurPos += l;
            // Debug.WriteLine($"{CurPos} {l}");
            if (span.HasValue && span.Value.End != CurPos)
            {
                if (!partial)
                {

                }
            }
            _takeTextRun(run);
            _prevTextRun = run;
        }

        private void Insert(SyntaxTrivia syntaxTrivia, string text, int offset = 0, bool partial= false,bool finalPartial = false)
        {
            var textRunProperties = _propertiesFunc(syntaxTrivia, text);
            var syn = new SyntaxTriviaTextCharacters(text, 0, text.Length,
                textRunProperties, syntaxTrivia.FullSpan, syntaxTrivia)
            {
                Index = syntaxTrivia.SpanStart+offset,
                Partial = partial,
                FinalPartial = finalPartial
            };
            Take(syn);
        }

        public override void DefaultVisit(SyntaxNode node)
        {
            nodes.Push(node);
            _nodeStack.Push(new List<NodeRuns>());
            //runs.Add(new NodeRuns());
            // foreach (var syntaxTrivia in node.GetLeadingTrivia())
            // {
                // DoTrivia(syntaxTrivia);
            // }
            DebugUtils.WriteLine($"At {CSharpExtensions.Kind(node)}", DebugCategory.Syntax);
            var l = node.GetLocation();
            var s1 = l.SourceSpan.Start;
            var pos = CurPos;
            TextSpan ss;
            if (node.HasLeadingTrivia)
            {
                ss = node.GetLeadingTrivia().Span;
            }
            if (s1 < CurPos)
            {
                
                
                DebugUtils.WriteLine($"Skipping {CurPos - s1} characters", DebugCategory.Syntax);
            }
            if (CurPos > s1)
            {
                DebugUtils.WriteLine("Position mismatch", DebugCategory.Syntax);
                int end = 0;
                for (int i = 0; i < _lrun.Count; i++)
                {
                    var index = i;
                    var textRun = _lrun[index];
                    
                    TextSpan span = default;
                    if (textRun is ICustomSpan ctc)
                    {
                        span = ctc.Span;
                        if (end != span.Start && !(textRun is CustomTextEndOfLine))
                        {
                            // Debug.WriteLine($"{end} !- {span.Start}", DebugCategory.Syntax);
                            throw new AppInvalidOperationException();
                        }
                        
                        
                        if(!ctc.Partial || ctc.FinalPartial && !(textRun is CustomTextEndOfLine))
                            end = span.End;
                    }
                    else
                    {
                        throw new AppInvalidOperationException();
                    }
                    
                    // Debug.WriteLine($"{index} [{span}] " + textRun.Length.ToString() + $" {textRun}", DebugCategory.Syntax);
                }   

                //throw new AppInvalidOperationException($"{CurPos} is not {s1}");
            }
            base.DefaultVisit(node);
            var n = nodes.Pop();
            var lastnr = _nodeStack.Pop();

            seen.Push(n);
            if (object.ReferenceEquals(n, node) == false)
            {
                throw new AppInvalidOperationException();
            }

        }

        public Stack<SyntaxNode> seen { get; set; } = new Stack<SyntaxNode>();

        public override void VisitTrailingTrivia(SyntaxToken token)
        {
            foreach (var syntaxTrivia in token.TrailingTrivia)
            {
                DoTrivia(syntaxTrivia);
            }
            //base.VisitTrailingTrivia(token);
        }

        public void DoToken(SyntaxToken token)
        {
            var text = token.Text;
            var c = token.Span.Length;
            
            // if (c != text.Length)
            // {
                // throw new AppInvalidOperationException($"{text} != {c}");
            // }
            if (text.Length == 0)
            {
                //_lrun.Add(new CustomRun2(token, _source3.PropsFor(token, text)));
            }
            else
            {
                text = ProcessText(text);
                var textRunProperties = _propertiesFunc(token, text);
                
                var syn = new SyntaxTokenTextCharacters(text, token.Span.Length, textRunProperties, token, nodes.Peek())
                {
                    Index = token.SpanStart
                };
                Take(syn);
            }
        }

        private static string ProcessText(string text)
        {
            if (StripEol)
            {
                while (text.EndsWith("\r\n"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
            }

            return text;
        }

        public static bool StripEol { get; set; }


        // public override void Visit(SyntaxNode node)
        // {
            // base.Visit(node);
            // foreach (var syntaxToken in node.ChildTokens())
            // {
                // if (syntaxToken.ToFullString().Length == 0)
                // {

                // }
                // else
                // {
                    // _lrun.Add(new CustomTextCharacters(syntaxToken.ToFullString(), _source3.PropsFor(syntaxToken)));
                // }
            // }

        // }

    }

    internal class TriviaWalker : SyntaxWalkerF
    {
        public TriviaWalker(IList<TextRun> lrun, Object source3, Action<TextRun> takeTextRun, Func<object, string, TextRunProperties> propertiesFunc, SyntaxWalkerDepth depth = SyntaxWalkerDepth.StructuredTrivia) : base(lrun, source3, takeTextRun, propertiesFunc, depth)
        {
        }
    }
    internal class TriviaWalkerVb : SyntaxTalkVb
    {
        public TriviaWalkerVb(IList<TextRun> lrun, Object source3, Action<TextRun> takeTextRun, Func<object, string, TextRunProperties> propertiesFunc, SyntaxWalkerDepth depth = SyntaxWalkerDepth.StructuredTrivia) : base(lrun, source3, takeTextRun, propertiesFunc, depth)
        {
        }
    }

    internal class NodeRuns
    {
        private List<TextRun> _list = new List<TextRun>();

        public void TakeTextRun(TextRun run)
        {
            _list.Add(run);
        }
    }

    public class SyntaxTalkVb : VisualBasicSyntaxWalker
    {
        private readonly IList<TextRun> _lrun;
        private readonly Object _source3;
        private readonly Action<TextRun> _takeTextRun;

        private readonly Object s3;
        private Func<object, string, TextRunProperties> _propertiesFunc;
        public int CurPos { get; set; }
        private Stack<SyntaxNode> nodes = new Stack<SyntaxNode>();
        private List<NodeRuns> runs = new List<NodeRuns>();
        private Stack<List<NodeRuns>> _nodeStack = new Stack<List<NodeRuns>>();
        private List<TextRun> _textRuns = new List<TextRun>();
        private TextRun _prevTextRun;

        public SyntaxTalkVb(IList<TextRun> lrun,

            Object source3,
            Action<TextRun> takeTextRun,
            Func<object, string, TextRunProperties> propertiesFunc,
            SyntaxWalkerDepth depth = SyntaxWalkerDepth.StructuredTrivia) : base(depth)
        {
            _lrun = lrun;
            _source3 = source3;
            _takeTextRun = takeTextRun;
            _propertiesFunc = propertiesFunc;
        }


        public override void VisitToken(SyntaxToken token)
        {
            VisitLeadingTrivia(token);
            DoToken(token);
            VisitTrailingTrivia(token);
        }

        public override void VisitLeadingTrivia(SyntaxToken token)
        {
            foreach (var syntaxTrivia in token.LeadingTrivia)
            {
                DoTrivia(syntaxTrivia);
            }
        }

        private void DoTrivia(SyntaxTrivia syntaxTrivia)
        {
            DebugUtils.WriteLine("At " + CSharpExtensions.Kind(syntaxTrivia).ToString(), DebugCategory.Syntax);
            if (syntaxTrivia.HasStructure)
            {
                var w = new TriviaWalkerVb(_lrun, _source3, _takeTextRun, _propertiesFunc,
                    SyntaxWalkerDepth.StructuredTrivia);
                w.CurPos = CurPos;
                w.Visit(syntaxTrivia.GetStructure());
                CurPos = w.CurPos;
                return;
            }

            if (syntaxTrivia.Span.Start != CurPos)
            {
                if (CurPos < syntaxTrivia.Span.Start)
                {

                }
                //throw new AppInvalidOperationException($"{syntaxTrivia.Span.Start} != {_curPos}");
            }
            if (VisualBasicExtensions.Kind(syntaxTrivia) == Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.EndOfLineTrivia)
            {
                Take(new CustomTextEndOfLine(2, syntaxTrivia.Span));
                return;
            }

            if (syntaxTrivia.HasStructure)
            {
                //syntaxTrivia.GetStructure()
            }
            //   RecordLocation(syntaxTrivia.GetLocation());

            var text = syntaxTrivia.ToFullString();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (CSharpExtensions.Kind(syntaxTrivia) == SyntaxKind.WhitespaceTrivia)
            {
                // if (text.Contains("\r"))
                // {
                // return;
                // }
            }

            while (text.EndsWith("\r\n"))
            {
                text = text.Substring(0, text.Length - 2);
            }
            Insert(syntaxTrivia, text);

        }

        private void Take(TextRun run)
        {
            if (run is CustomTextCharacters cc)
            {
                cc.PrevTextRun = _prevTextRun;
            }
            if (_prevTextRun is CustomTextCharacters cc1)
            {
                cc1.NextTextRun = run;
            }
            _textRuns.Add(run);
            //_nodeStack.Peek().TakeTextRun(run);
            var l = run.Length;
            CurPos += l;
            _takeTextRun(run);
            _prevTextRun = run;
        }

        private void Insert(SyntaxTrivia syntaxTrivia, string text)
        {
            var textRunProperties = _propertiesFunc(syntaxTrivia, text);
            var syn = new SyntaxTriviaTextCharacters(text, 0, syntaxTrivia.Span.Length,
                textRunProperties, syntaxTrivia.FullSpan, syntaxTrivia)
            {
                Index = syntaxTrivia.SpanStart
            };
            Take(syn);
        }

        public override void DefaultVisit(SyntaxNode node)
        {
            nodes.Push(node);
            _nodeStack.Push(new List<NodeRuns>());
            //runs.Add(new NodeRuns());
            // foreach (var syntaxTrivia in node.GetLeadingTrivia())
            // {
            // DoTrivia(syntaxTrivia);
            // }
            DebugUtils.WriteLine($"At {CSharpExtensions.Kind(node)}", DebugCategory.Syntax);
            var l = node.GetLocation();
            var s1 = l.SourceSpan.Start;
            var pos = CurPos;

            if (s1 < CurPos)
            {
                DebugUtils.WriteLine($"Skipping {CurPos - s1} characters", DebugCategory.Syntax);
            }
            if (CurPos > s1)
            {
                DebugUtils.WriteLine("Position mismatch", DebugCategory.Syntax);
                int end = 0;
                for (int i = 0; i < _lrun.Count; i++)
                {
                    var index = i;
                    var textRun = _lrun[index];

                    TextSpan span = default;
                    if (textRun is ICustomSpan ctc)
                    {
                        span = ctc.Span;
                        if (end != span.Start)
                        {
                            DebugUtils.WriteLine($"{end} !- {span.Start}", DebugCategory.Syntax);
                            throw new AppInvalidOperationException();
                        }

                        end = span.End;
                    }
                    else
                    {
                        throw new AppInvalidOperationException();
                    }

                    DebugUtils.WriteLine($"{index} [{span}] " + textRun.Length.ToString() + $" {textRun}", DebugCategory.Syntax);
                }

                throw new AppInvalidOperationException($"{CurPos} is not {s1}");
            }
            base.DefaultVisit(node);
            var n = nodes.Pop();
            var lastnr = _nodeStack.Pop();

            seen.Push(n);
            if (object.ReferenceEquals(n, node) == false)
            {
                throw new AppInvalidOperationException();
            }

        }

        public Stack<SyntaxNode> seen { get; set; } = new Stack<SyntaxNode>();

        public override void VisitTrailingTrivia(SyntaxToken token)
        {
            foreach (var syntaxTrivia in token.TrailingTrivia)
            {
                DoTrivia(syntaxTrivia);
            }
            //base.VisitTrailingTrivia(token);
        }

        public void DoToken(SyntaxToken token)
        {
            var text = token.Text;
            var c = token.Span.Length;

            // if (c != text.Length)
            // {
            // throw new AppInvalidOperationException($"{text} != {c}");
            // }
            if (text.Length == 0)
            {
                //_lrun.Add(new CustomRun2(token, _source3.PropsFor(token, text)));
            }
            else
            {
                text = ProcessText(text);
                var textRunProperties = _propertiesFunc(token, text);

                var syn = new SyntaxTokenTextCharacters(text, token.Span.Length, textRunProperties, token, nodes.Peek())
                {
                    Index = token.SpanStart
                };
                Take(syn);
            }
        }

        private static string ProcessText(string text)
        {
            if (StripEol)
            {
                while (text.EndsWith("\r\n"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
            }

            return text;
        }

        public static bool StripEol { get; set; }


        // public override void Visit(SyntaxNode node)
        // {
        // base.Visit(node);
        // foreach (var syntaxToken in node.ChildTokens())
        // {
        // if (syntaxToken.ToFullString().Length == 0)
        // {

        // }
        // else
        // {
        // _lrun.Add(new CustomTextCharacters(syntaxToken.ToFullString(), _source3.PropsFor(syntaxToken)));
        // }
        // }

        // }

    }
}