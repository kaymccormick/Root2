using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using NLog;

namespace AnalysisControls
{
    internal class SyntaxWalkerF : CSharpSyntaxWalker
    {
        private readonly IList<TextRun> _lrun;
        private readonly CustomTextSource3 _source3;
        private readonly Action<TextRun> _takeTextRun;
        
        private readonly CustomTextSource3 s3;
        private Func<object, string, TextRunProperties> _propertiesFunc;
        private int _curPos;
        private Stack<SyntaxNode> nodes=new Stack<SyntaxNode>();

        public SyntaxWalkerF( IList<TextRun> lrun, 
            
            CustomTextSource3 source3, 
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
            base.VisitToken(token);
        }

        private new void VisitLeadingTrivia(SyntaxToken token)
        {
            foreach (var syntaxTrivia in token.LeadingTrivia)
            {
                DoTrivia(syntaxTrivia);
            }
        }

        private void DoTrivia(SyntaxTrivia syntaxTrivia)
        {
            if (syntaxTrivia.Span.Start != _curPos)
            {
                if (_curPos < syntaxTrivia.Span.Start)
                {
                    
                }
                throw new InvalidOperationException($"{syntaxTrivia.Span.Start} != {_curPos}");
            }
            if (syntaxTrivia.Kind() == SyntaxKind.EndOfLineTrivia)
            {
                Take(new CustomTextEndOfLine(2, syntaxTrivia.Span));
                return;
            }

         //   RecordLocation(syntaxTrivia.GetLocation());

            var text = syntaxTrivia.ToString();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (syntaxTrivia.Kind() == SyntaxKind.WhitespaceTrivia)
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
            var l = run.Length;
            _curPos += l;
            _takeTextRun(run);
        }

        private void Insert(SyntaxTrivia syntaxTrivia, string text)
        {
            var textRunProperties = _propertiesFunc(syntaxTrivia, text);
            Take(new SyntaxTriviaTextCharacters(text, 0, syntaxTrivia.Span.Length,
                textRunProperties, syntaxTrivia.Span, syntaxTrivia));
        }

        public override void DefaultVisit(SyntaxNode node)
        {
            nodes.Push(node);
            // foreach (var syntaxTrivia in node.GetLeadingTrivia())
            // {
                // DoTrivia(syntaxTrivia);
            // }
            DebugUtils.WriteLine($"At {node.Kind()}");
            var l = node.GetLocation();
            var s1 = l.SourceSpan.Start;
            var pos = _curPos;

            if (s1 < _curPos)
            {
                DebugUtils.WriteLine($"Skipping {_curPos - s1} characters");
            }
            if (_curPos > s1)
            {
                DebugUtils.WriteLine("Position mismatch");
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
                            DebugUtils.WriteLine($"{end} !- {span.Start}");
                            throw new InvalidOperationException();
                        }
                        
                        end = span.End;
                    }
                    
                    DebugUtils.WriteLine($"{index} [{span}] " + textRun.Length.ToString() + $" {textRun}");
                }   

                throw new InvalidOperationException($"{_curPos} is not {s1}");
            }
            base.DefaultVisit(node);
            var n = nodes.Pop();
            seen.Push(n);
            if (object.ReferenceEquals(n, node) == false)
            {
                throw new InvalidOperationException();
            }

        }

        public Stack<SyntaxNode> seen { get; set; } = new Stack<SyntaxNode>();

        private new void VisitTrailingTrivia(SyntaxToken token)
        {
            foreach (var syntaxTrivia in token.TrailingTrivia)
            {
                DoTrivia(syntaxTrivia);
            }
        }

        public void DoToken(SyntaxToken token)
        {
            var text = token.Text;
            var c = token.Span.Length;
            
            // if (c != text.Length)
            // {
                // throw new InvalidOperationException($"{text} != {c}");
            // }
            if (text.Length == 0)
            {
                //_lrun.Add(new CustomRun2(token, _source3.PropsFor(token, text)));
            }
            else
            {
                text = ProcessText(text);
                var textRunProperties = _propertiesFunc(token, text);
                Take(new SyntaxTokenTextCharacters(text, token.Span.Length, textRunProperties, token, nodes.Peek()));
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

    internal class SyntaxTriviaTextCharacters : CustomTextCharacters
    {

        private readonly SyntaxTrivia _syntaxTrivia;

        public SyntaxTriviaTextCharacters([NotNull] char[] characterArray, int offsetToFirstChar, int length, [NotNull] TextRunProperties textRunProperties, TextSpan span, SyntaxTrivia syntaxTrivia) : base(characterArray, offsetToFirstChar, length, textRunProperties, span)
        {
            _syntaxTrivia = syntaxTrivia;
        }

        public SyntaxTriviaTextCharacters([NotNull] string characterString, [NotNull] TextRunProperties textRunProperties, TextSpan span, SyntaxTrivia syntaxTrivia) : base(characterString, textRunProperties, span)
        {
            _syntaxTrivia = syntaxTrivia;
        }

        public SyntaxTriviaTextCharacters([NotNull] string characterString, int offsetToFirstChar, int length, [NotNull] TextRunProperties textRunProperties, TextSpan span, SyntaxTrivia syntaxTrivia) : base(characterString, offsetToFirstChar, length, textRunProperties, span)
        {
            _syntaxTrivia = syntaxTrivia;
        }

        public unsafe SyntaxTriviaTextCharacters([NotNull] char* unsafeCharacterString, int length, [NotNull] TextRunProperties textRunProperties, TextSpan span, SyntaxTrivia syntaxTrivia) : base(unsafeCharacterString, length, textRunProperties, span)
        {
            _syntaxTrivia = syntaxTrivia;
        }

        public override string ToString()
        {
            return $"SyntaxTrivia {_syntaxTrivia.Kind()} [{Length}]";
        }

    }

    /// <inheritdoc />
    public class CustomTextEndOfLine : TextEndOfLine, ICustomSpan
    {
        private TextSpan _span;

        public TextSpan Span
        {
            get { return _span; }
            set { _span = value; }
        }

        public CustomTextEndOfLine(int length, TextSpan span) : base(length)
        {
            _span = span;
        }

        public CustomTextEndOfLine(int length, TextRunProperties textRunProperties, TextSpan span) : base(length, textRunProperties)
        {
        }
    }

    public interface ICustomSpan
    {
        TextSpan Span {
            get;
        }
    }
}