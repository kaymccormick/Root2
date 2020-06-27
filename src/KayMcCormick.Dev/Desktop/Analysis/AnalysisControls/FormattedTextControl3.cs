using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnalysisAppLib;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    public class InClassName
    {
        public InClassName(FormattedTextControl3 formattedTextControl3, int lineNo, int offset, double y, double x,
            LineInfo lineInfo, TextFormatter textFormatter, double paragraphWidth, FontRendering currentRendering,
            double pixelsPerDip, CustomTextSource4 customTextSource4, double maxY, double maxX, DrawingGroup d,
            DrawingContext dc)
        {
            FormattedTextControl3 = formattedTextControl3;
            LineNo = lineNo;
            Offset = offset;
            Y = y;
            X = x;
            LineInfo = lineInfo;
            TextFormatter = textFormatter;
            ParagraphWidth = paragraphWidth;
            CurrentRendering = currentRendering;
            PixelsPerDip = pixelsPerDip;
            CustomTextSource4 = customTextSource4;
            MaxY = maxY;
            MaxX = maxX;
            D = d;
            Dc = dc;
        }

        public FormattedTextControl3 FormattedTextControl3 { get; private set; }
        public int LineNo { get; private set; }
        public int Offset { get; private set; }
        public double Y { get; private set; }
        public double X { get; private set; }
        public LineInfo LineInfo { get; private set; }
        public TextFormatter TextFormatter { get; private set; }
        public double ParagraphWidth { get; private set; }
        public FontRendering CurrentRendering { get; set; }
        public double PixelsPerDip { get; private set; }
        public CustomTextSource4 CustomTextSource4 { get; private set; }
        public double MaxY { get; private set; }
        public double MaxX { get; private set; }
        public DrawingGroup D { get; private set; }
        public DrawingContext Dc { get; private set; }
        public string FontFamilyName { get; set; }
        public double FontSize { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
//    [TitleMetadata("Formatted Code Control")]
    public class FormattedTextControl3 : SyntaxNodeControl, ILineDrawer
    {
        /// <inheritdoc />
        protected override void OnDocumentChanged(Document oldValue, Document newValue)
        {
            base.OnDocumentChanged(oldValue, newValue);
        }

        /// <inheritdoc />
        protected override void OnModelChanged(SemanticModel oldValue, SemanticModel newValue)
        {
            base.OnModelChanged(oldValue, newValue);
            
        }

        public static readonly DependencyProperty InsertionPointProperty = DependencyProperty.Register(
            "InsertionPoint", typeof(int), typeof(FormattedTextControl3),
            new PropertyMetadata(default(int), OnInsertionPointChanged));

        public int InsertionPoint
        {
            get { return (int) GetValue(InsertionPointProperty); }
            set { SetValue(InsertionPointProperty, value); }
        }

        private static void OnInsertionPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FormattedTextControl3) d).OnInsertionPointChanged((int) e.OldValue, (int) e.NewValue);
        }


        protected virtual async void OnInsertionPointChanged(int oldValue, int newValue)
        {
            // var completionService = CompletionService.GetService(Document);
            // var results = await completionService.GetCompletionsAsync(Document, InsertionPoint);
            // foreach (var completionItem in results.Items)
            // {
                // DebugUtils.WriteLine(completionItem.DisplayText);
                // DebugUtils.WriteLine(completionItem.InlineDescription);
            // }
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverOffsetProperty = DependencyProperty.Register(
            "HoverOffset", typeof(int), typeof(FormattedTextControl3), new PropertyMetadata(default(int)));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverRegionInfoProperty = DependencyProperty.Register(
            "HoverRegionInfo", typeof(RegionInfo), typeof(FormattedTextControl3),
            new PropertyMetadata(default(RegionInfo)));

        /// <summary>
        /// 
        /// </summary>
        public RegionInfo HoverRegionInfo
        {
            get { return (RegionInfo) GetValue(HoverRegionInfoProperty); }
            set { SetValue(HoverRegionInfoProperty, value); }
        }

        /// <inheritdoc />
        public override void EndInit()
        {
            base.EndInit();
        }

        /// <summary>
        /// 
        /// </summary>
        public int HoverOffset
        {
            get { return (int) GetValue(HoverOffsetProperty); }
            set { SetValue(HoverOffsetProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverTokenProperty = DependencyProperty.Register(
            "HoverToken", typeof(SyntaxToken?), typeof(FormattedTextControl3),
            new PropertyMetadata(default(SyntaxToken?)));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverSymbolProperty = DependencyProperty.Register(
            "HoverSymbol", typeof(ISymbol), typeof(FormattedTextControl3), new PropertyMetadata(default(ISymbol)));

        /// <summary>
        /// 
        /// </summary>
        public ISymbol HoverSymbol
        {
            get { return (ISymbol) GetValue(HoverSymbolProperty); }
            set { SetValue(HoverSymbolProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxToken? HoverToken
        {
            get { return (SyntaxToken?) GetValue(HoverTokenProperty); }
            set { SetValue(HoverTokenProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverSyntaxNodeProperty = DependencyProperty.Register(
            "HoverSyntaxNode", typeof(SyntaxNode), typeof(FormattedTextControl3),
            new PropertyMetadata(default(SyntaxNode), new PropertyChangedCallback(OnHoverSyntaxNodeUpdated)));

        private static void OnHoverSyntaxNodeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue?.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxNode HoverSyntaxNode
        {
            get { return (SyntaxNode) GetValue(HoverSyntaxNodeProperty); }
            set { SetValue(HoverSyntaxNodeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverColumnProperty = DependencyProperty.Register(
            "HoverColumn", typeof(int), typeof(FormattedTextControl3), new PropertyMetadata(default(int)));

        /// <summary>
        /// 
        /// </summary>
        public int HoverColumn
        {
            get { return (int) GetValue(HoverColumnProperty); }
            set { SetValue(HoverColumnProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HoverRowProperty = DependencyProperty.Register(
            "HoverRow", typeof(int), typeof(FormattedTextControl3), new PropertyMetadata(default(int)));

        /// <summary>
        /// 
        /// </summary>
        public int HoverRow
        {
            get { return (int) GetValue(HoverRowProperty); }
            set { SetValue(HoverRowProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected FontRendering CurrentRendering
        {
            get
            {
                if (_currentRendering == null)
                    _currentRendering = FontRendering.CreateInstance(FontSize,
                        TextAlignment.Left,
                        null,
                        Brushes.Black,
                        Typeface);
                return _currentRendering;
            }
            private set { _currentRendering = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool UiLoaded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixelsPerDip"></param>
        /// <param name="tf"></param>
        /// <param name="st"></param>
        /// <param name="node"></param>
        /// <param name="compilation"></param>
        /// <param name="s1"></param>
        /// <param name="typefaceManager"></param>
        /// <returns></returns>
        protected CustomTextSource4 CreateAndInitTextSource(double pixelsPerDip,
            Typeface tf, SyntaxTree st, SyntaxNode node, Compilation compilation,
            [NotNull] SynchronizationContext synchContext, double fontSize)
        {
            if (synchContext == null) throw new ArgumentNullException(nameof(synchContext));

            if (st == null)
            {
                st = SyntaxFactory.ParseSyntaxTree("");
                node = st.GetRoot();
                compilation = null;
            }

            var textDecorationCollection = new TextDecorationCollection();
            var typeface = tf;
            var fontRendering = FontRendering.CreateInstance(fontSize,
                TextAlignment.Left, textDecorationCollection,
                Brushes.Black, typeface);
            var source = new CustomTextSource4(pixelsPerDip, fontRendering, new GenericTextRunProperties(
                fontRendering,
                PixelsPerDip), synchContext)
            {
                EmSize = fontSize,
                Compilation = compilation,
                Tree = st,
                Node = node
            };
            source.PropertyChanged += SourceOnPropertyChanged;
            source.Init();
            return source;
        }

        public SynchronizationContext SynchContext { get; set; }

        private async void SourceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                var textSourceText = CustomTextSource.Text.ToString();
                await Dispatcher.InvokeAsync(() => { TextSourceText = textSourceText; });
            }
        }

        public static readonly DependencyProperty TextSourceTextProperty = DependencyProperty.Register(
            "TextSourceText", typeof(string), typeof(FormattedTextControl3),
            new PropertyMetadata(default(string), OnTextSourceTextChanged));

        public string TextSourceText
        {
            get { return (string) GetValue(TextSourceTextProperty); }
            set { SetValue(TextSourceTextProperty, value); }
        }

        private static void OnTextSourceTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FormattedTextControl3) d).OnTextSourceTextChanged((string) e.OldValue, (string) e.NewValue);
        }


        protected virtual void OnTextSourceTextChanged(string oldValue, string newValue)
        {
        }

        private DrawingBrush _myDrawingBrush = new DrawingBrush();
        private DrawingGroup _textDest = new DrawingGroup();
        private Point _pos;

        /// <summary>
        /// 
        /// </summary>
        protected double MaxX { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected double MaxY { get; set; }


        // ReSharper disable once UnusedMember.Local
        private void UpdateCompilation(Compilation compilation)
        {
            HandleDiagnostics(compilation.GetDiagnostics());
        }

        private void HandleDiagnostics(ImmutableArray<Diagnostic> diagnostics)
        {
            foreach (var diagnostic in diagnostics)
            {
                DebugUtils.WriteLine(diagnostic.ToString(), DebugCategory.TextFormatting);
                MarkLocation(diagnostic.Location);
                if (diagnostic.Severity == DiagnosticSeverity.Error) Errors.Add(new DiagnosticError(diagnostic));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<CompilationError> Errors { get; } = new List<CompilationError>();

        private void MarkLocation(Location diagnosticLocation)
        {
            switch (diagnosticLocation.Kind)
            {
                case LocationKind.SourceFile:
                    if (diagnosticLocation.SourceTree == SyntaxTree)
                    {
                        // ReSharper disable once UnusedVariable
                        var s = diagnosticLocation.SourceSpan.Start;
                    }

                    break;
            }
        }

#if false
        protected override Size MeasureOverride(Size constraint)
        {
            _grid.Measure(constraint);
            var gridDesiredSize = _grid.DesiredSize;
            DebugUtils.WriteLine(gridDesiredSize.ToString());
            return gridDesiredSize;
            DebugUtils.WriteLine(constraint.ToString(), DebugCategory.TextFormatting);
            return base.MeasureOverride(constraint);
            return new Size(max_x, _pos.Y);
        }
#endif

        static FormattedTextControl3()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FormattedTextControl3),
                new FrameworkPropertyMetadata(typeof(FormattedTextControl3)));
            // TextElement.FontFamilyProperty.OverrideMetadata(typeof(FormattedTextControl3), new PropertyMetadata(null, PropertyChangedCallback));
            // TextElement.FontSizeProperty.OverrideMetadata(typeof(FormattedTextControl3), new PropertyMetadata(16.0, PropertyChangedCallback2));
        }

        private static async void PropertyChangedCallback2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugUtils.WriteLine(e.NewValue.ToString());
            //    await ((FormattedTextControl3)d).UpdateTextSource();
        }

        private static async void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //   await ((FormattedTextControl3) d).UpdateTextSource();
        }

        /// <summary>
        /// 
        /// </summary>
        public FormattedTextControl3()

        {
            var xx = new DoubleAnimationUsingPath();
            _x1 = new ObjectAnimationUsingKeyFrames();
            _x1.RepeatBehavior = RepeatBehavior.Forever;
            _x1.Duration = new Duration(TimeSpan.FromSeconds(1));
            DebugUtils.WriteLine(_x1.Duration.ToString());

            var c = new ObjectKeyFrameCollection();
            c.Add(new DiscreteObjectKeyFrame(Visibility.Visible));
            c.Add(new DiscreteObjectKeyFrame(Visibility.Hidden, KeyTime.FromPercent(.6)));
            c.Add(new DiscreteObjectKeyFrame(Visibility.Visible, KeyTime.FromPercent(.4)));
            _x1.KeyFrames = c;


            CSharpCompilationOptions = new CSharpCompilationOptions(default(OutputKind));
            PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            CommandBindings.Add(new CommandBinding(WpfAppCommands.SerializeContents, Executed));
            CommandBindings.Add(new CommandBinding(WpfAppCommands.Compile, CompileExecuted));
            CommandBindings.Add(new CommandBinding(EditingCommands.EnterLineBreak, OnEnterLineBreak,
                CanEnterLineBreak));

            ;
            InputBindings.Add(new KeyBinding(EditingCommands.EnterLineBreak, Key.Enter, ModifierKeys.None));
            TypefaceManager = new DefaultTypefaceManager();
        }

        private void CompileExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (Document != null)
                Document.Project.GetCompilationAsync().ContinueWith(task =>
                {
                    Dispatcher.InvokeAsync(() => Compilation = task.Result);
                });
        }

        // public static RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent(typeof(FormattedTextControl3))

        private async void OnEnterLineBreak(object sender, ExecutedRoutedEventArgs e)
        {
            var b = await DoInput("\r\n");
            if (!b)
                DebugUtils.WriteLine("Newline failed");
            // ChangingText = true;
            // DebugUtils.WriteLine("Enter line break");
            // InsertionPoint = TextSource.EnterLineBreak(InsertionPoint);

            // SourceText += "\r\n";
            // ChangingText = false;
        }

        private void CanEnterLineBreak(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private async void Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var text = await SyntaxTree.GetTextAsync();
            var root = await SyntaxTree.GetRootAsync();
            using (var s = new FileStream(@"C:\temp\serialize.bin", FileMode.Create))

            {
                root.SerializeTo(s);
            }
        }

        /// <inheritdoc />
        protected override void OnSyntaxTreeUpdated(SyntaxTree newValue)
        {
            base.OnSyntaxTreeUpdated(newValue);
            _text = newValue.GetText();
            if (UpdatingSourceText)
                SourceText = _text.ToString();
        }

        /// <inheritdoc />
        protected override async void OnSourceTextChanged1(string newValue, string eOldValue)
        {
            base.OnSourceTextChanged1(newValue, eOldValue);

            if (newValue != null && !ChangingText) await UpdateTextSource();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task UpdateTextSource()
        {
            if (!UiLoaded)
                return;
            if (Compilation != null && Compilation.SyntaxTrees.Contains(SyntaxTree) == false)
            {
                throw new InvalidOperationException();
                Compilation = null;
                DebugUtils.WriteLine("Compilation does not contain syntax tree.");
            }

            if (Node == null || SyntaxTree == null) return;
            if (ReferenceEquals(Node.SyntaxTree, SyntaxTree) == false)
                throw new AppInvalidOperationException("Node is not within syntax tree");
            DebugUtils.WriteLine("Creating new " + nameof(SyntaxNodeCustomTextSource), DebugCategory.TextFormatting);

            //_errorTextSource = Errors.Any() ? new ErrorsTextSource(PixelsPerDip, Errors, TypefaceManager) : null;
            //_baseProps = TextSource.BaseProps;
            await UpdateFormattedText();
        }

        /// <summary>
        /// 
        /// </summary>
        private double PixelsPerDip { get; }

        private GeometryDrawing _geometryDrawing;
        private Rect _rect;

        /// <summary>
        /// 
        /// </summary>
        /// <inheritdoc />
        public override async void OnApplyTemplate()
        {
            _scrollViewer = (ScrollViewer) GetTemplateChild("ScrollViewer");
            if (_scrollViewer != null) OutputWidth = _scrollViewer.ActualWidth;
            DebugUtils.WriteLine(OutputWidth.ToString());
            CodeControl = (FormattedTextControl3) GetTemplateChild("CodeControl");
            _rectangle = (Rectangle) GetTemplateChild("Rectangle");
            var dpd = DependencyPropertyDescriptor.FromProperty(TextElement.FontSizeProperty, typeof(Rectangle));
            var dpd2 = DependencyPropertyDescriptor.FromProperty(TextElement.FontFamilyProperty, typeof(Rectangle));

            if (_rectangle != null)
            {
                // dpd.AddValueChanged(_rectangle, Handler);
                // dpd2.AddValueChanged(_rectangle, Handler2);
            }

            _grid = (Grid) GetTemplateChild("Grid");
            _canvas = (Canvas) GetTemplateChild("Canvas");
            _innerGrid = (Grid) GetTemplateChild("InnerGrid");
            // var tryGetGlyphTypeface = Typeface.TryGetGlyphTypeface(out var gf);

            _textCaret = new TextCaret(20);


            _canvas.Children.Add(_textCaret);


            _border = (Border) GetTemplateChild("Border");
            _myDrawingBrush = (DrawingBrush) GetTemplateChild("DrawingBrush");

            _textDest = (DrawingGroup) GetTemplateChild("TextDest");
            _rect2 = (Rectangle) GetTemplateChild("Rect2");
            _dg2 = (DrawingGroup) GetTemplateChild("DG2");
            UiLoaded = true;

            var t = new ThreadStart(SecondaryThread);
            Thread newWindowThread = new Thread(t);
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
            //if (TextSource != null) UpdateFormattedText();
        }

        private void SecondaryThread()
        {
            var d = Dispatcher.CurrentDispatcher;
            Dispatcher.Invoke(() =>
            {
                SecondaryDispatcher = d;
            });
            System.Windows.Threading.Dispatcher.Run();
        }

        public Dispatcher SecondaryDispatcher
        {
            get { return _secondaryDispatcher; }
            set
            {
                _secondaryDispatcher = value;
                if (_secondaryDispatcher != null)
                    DoUpdateTextSource();
            }
        }

        private async Task DoUpdateTextSource()
        {
            await UpdateTextSource();
        }


        public FormattedTextControl3 CodeControl { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public LineInfo InsertionLine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CharacterCell InsertionCharacter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RegionInfo InsertionRegion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            switch (e.Key)
            {
                case Key.Left:
                {
                    var ip = --InsertionPoint;
                    if (ip < 0) ip = 0;
                    DebugUtils.WriteLine($"{ip}");

                    if (InsertionCharacter != null)
                    {
                        var newc = InsertionCharacter.PreviousCell;
                        if (newc?.Region != InsertionRegion)
                        {
                            InsertionRegion = newc.Region;
                            if (newc.Region.Line != InsertionLine) InsertionLine = newc.Region.Line;
                        }

                        InsertionCharacter = newc;
                    }

                    var top = InsertionLine.Origin.Y;
                    DebugUtils.WriteLine("Setting top to " + top, DebugCategory.TextFormatting);

                    _textCaret.SetValue(Canvas.TopProperty, top);
                    if (InsertionCharacter != null)
                        _textCaret.SetValue(Canvas.LeftProperty, InsertionCharacter.Bounds.Left);
                }
                    break;
                case Key.Right:
                {
                    DebugUtils.WriteLine("incrementing insertion point", DebugCategory.TextFormatting);
                    e.Handled = true;
                    if (InsertionCharacter != null && InsertionCharacter.NextCell == null)
                        break;
                    var ip = ++InsertionPoint;
                    DebugUtils.WriteLine($"Insertion point: {ip}");

                    var newc = InsertionCharacter.NextCell;
                    if (newc.Region != null && newc.Region != InsertionRegion)
                    {
                        InsertionRegion = newc.Region;
                        if (newc.Region.Line != InsertionLine) InsertionLine = newc.Region.Line;
                    }

                    InsertionCharacter = newc;

                    var top = InsertionLine.Origin.Y;
                    DebugUtils.WriteLine("Setting top to " + top, DebugCategory.TextFormatting);

                    _textCaret.SetValue(Canvas.TopProperty, top);
                    _textCaret.SetValue(Canvas.LeftProperty, InsertionCharacter.Bounds.Left);


                    break;
                }
            }
        }

        /// <inheritdoc />
        protected override async void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            var eText = e.Text;
            e.Handled = true;
            try
            {
                await DoInput(eText);
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
            }
        }

        public async Task<bool> DoInput(string eText)
        {
            try
            {
                DebugUtils.WriteLine(eText);
                //  if (_textDest.Children.Count == 0) _textDest.Children.Add(new DrawingGroup());

                var insertionPoint = InsertionPoint;
                var prev = SourceText.Substring(0, insertionPoint);
                var next = SourceText.Substring(insertionPoint);
                var code = prev + eText + next;
                if (InsertionLine != null)
                {
#if false
                var l = InsertionLine.Text.Substring(0, InsertionPoint - InsertionLine.Offset) + e.Text;
                var end = InsertionLine.Offset + InsertionLine.Length;
                if (end - InsertionPoint > 0)
                {
                    var start = InsertionPoint - InsertionLine.Offset;
                    var length = end - InsertionPoint;
                    if (start + length > InsertionLine.Text.Length)
                        length = length - (start + length - InsertionLine.Text.Length);
                    l += InsertionLine.Text.Substring(start, length);
                }
#endif
                }

                if (InsertionLine != null && InsertionLine.LineNumber == 1)
                {
                }

                ChangingText = true;
                var insertionLineOffset = InsertionLine?.Offset ?? 0;
                var originY = InsertionLine?.Origin.Y ?? 0;
                var originX = InsertionLine?.Origin.X ?? 0;
                var insertionLineLineNumber = InsertionLine?.LineNumber ?? 0;
                var insertionLine = InsertionLine;

                var l = new List<LineInfo>();
              

                var d = new DrawingGroup();
                var drawingContext = d.Open();
                var typefaceName = FontFamily.FamilyNames[XmlLanguage.GetLanguage("en-US")];
                ;
                var inn = new InClassName(this, insertionLineLineNumber, insertionLineOffset, originY, originX,
                    insertionLine, Formatter, OutputWidth, null, PixelsPerDip, CustomTextSource, MaxY, MaxX,
                    d, drawingContext) {FontSize = FontSize, FontFamilyName = typefaceName};
                var lineInfo = await SecondaryDispatcher.InvokeAsync(
                    new Func<LineInfo>(() => Callback(inn, insertionPoint, eText)),
                    DispatcherPriority.Send, CancellationToken.None);
                await Dispatcher.InvokeAsync(() =>
                {
                    if (lineInfo == null) throw new InvalidOleVariantTypeException();

                    DebugUtils.WriteLine("No customTextSource");
                    InsertionLine = (LineInfo) lineInfo;

                    InsertionPoint = insertionPoint + eText.Length;
                    if (InsertionLine.Offset + InsertionLine.Length <= insertionPoint)
                    {
                        if (InsertionLine.NextLine != null)
                        {
                            InsertionLine = InsertionLine.NextLine;
                        }
                        else
                        {
                            InsertionLine.NextLine = new LineInfo()
                            {
                                LineNumber = InsertionLine.LineNumber + 1, PrevLine = InsertionLine,
                                Origin = new Point(0, InsertionLine.Origin.Y + InsertionLine.Height),
                                Offset = InsertionLine.Offset + InsertionLine.Length
                            };
                            InsertionLine = InsertionLine.NextLine;
                        }
                    }

                    if (eText.Length == 1)
                    {
                        //_textCaret.SetValue(Canvas.LeftProperty, 0);
                    }

                    //AdvanceInsertionPoint(e.Text.Length);

                    DebugUtils.WriteLine("About to update source text", DebugCategory.TextFormatting);
                    SourceText = code;
                    DebugUtils.WriteLine("Done updating source text", DebugCategory.TextFormatting);
                    ChangingText = false;
                });
                return true;
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
                return false;
            }
        }

        private LineInfo Callback(InClassName inn, int insertionPoin, string eText)
        {
            try
            {
                inn.CurrentRendering = FontRendering.CreateInstance(inn.FontSize, TextAlignment.Left,
                    new TextDecorationCollection(), Brushes.Black,
                    new Typeface(new FontFamily(inn.FontFamilyName), FontStyles.Normal, FontWeights.Normal,
                        FontStretches.Normal));
                CustomTextSource.TextInput(insertionPoin, eText);

                var lineInfo = RedrawLine((InClassName) inn);
                return lineInfo;
            }
            catch (Exception ex)

            {
                DebugUtils.WriteLine(ex.ToString());
            }

            return null;
        }


        /// <inheritdoc />
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            _textCaret.BeginAnimation(VisibilityProperty, _x1);
        }

        /// <inheritdoc />
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
            _textCaret.BeginAnimation(VisibilityProperty, null);
        }

        private static LineInfo RedrawLine(InClassName inClassName)
        {
            //if (formattedTextControl3.LineInfos.Count == 0) formattedTextControl3.LineInfos.Add(null);
            LineContext lineCtx;
            LineInfo outLineInfo;
            using (var myTextLine = inClassName.TextFormatter.FormatLine(inClassName.CustomTextSource4,
                inClassName.Offset, inClassName.ParagraphWidth,
                new GenericTextParagraphProperties(inClassName.CurrentRendering, inClassName.PixelsPerDip), null))
            {
                lineCtx = new LineContext()
                {
                    LineNumber = inClassName.LineNo,
                    CurCellRow = inClassName.LineNo,
                    LineInfo = inClassName.LineInfo,
                    LineOriginPoint = new Point(inClassName.X, inClassName.Y),
                    MyTextLine = myTextLine,
                    MaxX = inClassName.MaxX,
                    MaxY = inClassName.MaxY,
                    TextStorePosition = inClassName.Offset
                };

                inClassName.Dc.Dispatcher.Invoke(() =>
                {
                    myTextLine.Draw(inClassName.Dc, lineCtx.LineOriginPoint, InvertAxes.None);
                });
                var regions = new List<RegionInfo>();
                FormattingHelper.HandleTextLine(regions, ref lineCtx, out var lineI, inClassName.FormattedTextControl3);

                inClassName.FormattedTextControl3.Dispatcher.Invoke(() =>
                {
                    if (inClassName.FormattedTextControl3.LineInfos.Count <= inClassName.LineNo)
                        inClassName.FormattedTextControl3.LineInfos.Add(lineI);
                    else
                        inClassName.FormattedTextControl3.LineInfos[inClassName.LineNo] = lineI;
                });
                outLineInfo = lineI;
            }

            DebugUtils.WriteLine(
                $"{inClassName.FormattedTextControl3._rect.Width}x{inClassName.FormattedTextControl3._rect.Height}",
                DebugCategory.TextFormatting);

            inClassName.FormattedTextControl3.Dispatcher.Invoke(() =>
            {
                inClassName.Dc.Close();
                if (inClassName.FormattedTextControl3._textDest.Children.Count <= inClassName.LineNo)
                    inClassName.FormattedTextControl3._textDest.Children.Add(inClassName.D);
                else
                    inClassName.FormattedTextControl3._textDest.Children[inClassName.LineNo] = inClassName.D;

                inClassName.FormattedTextControl3.MaxX = lineCtx.MaxX;
                inClassName.FormattedTextControl3.MaxY = lineCtx.MaxY;
                inClassName.FormattedTextControl3._rectangle.Width = lineCtx.MaxX;
                inClassName.FormattedTextControl3._rectangle.Height = lineCtx.MaxY;
                inClassName.FormattedTextControl3._rect2.Width = lineCtx.MaxX;
                inClassName.FormattedTextControl3._rect2.Height = lineCtx.MaxY;
                inClassName.FormattedTextControl3.InvalidateVisual();
            });

            return outLineInfo;
        }

        private void AdvanceInsertionPoint(int textLength)
        {
            InsertionPoint += textLength;
        }


        /// <summary>
        /// 
        /// </summary>
        private Typeface CreateTypeface(FontFamily fontFamily, FontStyle fontStyle, FontStretch fontStretch,
            FontWeight fontWeight)
        {
            return new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
        }

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
            DebugUtils.WriteLine($"{newTemplate}", DebugCategory.TextFormatting);
        }

        private void OnPropertyChangedz(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            DebugUtils.WriteLine($"{e.Property.Name}");
            if (e.Property.Name == "DesignerView1")
            {
                DebugUtils.WriteLine($"{e.Property.Name} {e.OldValue} = {e.NewValue}", DebugCategory.TextFormatting);
                foreach (var m in e.NewValue.GetType().GetMethods())
                    DebugUtils.WriteLine(m.ToString(), DebugCategory.TextFormatting);
                foreach (var ii in e.NewValue.GetType().GetInterfaces())
                    DebugUtils.WriteLine(ii.ToString(), DebugCategory.TextFormatting);
            }
            else if (e.Property.Name == "InstanceBuilderContext")
            {
                DebugUtils.WriteLine($"{e.Property.Name} {e.OldValue} = {e.NewValue}", DebugCategory.TextFormatting);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected TextFormatter Formatter { get; set; } = TextFormatter.Create();

        /// <inheritdoc />
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _nliens = (int) (arrangeBounds.Height / (FontFamily.LineSpacing * FontSize));
            DebugUtils.WriteLine(_nliens.ToString());
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual async Task UpdateFormattedText()
        {
            try
            {
                Geometries.Clear();
                GeoTuples.Clear();

                // Make sure all UI is loaded
                if (!UiLoaded)
                    return;

                var textStorePosition = 0;
                var linePosition = new Point(0, 0);

                // Create a DrawingGroup object for storing formatted text.

                _textDest.Children.Clear();


                // Format each line of text from the text store and draw it.
                TextLineBreak prev = null;
                LineInfo prevLine = null;
                CharacterCell prevCell = null;
                RegionInfo prevRegion = null;
                var line = 0;
                if (_nliens == 0) _nliens = 10;

                DebugUtils.WriteLine("Calling innser updatE");
                var compilation = Compilation;

                var node0 = Node;
                var tree = SyntaxTree;
                _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                var fontFamilyFamilyName = FontFamily.FamilyNames[XmlLanguage.GetLanguage("en-US")];
                DebugUtils.WriteLine(fontFamilyFamilyName);
                var emSize = FontSize;
                var source = await SecondaryDispatcher.InvokeAsync(() =>
                    {
                        return InnerUpdate(this, textStorePosition, prev, prevLine, line, linePosition, prevCell,
                            prevRegion,
                            Formatter, OutputWidth, PixelsPerDip, emSize, tree, node0, compilation,
                            fontFamilyFamilyName);
                    }).Task
                    .ContinueWith(
                        task =>
                        {
                            if (task.IsFaulted) DebugUtils.WriteLine(task.Exception.ToString());
                            return task.Result;
                        }).ConfigureAwait(true);
                _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                CustomTextSource = source;
                DebugUtils.WriteLine("return from Calling innser updatE");
                ;
                // Persist the drawn text content.

                _rectangle.Width = MaxX;
                _rectangle.Height = _pos.Y;

                // InsertionCharacter = LineInfos[0].Regions[0].Characters[0];
                UpdateCaretPosition();
                InvalidateVisual();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CustomTextSource4 CustomTextSource
        {
            get { return _customTextSource; }
            set { _customTextSource = value; }
        }

        private static CustomTextSource4 InnerUpdate(FormattedTextControl3 formattedTextControl3, int textStorePosition,
            TextLineBreak prev, LineInfo prevLine, int line, Point linePosition,
            CharacterCell prevCell, RegionInfo prevRegion,
            TextFormatter textFormatter, double paragraphWidth, double pixelsPerDip,
            double emSize0, SyntaxTree tree, SyntaxNode node0, Compilation compilation, string faceName)
        {
            var s1 = new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher);
            if (s1 == null) throw new InvalidOperationException("no synchh context");
            var tf = formattedTextControl3.CreateTypeface(new FontFamily(faceName), FontStyles.Normal,
                FontStretches.Normal,
                FontWeights.Normal);

            var CurrentRendering1 = FontRendering.CreateInstance(emSize0,
                TextAlignment.Left,
                null,
                Brushes.Black,
                tf);
            var customTextSource4 =
                formattedTextControl3.CreateAndInitTextSource(pixelsPerDip, tf, tree, node0, compilation, s1, emSize0);
            var chars = new List<List<char>>();
            while (textStorePosition < customTextSource4.Length)
            {
                var genericTextParagraphProperties =
                    new GenericTextParagraphProperties(CurrentRendering1, pixelsPerDip);
                using (var myTextLine = textFormatter.FormatLine(customTextSource4,
                    textStorePosition, paragraphWidth,
                    genericTextParagraphProperties,
                    prev))
                {
                    var lineChars = new List<char>();
                    chars.Add(lineChars);

                    var lineInfo = new LineInfo {Offset = textStorePosition, Length = myTextLine.Length};
                    lineInfo.PrevLine = prevLine;
                    lineInfo.LineNumber = line;

                    if (prevLine != null) prevLine.NextLine = lineInfo;

                    prevLine = lineInfo;
                    lineInfo.Size = new Size(myTextLine.WidthIncludingTrailingWhitespace, myTextLine.Height);
                    lineInfo.Origin = new Point(linePosition.X, linePosition.Y);

                    var location = linePosition;
                    var group = 0;

                    var textRunSpans = myTextLine.GetTextRunSpans();
                    var spans = textRunSpans;
                    var cell = linePosition;
                    var cellColumn = 0;
                    var characterOffset = textStorePosition;
                    var regionOffset = textStorePosition;
                    var eol = myTextLine.GetTextRunSpans().Select(xx => xx.Value).OfType<TextEndOfLine>();
                    if (eol.Any())
                    {
                        // dc.DrawRectangle(Brushes.Aqua, null,
                        // new Rect(linePosition.X + myTextLine.WidthIncludingTrailingWhitespace + 2,
                        // linePosition.Y + 2, 10, 10));
                    }
                    else
                    {
                        DebugUtils.WriteLine("no end of line", DebugCategory.TextFormatting);
                        foreach (var textRunSpan in myTextLine.GetTextRunSpans())
                            DebugUtils.WriteLine(textRunSpan.Value.ToString(), DebugCategory.TextFormatting);
                    }

                    var lineRegions = new List<RegionInfo>();

                    var lineString = "";
                    var xoffset = lineInfo.Origin.X;
                    var xoffsets = new List<double>();

                    var curOffset = linePosition;
                    foreach (var rect in myTextLine.GetIndexedGlyphRuns())
                    {
                        var rectGlyphRun = rect.GlyphRun;

                        if (rectGlyphRun != null)
                        {
                            var size = new Size(0, 0);
                            var cellBounds =
                                new List<CharacterCell>();
                            var emSize = rectGlyphRun.FontRenderingEmSize;


                            if (rectGlyphRun.Characters.Count > rectGlyphRun.GlyphIndices.Count)
                                DebugUtils.WriteLine($"Character mismatch");

                            var xx = new RectangleGeometry(new Rect(curOffset,
                                new Size(rectGlyphRun.AdvanceWidths.Sum(),
                                    rectGlyphRun.GlyphTypeface.Height * rectGlyphRun.FontRenderingEmSize)));
                            curOffset.Y += myTextLine.Height;
                            var x = new CombinedGeometry();

                            for (var i = 0; i < rectGlyphRun.GlyphIndices.Count; i++)
                            {
                                var advanceWidth = rectGlyphRun.AdvanceWidths[i];

                                xoffsets.Add(xoffset);
                                xoffset += advanceWidth;
                                size.Width += advanceWidth;
                                var gi = rectGlyphRun.GlyphIndices[i];
                                var c = rectGlyphRun.Characters[i];
                                lineChars.Add(c);
                                lineString += c;
                                var advWidth = rectGlyphRun.GlyphTypeface.AdvanceWidths[gi];
                                var advHeight = rectGlyphRun.GlyphTypeface.AdvanceHeights[gi];

                                var s = new Size(advWidth * emSize,
                                    (advHeight
                                     + rectGlyphRun.GlyphTypeface.BottomSideBearings[gi])
                                    * emSize);

                                var topSide = rectGlyphRun.GlyphTypeface.TopSideBearings[gi];
                                var bounds = new Rect(new Point(cell.X, cell.Y + topSide), s);
                                if (!bounds.IsEmpty)
                                {
                                    // ReSharper disable once UnusedVariable
                                    var glyphTypefaceBaseline = rectGlyphRun.GlyphTypeface.Baseline;
                                    //DebugUtils.WriteLine(glyphTypefaceBaseline.ToString(), DebugCategory.TextFormatting);
                                    //bounds.Offset(cell.X, cell.Y + glyphTypefaceBaseline);
                                    // dc.DrawRectangle(Brushes.White, null,  bounds);
                                    // dc.DrawText(
                                    // new FormattedText(cellColumn.ToString(), CultureInfo.CurrentCulture,
                                    // FlowDirection.LeftToRight, new Typeface("Arial"), _emSize * .66, Brushes.Aqua,
                                    // new NumberSubstitution(), _pixelsPerDip), new Point(bounds.Left, bounds.Top));
                                }

                                var char0 = new CharacterCell(bounds, new Point(cellColumn, chars.Count - 1), c)
                                {
                                    PreviousCell = prevCell
                                };

                                if (prevCell != null)
                                    prevCell.NextCell = char0;
                                prevCell = char0;

                                cellBounds.Add(char0);
                                cell.Offset(rectGlyphRun.AdvanceWidths[i], 0);

                                cellColumn++;
                                characterOffset++;
                                //                                _textDest.Children.Add(new GeometryDrawing(null, new Pen(Brushes.DarkOrange, 2), new RectangleGeometry(bounds)));
                            }

                            //var bb = rect.GlyphRun.BuildGeometry().Bounds;

                            size.Height += myTextLine.Height;
                            var r = new Rect(location, size);
                            location.Offset(size.Width, 0);
//                            dc.DrawRectangle(null, new Pen(Brushes.Green, 1), r);
                            //rects.Add(r);
                            if (@group < spans.Count)
                            {
                                var textSpan = spans[@group];
                                var textSpanValue = textSpan.Value;
                                SyntaxNode node = null;
                                SyntaxToken? token = null;
                                SyntaxTrivia? trivia = null;
                                SyntaxToken? AttachedToken = null;
                                SyntaxNode attachedNode = null;

                                SyntaxNode structuredTrivia = null;
                                TriviaPosition? triviaPosition = null;
                                if (textSpanValue is SyntaxTokenTextCharacters stc)
                                {
                                    node = stc.Node;
                                    token = stc.Token;
                                }
                                else
                                {
                                    if (textSpanValue is SyntaxTriviaTextCharacters stc2)
                                    {
                                        trivia = stc2.Trivia;
                                        AttachedToken = stc2.Token;
                                        attachedNode = stc2.Node;
                                        structuredTrivia = stc2.StructuredTrivia;
                                        triviaPosition = stc2.TriviaPosition;
                                    }
                                }

                                var tuple = new RegionInfo(textSpanValue, r, cellBounds)
                                {
                                    Line = lineInfo,
                                    Offset = regionOffset,
                                    Length = textSpan.Length,
                                    SyntaxNode = node,
                                    AttachedToken = AttachedToken,
                                    AttachedNode = attachedNode,
                                    SyntaxToken = token,
                                    Trivia = trivia,
                                    TriviaPosition = triviaPosition,
                                    PrevRegion = prevRegion,
                                    StructuredTrivia = structuredTrivia
                                };
                                foreach (var ch in tuple.Characters) ch.Region = tuple;
                                lineRegions.Add(tuple);

                                //formattedTextControl3.GeoTuples.Add(Tuple.Create(xx, tuple));

                                if (prevRegion != null) prevRegion.NextRegion = tuple;
                                prevRegion = tuple;
                                // Infos.Add(tuple);
                            }

                            @group++;
                            regionOffset = characterOffset;
                        }

                        lineInfo.Text = lineString;
                        lineInfo.Regions = lineRegions;
                        //                        DebugUtils.WriteLine(rect.ToString(), DebugCategory.TextFormatting);
                        //dc.DrawRectangle(null, new Pen(Brushes.Green, 1), r1);
                    }


                    //DebugUtils.WriteLine(line.ToString() + ddBounds.ToString(), DebugCategory.TextFormatting);
                    //dc.DrawRectangle(null, new Pen(Brushes.Red, 1), ddBounds);

                    // Draw the formatted text into the drawing context.
                    var p = new Point(linePosition.X + myTextLine.WidthIncludingTrailingWhitespace, linePosition.Y);
                    var w = myTextLine.Width;
                    linePosition.Y += myTextLine.Height;
                    formattedTextControl3.Dispatcher.Invoke(() =>
                    {
                        if (w >= formattedTextControl3.MaxX) formattedTextControl3.MaxX = w;
                        var dc = formattedTextControl3._textDest.Append();
                        myTextLine.Draw(dc, linePosition, InvertAxes.None);
                        dc.Close();

                        formattedTextControl3._rectangle.Width = formattedTextControl3.MaxX;
                        formattedTextControl3._rectangle.Height = formattedTextControl3._pos.Y;
                        formattedTextControl3.LineInfos.Add(lineInfo);
                    });
                    // ReSharper disable once UnusedVariable

                    var textLineBreak = myTextLine.GetTextLineBreak();
                    if (textLineBreak != null)
                        DebugUtils.WriteLine(textLineBreak.ToString(), DebugCategory.TextFormatting);
                    line++;

                    prev = textLineBreak;
                    if (prev != null) DebugUtils.WriteLine("Line break!", DebugCategory.TextFormatting);

                    // Update the index position in the text store.
                    textStorePosition += myTextLine.Length;
                    // Update the line position coordinate for the displayed line.
                }

                formattedTextControl3.Dispatcher.Invoke(() => { formattedTextControl3._pos = linePosition; });
            }

            return customTextSource4;
        }

        public GeometryCollection Geometries { get; set; } = new GeometryCollection(200);

        private void UpdateCaretPosition()
        {
            var insertionPoint = InsertionPoint;
            var l0 = LineInfos.FirstOrDefault(l => l.Offset + l.Length >= insertionPoint);
            if (l0 != null)
            {
                InsertionLine = l0;
                _textCaret.SetValue(Canvas.TopProperty, l0.Origin.Y);
                var rr = l0.Regions.FirstOrDefault(r => r.Offset + r.Length >= insertionPoint);
                InsertionRegion = rr;
                if (rr != null)
                {
                    var ch = rr.Characters[insertionPoint - rr.Offset];
                    InsertionCharacter = ch;
                    var x = ch.Bounds.Right - ch.Bounds.Width / 2;
                    _textCaret.SetValue(Canvas.LeftProperty, x);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double OutputWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<LineInfo> LineInfos { get; } = new ObservableCollection<LineInfo>();

        // ReSharper disable once NotAccessedField.Local
        private Border _border;

        // ReSharper disable once NotAccessedField.Local
        private Grid _grid;
        private Rectangle _rectangle;
        private ScrollViewer _scrollViewer;
        private GenericTextRunProperties _baseProps;

        /// <summary>
        /// 
        /// </summary>
        public Typeface Typeface { get; protected set; }

        public ITypefaceManager TypefaceManager
        {
            get { return _typefaceManager; }
            set { _typefaceManager = value; }
        }

        private FontFamily _fontFamily;
        private readonly FontStyle _fontStyle = FontStyles.Normal;
        private readonly FontWeight _fontWeight = FontWeights.Normal;
        private readonly FontStretch _fontStretch = FontStretches.Normal;

        // ReSharper disable once NotAccessedField.Local
        private ErrorsTextSource _errorTextSource;

        // ReSharper disable once NotAccessedField.Local
        private int _startColumn;

        // ReSharper disable once NotAccessedField.Local
        private int _startRow;
        private int _startOffset;
        private DrawingGroup _selectionGeometry;
        private Rectangle _rect2;
        private DrawingGroup _dg2;
        private Grid _innerGrid;
        private TextCaret _textCaret;
        private Canvas _canvas;
        private FontRendering _currentRendering;
        private CustomTextSource4 _store;
        private ITypefaceManager _typefaceManager;
        private readonly DocumentPaginator _documentPaginator;
        private int _selectionEnd;
        private SyntaxNode _startNode;
        private SyntaxNode _endNode;
        private Rectangle geometryRectangle = new Rectangle();
        private SourceText _text;
        private int _nliens;
        private ComboBox _fontSizeCombo;
        private ComboBox _fontCombo;
        private ObjectAnimationUsingKeyFrames _x1;
        private CustomTextSource4 _customTextSource;
        private Dispatcher _secondaryDispatcher;

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                // if (SelectionEnabled && e.LeftButton == MouseButtonState.Pressed)
                // {
                var point = e.GetPosition(_rectangle);
                var q = LineInfos.SkipWhile(z => z.Origin.Y < point.Y);
                if (q.Any())
                {
                    var line = q.First();
                    DebugUtils.WriteLine(line.LineNumber.ToString());
                    if (line.Regions != null)
                    {
                        var qq = line.Regions.SkipWhile(zz => !zz.BoundingRect.Contains(point));
                        if (qq.Any())
                        {
                            var region = qq.First();
                            DebugUtils.WriteLine(region.SyntaxToken?.ToString());
                        }
                    }
                }

                var zz = LineInfos.Where(z => z.Regions != null).SelectMany(z => z.Regions)
                    .Where(x => x.BoundingRect.Contains(point)).ToList();
                if (zz.Count > 1)
                    DebugUtils.WriteLine("Multiple regions matched", DebugCategory.TextFormatting);
                //    throw new AppInvalidOperationException();

                // Retrieve the coordinate of the mouse position.


                // Perform the hit test against a given portion of the visual object tree.
                var drawingVisual = new DrawingVisual();
                var drawingVisualDrawing = new DrawingGroup();
                var dc = drawingVisual.RenderOpen();

                // foreach (var g in GeoTuples)
                // {
                // dc.DrawGeometry(Brushes.Black, null, g);
                // }

                foreach (var g in GeoTuples)
                    if (g.Item1.Rect.Contains(point))
                        Debug.WriteLine(g.Item2.SyntaxNode?.Kind().ToString() ?? "");
                // Debug.WriteLine(((RectangleGeometry)g).Rect);

                //

                dc.Close();


                var result = VisualTreeHelper.HitTest(drawingVisual, point);

                if (result != null)
                {
                    // Perform action on hit visual object.
                }

                if (!zz.Any())
                {
                    HoverColumn = 0;
                    HoverSyntaxNode = null;
                    HoverOffset = 0;
                    HoverRegionInfo = null;
                    HoverRow = 0;
                    HoverSymbol = null;
                    HoverToken = null;
                }

                foreach (var tuple in zz)
                {
                    HoverRegionInfo = tuple;
                    if (tuple.Trivia.HasValue) DebugUtils.WriteLine(tuple.ToString(), DebugCategory.TextFormatting);

                    if (tuple.SyntaxNode != HoverSyntaxNode)
                    {
                        if (ToolTip is ToolTip tt) tt.IsOpen = false;
                        HoverSyntaxNode = tuple.SyntaxNode;
                        if (tuple.SyntaxNode != null)
                        {
                            ISymbol sym = null;
                            IOperation operation = null;
                            if (Model != null)
                                try
                                {
                                    sym = Model?.GetDeclaredSymbol(tuple.SyntaxNode);
                                    operation = Model.GetOperation(tuple.SyntaxNode);
                                    var zzz = tuple.SyntaxNode.AncestorsAndSelf().OfType<ForEachStatementSyntax>()
                                        .FirstOrDefault();
                                    if (zzz != null)
                                    {
                                        var info = Model.GetForEachStatementInfo(zzz);
                                        Debug.WriteLine(info.ElementType?.ToDisplayString());
                                    }

                                    switch ((CSharpSyntaxNode) tuple.SyntaxNode)
                                    {
                                        case AssignmentExpressionSyntax assignmentExpressionSyntax:
                                            break;
                                        case ForEachStatementSyntax forEachStatementSyntax:
                                            var info = Model.GetForEachStatementInfo(forEachStatementSyntax);
                                            Debug.WriteLine(info.ElementType.ToDisplayString());
                                            break;
                                        case ForEachVariableStatementSyntax forEachVariableStatementSyntax:
                                            break;
                                        case MethodDeclarationSyntax methodDeclarationSyntax:

                                            break;
                                        case TryStatementSyntax tryStatementSyntax:
                                            break;
                                        case StatementSyntax statementSyntax:
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                catch
                                {
                                    // ignored
                                }

                            if (sym != null)
                            {
                                HoverSymbol = sym;
                                DebugUtils.WriteLine(sym.Kind.ToString(), DebugCategory.TextFormatting);
                            }

                            var node = tuple.SyntaxNode;
                            var nodes = new Stack<SyntaxNodeDepth>();
                            var depth = 0;
                            while (node != null)
                            {
                                node = node.Parent;
                                depth++;
                            }

                            depth--;
                            node = tuple.SyntaxNode;
                            while (node != null)
                            {
                                nodes.Push(new SyntaxNodeDepth {SyntaxNode = node, Depth = depth});
                                node = node.Parent;
                                depth--;
                            }


                            var content = new CodeToolTipContent()
                                {Symbol = sym, SyntaxNode = tuple.SyntaxNode, Nodes = nodes, Operation = operation};
                            var template =
                                TryFindResource(new DataTemplateKey(typeof(CodeToolTipContent))) as DataTemplate;
                            var toolTip = new ToolTip {Content = content, ContentTemplate = template};
                            ToolTip = toolTip;
                            toolTip.IsOpen = true;
                        }
                    }

                    if (tuple.SyntaxNode != null)
                    {
                    }

                    HoverToken = tuple.SyntaxToken;

                    var cellIndex = tuple.Characters.FindIndex(zx => zx.Bounds.Contains(point));
                    if (cellIndex != -1)
                    {
                        var cell = tuple.Characters[cellIndex];

                        var first = cell;
                        var item2 = first.Point;

                        var item2Y = (int) item2.Y;
                        // if (item2Y >= _chars.Count)
                        // {
                        // DebugUtils.WriteLine("out of bounds", DebugCategory.TextFormatting);
                        // }
                        // else
                        // {
                        // var chars = _chars[item2Y];
                        // DebugUtils.WriteLine("y is " + item2Y, DebugCategory.MouseEvents);
                        // var item2X = (int) item2.X;
                        // if (item2X >= chars.Count)
                        // {
                        //DebugUtils.WriteLine("out of bounds", DebugCategory.TextFormatting);
                        // }
                        // else
                        // {
                        // var ch = chars[item2X];
                        // DebugUtils.WriteLine("Cell is " + item2 + " " + ch, DebugCategory.MouseEvents);
                        var newOffset = tuple.Offset + cellIndex;
                        HoverOffset = newOffset;
                        HoverColumn = (int) item2.X;
                        HoverRow = (int) item2.Y;
                        if (SelectionEnabled && IsSelecting)
                        {
                            if (_selectionGeometry != null) _textDest.Children.Remove(_selectionGeometry);
                            DebugUtils.WriteLine("Calculating selection", DebugCategory.TextFormatting);

                            var group = new DrawingGroup();

                            int begin;
                            int end;
                            if (_startOffset < newOffset)
                            {
                                begin = _startOffset;
                                end = newOffset;
                            }
                            else
                            {
                                begin = newOffset;
                                end = _startOffset;
                            }

                            var green = new SolidColorBrush(Colors.Green) {Opacity = .2};
                            var blue = new SolidColorBrush(Colors.Blue) {Opacity = .2};
                            var red = new SolidColorBrush(Colors.Red) {Opacity = .2};
                            foreach (var regionInfo in LineInfos.SelectMany(z => z.Regions).Where(info =>
                                info.Offset <= begin && info.Offset + info.Length > begin ||
                                info.Offset >= begin && info.Offset + info.Length <= end))
                            {
                                DebugUtils.WriteLine(
                                    $"Region offset {regionInfo.Offset} : Length {regionInfo.Length}",
                                    DebugCategory.TextFormatting);
                                if (regionInfo.Offset <= begin)
                                {
                                    var takeNum = begin - regionInfo.Offset;
                                    DebugUtils.WriteLine("Taking " + takeNum, DebugCategory.TextFormatting);
                                    foreach (var tuple1 in regionInfo.Characters.Take(takeNum))
                                    {
                                        DebugUtils.WriteLine("Adding " + tuple1, DebugCategory.TextFormatting);
                                        @group.Children.Add(new GeometryDrawing(red, null,
                                            new RectangleGeometry(tuple1.Bounds)));
                                    }

                                    continue;
                                }

                                if (regionInfo.Offset + regionInfo.Length > end)
                                {
                                    foreach (var tuple1 in regionInfo.Characters.Take(end - regionInfo.Offset))
                                        @group.Children.Add(new GeometryDrawing(blue, null,
                                            new RectangleGeometry(tuple1.Bounds)));

                                    continue;
                                }

                                var geo = new RectangleGeometry(regionInfo.BoundingRect);
                                @group.Children.Add(new GeometryDrawing(green, null, geo));
                            }


                            _selectionGeometry = @group;
                            _textDest.Children.Add(_selectionGeometry);
                            _myDrawingBrush.Drawing = _textDest;
                            _selectionEnd = newOffset;
                            InvalidateVisual();
                        }
                    }

                    var textRunProperties = tuple.TextRun.Properties;
                    if (!(textRunProperties is GenericTextRunProperties)) continue;
                    if (_rect != tuple.BoundingRect)
                    {
                        _rect = tuple.BoundingRect;
                        if (_geometryDrawing != null) _textDest.Children.Remove(_geometryDrawing);

                        var solidColorBrush = new SolidColorBrush(Colors.CadetBlue) {Opacity = .6};


                        _geometryDrawing =
                            new GeometryDrawing(solidColorBrush, null, new RectangleGeometry(tuple.BoundingRect));

                        _textDest.Children.Add(_geometryDrawing);
                        InvalidateVisual();
                    }

                    //DebugUtils.WriteLine(pp.Text);
                }

                if (SelectionEnabled && e.LeftButton == MouseButtonState.Pressed)
                    if (!IsSelecting)
                    {
                        var xy = e.GetPosition(_scrollViewer);
                        if (xy.X < _scrollViewer.ViewportWidth && xy.X >= 0 && xy.Y >= 0 &&
                            xy.Y <= _scrollViewer.ViewportHeight)
                        {
                            _startOffset = HoverOffset;
                            _startRow = HoverRow;
                            _startColumn = HoverColumn;
                            _startNode = HoverSyntaxNode;


                            IsSelecting = true;
                            e.Handled = true;
                            _rectangle.CaptureMouse();
                        }
                    }
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
            }
        }

        private bool z(SyntaxNode arg)
        {
            return arg.Kind() == SyntaxKind.ForEachStatement;
        }

        /// <inheritdoc />
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (IsSelecting)
            {
                IsSelecting = false;
                _endNode = HoverSyntaxNode;
                DebugUtils.WriteLine($"{_startOffset} {_selectionEnd}");
                if (_startNode != null)
                    if (_endNode != null)
                    {
                        var st1 = _startNode.AncestorsAndSelf().OfType<StatementSyntax>().FirstOrDefault();
                        var st2 = _endNode.AncestorsAndSelf().OfType<StatementSyntax>().FirstOrDefault();
                        if (st1 != null)
                            if (st2 != null)
                                if (Model != null)
                                {
                                    var r = Model.AnalyzeDataFlow(st1, st2);
                                    if (r != null)
                                        return;
                                    DebugUtils.WriteLine(r != null && r.Succeeded);
                                }
                    }

                _rectangle.ReleaseMouseCapture();
            }
        }

        /// <inheritdoc />
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override async void OnNodeUpdated()
        {
            base.OnNodeUpdated();
            if (!(ChangingText || UpdatingSourceText))
            {
                DebugUtils.WriteLine("Node updated", DebugCategory.TextFormatting);
                LineInfos.Clear();
                MaxX = 0;
                MaxY = 0;
                _pos = new Point(0, 0);
                if (_scrollViewer != null) _scrollViewer.ScrollToTop();
                if (SecondaryDispatcher != null)
                    await UpdateTextSource();

                //UpdateFormattedText();
            }
        }


        public List<Tuple<RectangleGeometry, RegionInfo>> GeoTuples { get; set; } =
            new List<Tuple<RectangleGeometry, RegionInfo>>();

        /// <inheritdoc />
        public void PrepareDrawLines(LineContext lineContext, bool clear)
        {
        }

        /// <inheritdoc />
        public void PrepareDrawLine(LineContext lineContext)
        {
        }

        /// <inheritdoc />
        public void DrawLine(LineContext lineContext)
        {
        }

        /// <inheritdoc />
        public void EndDrawLines(LineContext lineContext)
        {
        }
    }
}