using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalysisControls
{
    class CustomTextSource3 : TextSource
    {
        public CustomTextSource3(double pixelsPerDip)
        {
            PixelsPerDip = pixelsPerDip;
        }

        public Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                var stringBuilder = new StringBuilder();
                if (_type != null)
                {
                    col.Clear();
                    chars.Clear();
                }
                //_text = stringBuilder.ToString();
            }
        }

	public CSharpCompilation Compilation {get; set;}

        // Used by the TextFormatter object to retrieve a run of text from the text source.
        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            // Make sure text source index is in bounds.
            if (textSourceCharacterIndex < 0)
                throw new ArgumentOutOfRangeException("textSourceCharacterIndex", "Value must be greater than 0.");
            if (textSourceCharacterIndex >= length)
            {
                return new TextEndOfParagraph(1);
            }

            // Create TextCharacters using the current font rendering properties.
            if (textSourceCharacterIndex < length)
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
                else
                {
                    return col[xx];
                }
            }

            // Return an end-of-paragraph if no more text source.
            return new TextEndOfParagraph(1);
        }

        public int length { get; set; }

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

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #region Properties

        public FontRendering FontRendering
        {
            get { return _currentRendering; }
            set { _currentRendering = value; }
        }

        public SyntaxNode Node  
        {
            get { return _node; }
            set
            {
                _node = value;
                GenerateText(); 
            }
        }

        private void GenerateText()
        {
            //_node = Node.NormalizeWhitespace("    ", "");//"    ", "", true);
            //length = Node.ToFullString().Length;
            SyntaxWalkerF f = new SyntaxWalkerF(col,this, TakeTextRun, MakeProperties);
            f.DefaultVisit(Node);
            int i = 0;
            chars.Clear();
            var model =  Compilation?.GetSemanticModel(Tree);
            foreach (var textRun in col)
            {
                length += textRun.Length;
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
            {
                return PropsFor(st, text);
            } else if (arg is SyntaxToken t)
            {
                return PropsFor(t, text);
            }

            return null;
        }

        private void TakeTextRun(TextRun obj)
        {
            col.Add(obj);
        }

        public SyntaxTree Tree { get; set; }

        public TextRunProperties PropsFor(SyntaxToken symbolDisplayPart, string text)
        {   
            var pp = BasicProps();
            pp.SyntaxToken = symbolDisplayPart;
            pp.Text = text;
            return pp;
        }

        #endregion

        #region Private Fields

        private FontRendering _currentRendering;
        private int lemgth;
        private Type _type;
        private static double _pixelsPerDip;
        private List<int> chars = new List<int>();
        private List<TextRun> col = new List<TextRun>();
        private FontFamily _fontFamily = new FontFamily("Courier New");
        private FontWeight _fontWeight = FontWeights.Medium;
        private FontStyle _fontStyle  = FontStyles.Normal;
        private FontStretch _fontStretch = FontStretches.Normal;
        private int _emSize = 24;
        private IList colx = new ArrayList();
        private List<int> chars2= new List<int>();
        private SyntaxNode _node;

        #endregion

        private static string FullyQualifiedMetadataName(Type myType)
        {
            return (myType.Namespace != null ? myType.Namespace + "." : "") + myType.Name;
        }

        public GenericTextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ITypeSymbol tt)
        {
            var xx = BasicProps();
            xx.SymbolDisplaYPart = symbolDisplayPart;
            xx.TypeSymbol = tt;
            return xx;
        }

        public GenericTextRunProperties BasicProps()
        {
            var tf =
                new Typeface(_fontFamily, _fontStyle, _fontWeight,
                    _fontStretch);
            var fontRendering =
                new FontRendering(_emSize, TextAlignment.Left, new TextDecorationCollection(), Brushes.Blue, tf);
            var xx = new GenericTextRunProperties(
                fontRendering,
                PixelsPerDip);
            return xx;
        }

        private static TypeSyntax M (Type arg)
        {
            var t = DevTypeControl.MyTypeName(arg);
            var identifierNameSyntax = SyntaxFactory.ParseTypeName(t);
            if (arg.IsGenericType)
            {
                return SyntaxFactory.GenericName(SyntaxFactory.Identifier(arg.Name), SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList<TypeSyntax>().AddRange(arg.GenericTypeArguments.Select(M))));
            }
            return identifierNameSyntax;
        }

        public TextRunProperties PropsFor(in SyntaxTrivia symbolDisplayPart, string text)
        {
            var r = BasicProps();
            r.SyntaxTrivia = symbolDisplayPart;

            r.Text = text;
            return r;
        }
    }

    internal class CustomRun2 : TextRun
    {
        private readonly SyntaxToken _token;

        public CustomRun2(in SyntaxToken token, TextRunProperties propsFor)
        {
            Properties = propsFor;
        }

        public override CharacterBufferReference CharacterBufferReference { get; }
        public override int Length { get; }
        public override TextRunProperties Properties { get; }
    }
}
