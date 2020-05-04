using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    public class CustomTextSource3 : TextSource
    {
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

        public CSharpCompilation Compilation { get; set; }

        public int length { get; set; }

        // Used by the TextFormatter object to retrieve a run of text from the text source.
        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            // Make sure text source index is in bounds.
            if (textSourceCharacterIndex < 0)
                throw new ArgumentOutOfRangeException("textSourceCharacterIndex", "Value must be greater than 0.");
            if (textSourceCharacterIndex >= length) return new TextEndOfParagraph(1);

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

                return col[xx];
            }

            // Return an end-of-paragraph if no more text source.
            return new TextEndOfParagraph(1);
        }

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

        private static string FullyQualifiedMetadataName(Type myType)
        {
            return (myType.Namespace != null ? myType.Namespace + "." : "") + myType.Name;
        }

        public TextRunProperties PropsFor(SymbolDisplayPart symbolDisplayPart, ITypeSymbol tt)
        {
            var xx = BasicProps();
            // xx.SymbolDisplaYPart = symbolDisplayPart;
            // xx.TypeSymbol = tt;
            return xx;
        }

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

        public TextRunProperties PropsFor(in SyntaxTrivia symbolDisplayPart, string text)
        {
            var r = BasicProps();
            // r.SyntaxTrivia = symbolDisplayPart;
            // r.Text = text;
            return r;
        }

        #region Properties

        public FontRendering FontRendering { get; set; }

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

        public SyntaxTree Tree { get; set; }

        public TextRunProperties PropsFor(SyntaxToken token, string text)
        {
            var pp = BasicProps();
            var kind = token.Kind();
            switch (token.Parent.Kind())
            {
                case SyntaxKind.None:
                    break;
                case SyntaxKind.List:
                    break;
                case SyntaxKind.TildeToken:
                    break;
                case SyntaxKind.ExclamationToken:
                    break;
                case SyntaxKind.DollarToken:
                    break;
                case SyntaxKind.PercentToken:
                    break;
                case SyntaxKind.CaretToken:
                    break;
                case SyntaxKind.AmpersandToken:
                    break;
                case SyntaxKind.AsteriskToken:
                    break;
                case SyntaxKind.OpenParenToken:
                    break;
                case SyntaxKind.CloseParenToken:
                    break;
                case SyntaxKind.MinusToken:
                    break;
                case SyntaxKind.PlusToken:
                    break;
                case SyntaxKind.EqualsToken:
                    break;
                case SyntaxKind.OpenBraceToken:
                    break;
                case SyntaxKind.CloseBraceToken:
                    break;
                case SyntaxKind.OpenBracketToken:
                    break;
                case SyntaxKind.CloseBracketToken:
                    break;
                case SyntaxKind.BarToken:
                    break;
                case SyntaxKind.BackslashToken:
                    break;
                case SyntaxKind.ColonToken:
                    break;
                case SyntaxKind.SemicolonToken:
                    break;
                case SyntaxKind.DoubleQuoteToken:
                    break;
                case SyntaxKind.SingleQuoteToken:
                    break;
                case SyntaxKind.LessThanToken:
                    break;
                case SyntaxKind.CommaToken:
                    break;
                case SyntaxKind.GreaterThanToken:
                    break;
                case SyntaxKind.DotToken:
                    break;
                case SyntaxKind.QuestionToken:
                    break;
                case SyntaxKind.HashToken:
                    break;
                case SyntaxKind.SlashToken:
                    break;
                case SyntaxKind.DotDotToken:
                    break;
                case SyntaxKind.SlashGreaterThanToken:
                    break;
                case SyntaxKind.LessThanSlashToken:
                    break;
                case SyntaxKind.XmlCommentStartToken:
                    break;
                case SyntaxKind.XmlCommentEndToken:
                    break;
                case SyntaxKind.XmlCDataStartToken:
                    break;
                case SyntaxKind.XmlCDataEndToken:
                    break;
                case SyntaxKind.XmlProcessingInstructionStartToken:
                    break;
                case SyntaxKind.XmlProcessingInstructionEndToken:
                    break;
                case SyntaxKind.BarBarToken:
                    break;
                case SyntaxKind.AmpersandAmpersandToken:
                    break;
                case SyntaxKind.MinusMinusToken:
                    break;
                case SyntaxKind.PlusPlusToken:
                    break;
                case SyntaxKind.ColonColonToken:
                    break;
                case SyntaxKind.QuestionQuestionToken:
                    break;
                case SyntaxKind.MinusGreaterThanToken:
                    break;
                case SyntaxKind.ExclamationEqualsToken:
                    break;
                case SyntaxKind.EqualsEqualsToken:
                    break;
                case SyntaxKind.EqualsGreaterThanToken:
                    break;
                case SyntaxKind.LessThanEqualsToken:
                    break;
                case SyntaxKind.LessThanLessThanToken:
                    break;
                case SyntaxKind.LessThanLessThanEqualsToken:
                    break;
                case SyntaxKind.GreaterThanEqualsToken:
                    break;
                case SyntaxKind.GreaterThanGreaterThanToken:
                    break;
                case SyntaxKind.GreaterThanGreaterThanEqualsToken:
                    break;
                case SyntaxKind.SlashEqualsToken:
                    break;
                case SyntaxKind.AsteriskEqualsToken:
                    break;
                case SyntaxKind.BarEqualsToken:
                    break;
                case SyntaxKind.AmpersandEqualsToken:
                    break;
                case SyntaxKind.PlusEqualsToken:
                    break;
                case SyntaxKind.MinusEqualsToken:
                    break;
                case SyntaxKind.CaretEqualsToken:
                    break;
                case SyntaxKind.PercentEqualsToken:
                    break;
                case SyntaxKind.QuestionQuestionEqualsToken:
                    break;
                case SyntaxKind.BoolKeyword:
                    break;
                case SyntaxKind.ByteKeyword:
                    break;
                case SyntaxKind.SByteKeyword:
                    break;
                case SyntaxKind.ShortKeyword:
                    break;
                case SyntaxKind.UShortKeyword:
                    break;
                case SyntaxKind.IntKeyword:
                    break;
                case SyntaxKind.UIntKeyword:
                    break;
                case SyntaxKind.LongKeyword:
                    break;
                case SyntaxKind.ULongKeyword:
                    break;
                case SyntaxKind.DoubleKeyword:
                    break;
                case SyntaxKind.FloatKeyword:
                    break;
                case SyntaxKind.DecimalKeyword:
                    break;
                case SyntaxKind.StringKeyword:
                    break;
                case SyntaxKind.CharKeyword:
                    break;
                case SyntaxKind.VoidKeyword:
                    break;
                case SyntaxKind.ObjectKeyword:
                    break;
                case SyntaxKind.TypeOfKeyword:
                    break;
                case SyntaxKind.SizeOfKeyword:
                    break;
                case SyntaxKind.NullKeyword:
                    break;
                case SyntaxKind.TrueKeyword:
                    break;
                case SyntaxKind.FalseKeyword:
                    break;
                case SyntaxKind.IfKeyword:
                    break;
                case SyntaxKind.ElseKeyword:
                    break;
                case SyntaxKind.WhileKeyword:
                    break;
                case SyntaxKind.ForKeyword:
                    break;
                case SyntaxKind.ForEachKeyword:
                    break;
                case SyntaxKind.DoKeyword:
                    break;
                case SyntaxKind.SwitchKeyword:
                    break;
                case SyntaxKind.CaseKeyword:
                    break;
                case SyntaxKind.DefaultKeyword:
                    break;
                case SyntaxKind.TryKeyword:
                    break;
                case SyntaxKind.CatchKeyword:
                    break;
                case SyntaxKind.FinallyKeyword:
                    break;
                case SyntaxKind.LockKeyword:
                    break;
                case SyntaxKind.GotoKeyword:
                    break;
                case SyntaxKind.BreakKeyword:
                    break;
                case SyntaxKind.ContinueKeyword:
                    break;
                case SyntaxKind.ReturnKeyword:
                    break;
                case SyntaxKind.ThrowKeyword:
                    break;
                case SyntaxKind.PublicKeyword:
                    break;
                case SyntaxKind.PrivateKeyword:
                    break;
                case SyntaxKind.InternalKeyword:
                    break;
                case SyntaxKind.ProtectedKeyword:
                    break;
                case SyntaxKind.StaticKeyword:
                    break;
                case SyntaxKind.ReadOnlyKeyword:
                    break;
                case SyntaxKind.SealedKeyword:
                    break;
                case SyntaxKind.ConstKeyword:
                    break;
                case SyntaxKind.FixedKeyword:
                    break;
                case SyntaxKind.StackAllocKeyword:
                    break;
                case SyntaxKind.VolatileKeyword:
                    break;
                case SyntaxKind.NewKeyword:
                    break;
                case SyntaxKind.OverrideKeyword:
                    break;
                case SyntaxKind.AbstractKeyword:
                    break;
                case SyntaxKind.VirtualKeyword:
                    break;
                case SyntaxKind.EventKeyword:
                    break;
                case SyntaxKind.ExternKeyword:
                    break;
                case SyntaxKind.RefKeyword:
                    break;
                case SyntaxKind.OutKeyword:
                    break;
                case SyntaxKind.InKeyword:
                    break;
                case SyntaxKind.IsKeyword:
                    break;
                case SyntaxKind.AsKeyword:
                    break;
                case SyntaxKind.ParamsKeyword:
                    break;
                case SyntaxKind.ArgListKeyword:
                    break;
                case SyntaxKind.MakeRefKeyword:
                    break;
                case SyntaxKind.RefTypeKeyword:
                    break;
                case SyntaxKind.RefValueKeyword:
                    break;
                case SyntaxKind.ThisKeyword:
                    break;
                case SyntaxKind.BaseKeyword:
                    break;
                case SyntaxKind.NamespaceKeyword:
                    break;
                case SyntaxKind.UsingKeyword:
                    break;
                case SyntaxKind.ClassKeyword:
                    break;
                case SyntaxKind.StructKeyword:
                    break;
                case SyntaxKind.InterfaceKeyword:
                    break;
                case SyntaxKind.EnumKeyword:
                    break;
                case SyntaxKind.DelegateKeyword:
                    break;
                case SyntaxKind.CheckedKeyword:
                    break;
                case SyntaxKind.UncheckedKeyword:
                    break;
                case SyntaxKind.UnsafeKeyword:
                    break;
                case SyntaxKind.OperatorKeyword:
                    break;
                case SyntaxKind.ExplicitKeyword:
                    break;
                case SyntaxKind.ImplicitKeyword:
                    break;
                case SyntaxKind.YieldKeyword:
                    break;
                case SyntaxKind.PartialKeyword:
                    break;
                case SyntaxKind.AliasKeyword:
                    break;
                case SyntaxKind.GlobalKeyword:
                    break;
                case SyntaxKind.AssemblyKeyword:
                    break;
                case SyntaxKind.ModuleKeyword:
                    break;
                case SyntaxKind.TypeKeyword:
                    break;
                case SyntaxKind.FieldKeyword:
                    break;
                case SyntaxKind.MethodKeyword:
                    break;
                case SyntaxKind.ParamKeyword:
                    break;
                case SyntaxKind.PropertyKeyword:
                    break;
                case SyntaxKind.TypeVarKeyword:
                    break;
                case SyntaxKind.GetKeyword:
                    break;
                case SyntaxKind.SetKeyword:
                    break;
                case SyntaxKind.AddKeyword:
                    break;
                case SyntaxKind.RemoveKeyword:
                    break;
                case SyntaxKind.WhereKeyword:
                    break;
                case SyntaxKind.FromKeyword:
                    break;
                case SyntaxKind.GroupKeyword:
                    break;
                case SyntaxKind.JoinKeyword:
                    break;
                case SyntaxKind.IntoKeyword:
                    break;
                case SyntaxKind.LetKeyword:
                    break;
                case SyntaxKind.ByKeyword:
                    break;
                case SyntaxKind.SelectKeyword:
                    break;
                case SyntaxKind.OrderByKeyword:
                    break;
                case SyntaxKind.OnKeyword:
                    break;
                case SyntaxKind.EqualsKeyword:
                    break;
                case SyntaxKind.AscendingKeyword:
                    break;
                case SyntaxKind.DescendingKeyword:
                    break;
                case SyntaxKind.NameOfKeyword:
                    break;
                case SyntaxKind.AsyncKeyword:
                    break;
                case SyntaxKind.AwaitKeyword:
                    break;
                case SyntaxKind.WhenKeyword:
                    break;
                case SyntaxKind.ElifKeyword:
                    break;
                case SyntaxKind.EndIfKeyword:
                    break;
                case SyntaxKind.RegionKeyword:
                    break;
                case SyntaxKind.EndRegionKeyword:
                    break;
                case SyntaxKind.DefineKeyword:
                    break;
                case SyntaxKind.UndefKeyword:
                    break;
                case SyntaxKind.WarningKeyword:
                    break;
                case SyntaxKind.ErrorKeyword:
                    break;
                case SyntaxKind.LineKeyword:
                    break;
                case SyntaxKind.PragmaKeyword:
                    break;
                case SyntaxKind.HiddenKeyword:
                    break;
                case SyntaxKind.ChecksumKeyword:
                    break;
                case SyntaxKind.DisableKeyword:
                    break;
                case SyntaxKind.RestoreKeyword:
                    break;
                case SyntaxKind.ReferenceKeyword:
                    break;
                case SyntaxKind.InterpolatedStringStartToken:
                    break;
                case SyntaxKind.InterpolatedStringEndToken:
                    break;
                case SyntaxKind.InterpolatedVerbatimStringStartToken:
                    break;
                case SyntaxKind.LoadKeyword:
                    break;
                case SyntaxKind.NullableKeyword:
                    break;
                case SyntaxKind.EnableKeyword:
                    break;
                case SyntaxKind.WarningsKeyword:
                    break;
                case SyntaxKind.AnnotationsKeyword:
                    break;
                case SyntaxKind.VarKeyword:
                    break;
                case SyntaxKind.UnderscoreToken:
                    break;
                case SyntaxKind.OmittedTypeArgumentToken:
                    break;
                case SyntaxKind.OmittedArraySizeExpressionToken:
                    break;
                case SyntaxKind.EndOfDirectiveToken:
                    break;
                case SyntaxKind.EndOfDocumentationCommentToken:
                    break;
                case SyntaxKind.EndOfFileToken:
                    break;
                case SyntaxKind.BadToken:
                    break;
                case SyntaxKind.IdentifierToken:
                    break;
                case SyntaxKind.NumericLiteralToken:
                    break;
                case SyntaxKind.CharacterLiteralToken:
                    break;
                case SyntaxKind.StringLiteralToken:
                    break;
                case SyntaxKind.XmlEntityLiteralToken:
                    break;
                case SyntaxKind.XmlTextLiteralToken:
                    break;
                case SyntaxKind.XmlTextLiteralNewLineToken:
                    break;
                case SyntaxKind.InterpolatedStringToken:
                    break;
                case SyntaxKind.InterpolatedStringTextToken:
                    break;
                case SyntaxKind.EndOfLineTrivia:
                    break;
                case SyntaxKind.WhitespaceTrivia:
                    break;
                case SyntaxKind.SingleLineCommentTrivia:
                    break;
                case SyntaxKind.MultiLineCommentTrivia:
                    break;
                case SyntaxKind.DocumentationCommentExteriorTrivia:
                    break;
                case SyntaxKind.SingleLineDocumentationCommentTrivia:
                    break;
                case SyntaxKind.MultiLineDocumentationCommentTrivia:
                    break;
                case SyntaxKind.DisabledTextTrivia:
                    break;
                case SyntaxKind.PreprocessingMessageTrivia:
                    break;
                case SyntaxKind.IfDirectiveTrivia:
                    break;
                case SyntaxKind.ElifDirectiveTrivia:
                    break;
                case SyntaxKind.ElseDirectiveTrivia:
                    break;
                case SyntaxKind.EndIfDirectiveTrivia:
                    break;
                case SyntaxKind.RegionDirectiveTrivia:
                    break;
                case SyntaxKind.EndRegionDirectiveTrivia:
                    break;
                case SyntaxKind.DefineDirectiveTrivia:
                    break;
                case SyntaxKind.UndefDirectiveTrivia:
                    break;
                case SyntaxKind.ErrorDirectiveTrivia:
                    break;
                case SyntaxKind.WarningDirectiveTrivia:
                    break;
                case SyntaxKind.LineDirectiveTrivia:
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
                case SyntaxKind.XmlElement:
                    break;
                case SyntaxKind.XmlElementStartTag:
                    break;
                case SyntaxKind.XmlElementEndTag:
                    break;
                case SyntaxKind.XmlEmptyElement:
                    break;
                case SyntaxKind.XmlTextAttribute:
                    break;
                case SyntaxKind.XmlCrefAttribute:
                    break;
                case SyntaxKind.XmlNameAttribute:
                    break;
                case SyntaxKind.XmlName:
                    break;
                case SyntaxKind.XmlPrefix:
                    break;
                case SyntaxKind.XmlText:
                    break;
                case SyntaxKind.XmlCDataSection:
                    break;
                case SyntaxKind.XmlComment:
                    break;
                case SyntaxKind.XmlProcessingInstruction:
                    break;
                case SyntaxKind.TypeCref:
                    break;
                case SyntaxKind.QualifiedCref:
                    break;
                case SyntaxKind.NameMemberCref:
                    break;
                case SyntaxKind.IndexerMemberCref:
                    break;
                case SyntaxKind.OperatorMemberCref:
                    break;
                case SyntaxKind.ConversionOperatorMemberCref:
                    break;
                case SyntaxKind.CrefParameterList:
                    break;
                case SyntaxKind.CrefBracketedParameterList:
                    break;
                case SyntaxKind.CrefParameter:
                    break;
                case SyntaxKind.IdentifierName:
                    break;
                case SyntaxKind.QualifiedName:
                    break;
                case SyntaxKind.GenericName:
                    break;
                case SyntaxKind.TypeArgumentList:
                    break;
                case SyntaxKind.AliasQualifiedName:
                    break;
                case SyntaxKind.PredefinedType:
                    break;
                case SyntaxKind.ArrayType:
                    break;
                case SyntaxKind.ArrayRankSpecifier:
                    break;
                case SyntaxKind.PointerType:
                    break;
                case SyntaxKind.NullableType:
                    break;
                case SyntaxKind.OmittedTypeArgument:
                    break;
                case SyntaxKind.ParenthesizedExpression:
                    break;
                case SyntaxKind.ConditionalExpression:
                    break;
                case SyntaxKind.InvocationExpression:
                    break;
                case SyntaxKind.ElementAccessExpression:
                    break;
                case SyntaxKind.ArgumentList:
                    break;
                case SyntaxKind.BracketedArgumentList:
                    break;
                case SyntaxKind.Argument:
                    break;
                case SyntaxKind.NameColon:
                    break;
                case SyntaxKind.CastExpression:
                    break;
                case SyntaxKind.AnonymousMethodExpression:
                    break;
                case SyntaxKind.SimpleLambdaExpression:
                    break;
                case SyntaxKind.ParenthesizedLambdaExpression:
                    break;
                case SyntaxKind.ObjectInitializerExpression:
                    break;
                case SyntaxKind.CollectionInitializerExpression:
                    break;
                case SyntaxKind.ArrayInitializerExpression:
                    break;
                case SyntaxKind.AnonymousObjectMemberDeclarator:
                    break;
                case SyntaxKind.ComplexElementInitializerExpression:
                    break;
                case SyntaxKind.ObjectCreationExpression:
                    break;
                case SyntaxKind.AnonymousObjectCreationExpression:
                    break;
                case SyntaxKind.ArrayCreationExpression:
                    break;
                case SyntaxKind.ImplicitArrayCreationExpression:
                    break;
                case SyntaxKind.StackAllocArrayCreationExpression:
                    break;
                case SyntaxKind.OmittedArraySizeExpression:
                    break;
                case SyntaxKind.InterpolatedStringExpression:
                    break;
                case SyntaxKind.ImplicitElementAccess:
                    break;
                case SyntaxKind.IsPatternExpression:
                    break;
                case SyntaxKind.RangeExpression:
                    break;
                case SyntaxKind.AddExpression:
                    break;
                case SyntaxKind.SubtractExpression:
                    break;
                case SyntaxKind.MultiplyExpression:
                    break;
                case SyntaxKind.DivideExpression:
                    break;
                case SyntaxKind.ModuloExpression:
                    break;
                case SyntaxKind.LeftShiftExpression:
                    break;
                case SyntaxKind.RightShiftExpression:
                    break;
                case SyntaxKind.LogicalOrExpression:
                    break;
                case SyntaxKind.LogicalAndExpression:
                    break;
                case SyntaxKind.BitwiseOrExpression:
                    break;
                case SyntaxKind.BitwiseAndExpression:
                    break;
                case SyntaxKind.ExclusiveOrExpression:
                    break;
                case SyntaxKind.EqualsExpression:
                    break;
                case SyntaxKind.NotEqualsExpression:
                    break;
                case SyntaxKind.LessThanExpression:
                    break;
                case SyntaxKind.LessThanOrEqualExpression:
                    break;
                case SyntaxKind.GreaterThanExpression:
                    break;
                case SyntaxKind.GreaterThanOrEqualExpression:
                    break;
                case SyntaxKind.IsExpression:
                    break;
                case SyntaxKind.AsExpression:
                    break;
                case SyntaxKind.CoalesceExpression:
                    break;
                case SyntaxKind.SimpleMemberAccessExpression:
                    break;
                case SyntaxKind.PointerMemberAccessExpression:
                    break;
                case SyntaxKind.ConditionalAccessExpression:
                    break;
                case SyntaxKind.MemberBindingExpression:
                    break;
                case SyntaxKind.ElementBindingExpression:
                    break;
                case SyntaxKind.SimpleAssignmentExpression:
                    break;
                case SyntaxKind.AddAssignmentExpression:
                    break;
                case SyntaxKind.SubtractAssignmentExpression:
                    break;
                case SyntaxKind.MultiplyAssignmentExpression:
                    break;
                case SyntaxKind.DivideAssignmentExpression:
                    break;
                case SyntaxKind.ModuloAssignmentExpression:
                    break;
                case SyntaxKind.AndAssignmentExpression:
                    break;
                case SyntaxKind.ExclusiveOrAssignmentExpression:
                    break;
                case SyntaxKind.OrAssignmentExpression:
                    break;
                case SyntaxKind.LeftShiftAssignmentExpression:
                    break;
                case SyntaxKind.RightShiftAssignmentExpression:
                    break;
                case SyntaxKind.CoalesceAssignmentExpression:
                    break;
                case SyntaxKind.UnaryPlusExpression:
                    break;
                case SyntaxKind.UnaryMinusExpression:
                    break;
                case SyntaxKind.BitwiseNotExpression:
                    break;
                case SyntaxKind.LogicalNotExpression:
                    break;
                case SyntaxKind.PreIncrementExpression:
                    break;
                case SyntaxKind.PreDecrementExpression:
                    break;
                case SyntaxKind.PointerIndirectionExpression:
                    break;
                case SyntaxKind.AddressOfExpression:
                    break;
                case SyntaxKind.PostIncrementExpression:
                    break;
                case SyntaxKind.PostDecrementExpression:
                    break;
                case SyntaxKind.AwaitExpression:
                    break;
                case SyntaxKind.IndexExpression:
                    break;
                case SyntaxKind.ThisExpression:
                    break;
                case SyntaxKind.BaseExpression:
                    break;
                case SyntaxKind.ArgListExpression:
                    break;
                case SyntaxKind.NumericLiteralExpression:
                    break;
                case SyntaxKind.StringLiteralExpression:
                    break;
                case SyntaxKind.CharacterLiteralExpression:
                    break;
                case SyntaxKind.TrueLiteralExpression:
                    break;
                case SyntaxKind.FalseLiteralExpression:
                    break;
                case SyntaxKind.NullLiteralExpression:
                    break;
                case SyntaxKind.DefaultLiteralExpression:
                    break;
                case SyntaxKind.TypeOfExpression:
                    break;
                case SyntaxKind.SizeOfExpression:
                    break;
                case SyntaxKind.CheckedExpression:
                    break;
                case SyntaxKind.UncheckedExpression:
                    break;
                case SyntaxKind.DefaultExpression:
                    break;
                case SyntaxKind.MakeRefExpression:
                    break;
                case SyntaxKind.RefValueExpression:
                    break;
                case SyntaxKind.RefTypeExpression:
                    break;
                case SyntaxKind.QueryExpression:
                    break;
                case SyntaxKind.QueryBody:
                    break;
                case SyntaxKind.FromClause:
                    break;
                case SyntaxKind.LetClause:
                    break;
                case SyntaxKind.JoinClause:
                    break;
                case SyntaxKind.JoinIntoClause:
                    break;
                case SyntaxKind.WhereClause:
                    break;
                case SyntaxKind.OrderByClause:
                    break;
                case SyntaxKind.AscendingOrdering:
                    break;
                case SyntaxKind.DescendingOrdering:
                    break;
                case SyntaxKind.SelectClause:
                    break;
                case SyntaxKind.GroupClause:
                    break;
                case SyntaxKind.QueryContinuation:
                    break;
                case SyntaxKind.Block:
                    break;
                case SyntaxKind.LocalDeclarationStatement:
                    break;
                case SyntaxKind.VariableDeclaration:
                    break;
                case SyntaxKind.VariableDeclarator:
                    break;
                case SyntaxKind.EqualsValueClause:
                    break;
                case SyntaxKind.ExpressionStatement:
                    break;
                case SyntaxKind.EmptyStatement:
                    break;
                case SyntaxKind.LabeledStatement:
                    break;
                case SyntaxKind.GotoStatement:
                    break;
                case SyntaxKind.GotoCaseStatement:
                    break;
                case SyntaxKind.GotoDefaultStatement:
                    break;
                case SyntaxKind.BreakStatement:
                    break;
                case SyntaxKind.ContinueStatement:
                    break;
                case SyntaxKind.ReturnStatement:
                    break;
                case SyntaxKind.YieldReturnStatement:
                    break;
                case SyntaxKind.YieldBreakStatement:
                    break;
                case SyntaxKind.ThrowStatement:
                    break;
                case SyntaxKind.WhileStatement:
                    break;
                case SyntaxKind.DoStatement:
                    break;
                case SyntaxKind.ForStatement:
                    break;
                case SyntaxKind.ForEachStatement:
                    break;
                case SyntaxKind.UsingStatement:
                    break;
                case SyntaxKind.FixedStatement:
                    break;
                case SyntaxKind.CheckedStatement:
                    break;
                case SyntaxKind.UncheckedStatement:
                    break;
                case SyntaxKind.UnsafeStatement:
                    break;
                case SyntaxKind.LockStatement:
                    break;
                case SyntaxKind.IfStatement:
                    break;
                case SyntaxKind.ElseClause:
                    break;
                case SyntaxKind.SwitchStatement:
                    break;
                case SyntaxKind.SwitchSection:
                    break;
                case SyntaxKind.CaseSwitchLabel:
                    break;
                case SyntaxKind.DefaultSwitchLabel:
                    break;
                case SyntaxKind.TryStatement:
                    break;
                case SyntaxKind.CatchClause:
                    break;
                case SyntaxKind.CatchDeclaration:
                    break;
                case SyntaxKind.CatchFilterClause:
                    break;
                case SyntaxKind.FinallyClause:
                    break;
                case SyntaxKind.LocalFunctionStatement:
                    break;
                case SyntaxKind.CompilationUnit:
                    break;
                case SyntaxKind.GlobalStatement:
                    break;
                case SyntaxKind.NamespaceDeclaration:
                    break;
                case SyntaxKind.UsingDirective:
                    break;
                case SyntaxKind.ExternAliasDirective:
                    break;
                case SyntaxKind.AttributeList:
                    break;
                case SyntaxKind.AttributeTargetSpecifier:
                    break;
                case SyntaxKind.Attribute:
                    break;
                case SyntaxKind.AttributeArgumentList:
                    break;
                case SyntaxKind.AttributeArgument:
                    break;
                case SyntaxKind.NameEquals:
                    break;
                case SyntaxKind.ClassDeclaration:
                    break;
                case SyntaxKind.StructDeclaration:
                    break;
                case SyntaxKind.InterfaceDeclaration:
                    break;
                case SyntaxKind.EnumDeclaration:
                    break;
                case SyntaxKind.DelegateDeclaration:
                    break;
                case SyntaxKind.BaseList:
                    break;
                case SyntaxKind.SimpleBaseType:
                    break;
                case SyntaxKind.TypeParameterConstraintClause:
                    break;
                case SyntaxKind.ConstructorConstraint:
                    break;
                case SyntaxKind.ClassConstraint:
                    break;
                case SyntaxKind.StructConstraint:
                    break;
                case SyntaxKind.TypeConstraint:
                    break;
                case SyntaxKind.ExplicitInterfaceSpecifier:
                    break;
                case SyntaxKind.EnumMemberDeclaration:
                    break;
                case SyntaxKind.FieldDeclaration:
                    break;
                case SyntaxKind.EventFieldDeclaration:
                    break;
                case SyntaxKind.MethodDeclaration:
                    break;
                case SyntaxKind.OperatorDeclaration:
                    break;
                case SyntaxKind.ConversionOperatorDeclaration:
                    break;
                case SyntaxKind.ConstructorDeclaration:
                    break;
                case SyntaxKind.BaseConstructorInitializer:
                    break;
                case SyntaxKind.ThisConstructorInitializer:
                    break;
                case SyntaxKind.DestructorDeclaration:
                    break;
                case SyntaxKind.PropertyDeclaration:
                    break;
                case SyntaxKind.EventDeclaration:
                    break;
                case SyntaxKind.IndexerDeclaration:
                    break;
                case SyntaxKind.AccessorList:
                    break;
                case SyntaxKind.GetAccessorDeclaration:
                    break;
                case SyntaxKind.SetAccessorDeclaration:
                    break;
                case SyntaxKind.AddAccessorDeclaration:
                    break;
                case SyntaxKind.RemoveAccessorDeclaration:
                    break;
                case SyntaxKind.UnknownAccessorDeclaration:
                    break;
                case SyntaxKind.ParameterList:
                    break;
                case SyntaxKind.BracketedParameterList:
                    break;
                case SyntaxKind.Parameter:
                    break;
                case SyntaxKind.TypeParameterList:
                    break;
                case SyntaxKind.TypeParameter:
                    break;
                case SyntaxKind.IncompleteMember:
                    break;
                case SyntaxKind.ArrowExpressionClause:
                    break;
                case SyntaxKind.Interpolation:
                    break;
                case SyntaxKind.InterpolatedStringText:
                    break;
                case SyntaxKind.InterpolationAlignmentClause:
                    break;
                case SyntaxKind.InterpolationFormatClause:
                    break;
                case SyntaxKind.ShebangDirectiveTrivia:
                    break;
                case SyntaxKind.LoadDirectiveTrivia:
                    break;
                case SyntaxKind.TupleType:
                    break;
                case SyntaxKind.TupleElement:
                    break;
                case SyntaxKind.TupleExpression:
                    break;
                case SyntaxKind.SingleVariableDesignation:
                    break;
                case SyntaxKind.ParenthesizedVariableDesignation:
                    break;
                case SyntaxKind.ForEachVariableStatement:
                    break;
                case SyntaxKind.DeclarationPattern:
                    break;
                case SyntaxKind.ConstantPattern:
                    break;
                case SyntaxKind.CasePatternSwitchLabel:
                    break;
                case SyntaxKind.WhenClause:
                    break;
                case SyntaxKind.DiscardDesignation:
                    break;
                case SyntaxKind.RecursivePattern:
                    break;
                case SyntaxKind.PropertyPatternClause:
                    break;
                case SyntaxKind.Subpattern:
                    break;
                case SyntaxKind.PositionalPatternClause:
                    break;
                case SyntaxKind.DiscardPattern:
                    break;
                case SyntaxKind.SwitchExpression:
                    break;
                case SyntaxKind.SwitchExpressionArm:
                    break;
                case SyntaxKind.VarPattern:
                    break;
                case SyntaxKind.DeclarationExpression:
                    break;
                case SyntaxKind.RefExpression:
                    break;
                case SyntaxKind.RefType:
                    break;
                case SyntaxKind.ThrowExpression:
                    break;
                case SyntaxKind.ImplicitStackAllocArrayCreationExpression:
                    break;
                case SyntaxKind.SuppressNullableWarningExpression:
                    break;
                case SyntaxKind.NullableDirectiveTrivia:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (token.ContainsDiagnostics)
            {
                    pp.SetBackgroundBrush(Brushes.LightSalmon);
                
            }
            // pp.SyntaxToken = symbolDisplayPart;
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