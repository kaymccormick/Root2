using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Castle.DynamicProxy;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    class CustomTextSource2 : TextSource
    {
        public CustomTextSource2(double pixelsPerDip)
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
                    _text = "";
                    GenerateTextForType(_type, col, stringBuilder, false);
                }
                //_text = stringBuilder.ToString();
            }
        }


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
                    throw new InvalidOperationException();
                }
            }

            // Return an end-of-paragraph if no more text source.
            return new TextEndOfParagraph(1);
        }

        public int length => Text.Length;

        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(
            int textSourceCharacterIndexLimit)
        {
            CharacterBufferRange cbr = new CharacterBufferRange(_text, 0, textSourceCharacterIndexLimit);
            return new TextSpan<CultureSpecificCharacterBufferRange>(
                textSourceCharacterIndexLimit,
                new CultureSpecificCharacterBufferRange(CultureInfo.CurrentUICulture, cbr)
            );
        }

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #region Properties

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public FontRendering FontRendering
        {
            get { return _currentRendering; }
            set { _currentRendering = value; }
        }

        #endregion

        #region Private Fields

        private string _text = ""; //text store
        private FontRendering _currentRendering;
        private Type _type;
        private static double _pixelsPerDip;
        private List<int> chars = new List<int>();
        private List<TextRun> col = new List<TextRun>();
        private FontFamily _fontFamily;
        private FontWeight _fontWeight;
        private FontStyle _fontStyle;
        private FontStretch _fontStretch;
        private int _emSize = 12;
        private IList colx = new ArrayList();
        private List<int> chars2= new List<int>();

        #endregion

        private void GenerateTextForType([NotNull] Type myType
            , IList col
            , StringBuilder sb
            , bool toolTip)
        {
            
            if ( myType == null )
            {
                throw new ArgumentNullException ( nameof ( myType ) ) ;
            }

            List<MetadataReference> mrs = new List<MetadataReference>();
            var mr =MetadataReference.CreateFromFile(myType.Assembly.Location);
            mrs.Add(mr);
            ISet<Assembly> coll = new HashSet<Assembly>();

            void Collect(Type t1, ISet<Assembly> col0)
            {
                col0.Add(t1.Assembly);
                if (t1.IsGenericType && t1.IsConstructedGenericType)
                {
                    foreach (var xx in t1.GenericTypeArguments)
                    {
                        Collect(xx, col0);
                    }
                }
            }
            Collect(myType, coll);
            var mrs1 = coll.Select(a => a.Location).Select(y => MetadataReference.CreateFromFile(y));
            CSharpCompilation x1 = CSharpCompilation.Create("x", new[]
                {
                    SyntaxFactory.SyntaxTree(SyntaxFactory.CompilationUnit())
                }, mrs1,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var xyz = x1.GetTypeByMetadataName(myType.FullName);

            void procNs(INamespaceSymbol nst)
            {

                foreach (var namespaceOrTypeSymbol in nst.GetNamespaceMembers())
                {
                    procNs(namespaceOrTypeSymbol);
                }

                foreach (var t in nst.GetTypeMembers())
                {
                    DebugUtils.WriteLine(t.Name);
                    DebugUtils.WriteLine(t.MetadataName);
                    DebugUtils.WriteLine(t.Name + (t.IsGenericType ? "`" + t.Arity : ""));
                    if (t.Name.StartsWith("Dictionary"))
                    {
                        DebugUtils.WriteLine(t.ToDisplayString());
                        DebugUtils.WriteLine(t.MetadataName);
                    }

                }

            }


            //procNs(x1.GlobalNamespace);
            ITypeSymbol Find(CSharpCompilation compilation, Type t)
            {
                var fullyQualifiedMetadataName = FullyQualifiedMetadataName(t);
                var tt0 = x1.GetTypeByMetadataName(fullyQualifiedMetadataName);
                if (tt0 == null)
                {
                    throw new InvalidOperationException(fullyQualifiedMetadataName);
                }
                if (t.IsGenericType && t.IsConstructedGenericType)
                {
                    return tt0.Construct(t.GenericTypeArguments.Select(x3 => Find(compilation, x3)).ToArray());
                }

                return tt0;
            }

            var tt = Find(x1, myType);
            int index = 0;
            int j = 0;
            foreach (var symbolDisplayPart in tt.ToDisplayParts(SymbolDisplayFormat.MinimallyQualifiedFormat))
            {
                
                var fullString = symbolDisplayPart.ToString();
                
                var pp = PropsFor(symbolDisplayPart, tt);
                // var c = new SymbolTextCharacters(fullString, 0, fullString.Length, pp, tt)
                    // {TypeSymbol = tt, DisplayPart = symbolDisplayPart, Type = myType, Index = index};
                    

                // TextModifier x1z = new MyTextModifier(pp, false);
                // colx.Add(x1z);
                // colx.Add(c);
                // colx.Add(new TextEndOfSegment(0));

                var customRun = new CustomRun(index, fullString,
                    pp);
                //customRun.Prop.
                object c = null;
                col.Add(c);
                
                chars.AddRange(Enumerable.Repeat(col.Count - 1, fullString.Length));
                index += fullString.Length;
                _text += fullString;
                DebugUtils.WriteLine(j.ToString() + fullString);
                j++;

            }
            // DebugUtils.WriteLine(tt.ToDisplayString());
            // var myTypeName = DevTypeControl.MyTypeName(myType);
            // sb.Append(myTypeName);
            // TextPart p = new TextPart() {Text = myTypeName, PartKind = PartKind.TypeName};
            // col.Add(p);
            // if ( ! myType.IsGenericType )
            // {
                // return ;
            // }

            // var x = M(myType);
            // int j = 0;
            // _text = "";
            // int index = 0;
            // foreach (var descendantToken in x.DescendantTokens())
            // {
                // var tf =
                    // new Typeface(new FontFamily("Courier New"), new FontStyle(), FontWeights.Medium,
                        // FontStretches.Medium);
                // var xx = new GenericTextRunProperties(
                    // new FontRendering(12, TextAlignment.Left, new TextDecorationCollection(), Brushes.Blue, tf),
                    // PixelsPerDip);
                // var fullString = descendantToken.ToFullString();
                // chars.AddRange(Enumerable.Repeat(col.Count, fullString.Length));
                // col.Add(new CustomRun(index, fullString, xx));
                // index += fullString.Length;
                // _text += fullString;
                // DebugUtils.WriteLine(j.ToString( ) + fullString);
                // j++;
            // }

            return;
            sb.Append("<");
             int i = 0 ;
            foreach ( var arg in myType.GenericTypeArguments )
            {
                //GenerateTextForType( arg , col, sb, true ) ;
                if ( i < myType.GenericTypeArguments.Length - 1)
                {
                    sb.Append(", ");
                }

                i++;
            }

            sb.Append(">");

            //old.AddChild ( tb ) ;
        }

        private static string FullyQualifiedMetadataName(Type myType)
        {
            return (myType.Namespace != null ? myType.Namespace + "." : "") + myType.Name;
        }

        private GenericTextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ITypeSymbol tt)
        {
            _fontFamily = new FontFamily("Courier New");
            _fontStyle = FontStyles.Normal;
            _fontWeight = FontWeights.Medium;
            _fontStretch = FontStretches.Medium;
            var tf =
                new Typeface(_fontFamily, _fontStyle, _fontWeight,
                    _fontStretch);
            var fontRendering = new FontRendering(_emSize, TextAlignment.Left, new TextDecorationCollection(), Brushes.Blue, tf);
            var xx = new GenericTextRunProperties(
                fontRendering,
                PixelsPerDip);
            // xx.SymbolDisplaYPart = symbolDisplayPart;
            // xx.TypeSymbol = tt;
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
    }

    internal class SymbolTextCharacters : CustomTextCharacters
    {
        private ISymbol _symbol;
        private readonly SymbolDisplayPart _symbolDisplayPart;


        public SymbolTextCharacters([NotNull] string characterString, [NotNull] TextRunProperties textRunProperties, TextSpan span, ISymbol symbol) : base(characterString, textRunProperties, span)
        {
            _symbol = symbol;
        }

        public SymbolTextCharacters([NotNull] string characterString, int offsetToFirstChar, int length,
            [NotNull] TextRunProperties textRunProperties, TextSpan span, ISymbol symbol,
            SymbolDisplayPart symbolDisplayPart) : base(characterString, offsetToFirstChar, length, textRunProperties, span)
        {
            _symbol = symbol;
            _symbolDisplayPart = symbolDisplayPart;
        }

        public SymbolDisplayPart DisplayPart
        {
            get { return _symbolDisplayPart; }
        }
    }

    internal class MyTextModifier : TextModifier
    {
        private readonly TextRunProperties _properties;
        private readonly int _length;

        public MyTextModifier(TextRunProperties properties, bool hasDirectionalEmbedding=false)
        {
            _properties = properties;
         
            HasDirectionalEmbedding = hasDirectionalEmbedding;
         
        }

        public override int Length => 0;

        public override TextRunProperties Properties
        {
            get { return _properties; }
        }

        public override TextRunProperties ModifyProperties(TextRunProperties properties)
        {
            return properties;
        }

        public override bool HasDirectionalEmbedding { get; }
        public override FlowDirection FlowDirection { get; }
    }

    internal class CustomRun
    {
        public TextRunProperties Prop { get; }

        public CustomRun(int index, string text, TextRunProperties prop)
        {
            Index = index;
            Text = text;
            Prop = prop;
        }

        public int Index { get; }
        public string Text { get; set; }
    }

    internal enum PartKind
    {
        TypeName = 1
    }

    internal class TextPart
    {
        public string Text { get; set; }
        public object PartKind { get; set; }
    }
}