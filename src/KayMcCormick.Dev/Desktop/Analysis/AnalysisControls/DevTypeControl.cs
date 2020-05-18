using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CSharp;

namespace AnalysisControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KayMcCormick.Lib.Wpf"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KayMcCormick.Lib.Wpf;assembly=KayMcCormick.Lib.Wpf"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DevTypeControl/>
    ///
    /// </summary>
    public class DevTypeControl : Control
    {
        public static readonly DependencyProperty InlinesProperty = DependencyProperty.Register(
            "Inlines", typeof(ObservableCollection<Inline>), typeof(DevTypeControl), new PropertyMetadata(default(InlineCollection)));

        public ObservableCollection<Inline> Inlines
        {
            get { return (ObservableCollection<Inline>) GetValue(InlinesProperty); }
            set { SetValue(InlinesProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(Type), typeof(DevTypeControl), new PropertyMetadata(default(Type), OnTypeChaged));

        private static void OnTypeChaged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DevTypeControl c = (DevTypeControl) d;
            c.UpdateFormattedText(c._pixelsPerDip);
        }

        private double _pixelsPerDip;
        private FontRendering _currentRendering;
        private bool _UILoaded;
        private CustomTextSource2 _textStore;
        private DrawingBrush myDrawingBrush = new DrawingBrush();
        private DrawingBrush _drawing;
        private DrawingGroup _textDest;
        private Point _pos;
        private double max_x;
        public TextLine TheLine { get; private set; }
        
        protected override Size MeasureOverride(Size constraint)
        {
            return new Size(max_x, _pos.Y);
        }

        public Type Type
        {
            get { return (Type) GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }   
        static DevTypeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DevTypeControl), new FrameworkPropertyMetadata(typeof(DevTypeControl)));
        }

        public DevTypeControl()

        {
            
        }

        public static string MyTypeName(Type myType)
        {

            var myTypeName = myType.Name;
            if (myType == typeof(Boolean))
            {
                myTypeName = "bool";
            }
            else if (myType == typeof(Byte))
            {
                myTypeName = "byte";
            }
            else if (myType == typeof(SByte))
            {
                myTypeName = "sbyte";
            }
            else if (myType == typeof(Char))
            {
                myTypeName = "char";
            }
            else if (myType == typeof(Decimal))
            {
                myTypeName = "decimal";
            }
            else if (myType == typeof(Double))
            {
                myTypeName = "double";
            }
            else if (myType == typeof(Single))
            {
                myTypeName = "float";
            }
            else if (myType == typeof(Int32))
            {
                myTypeName = "int";
            }
            else if (myType == typeof(UInt32))
            {
                myTypeName = "uint";
            }
            else if (myType == typeof(Int64))
            {
                myTypeName = "long";
            }
            else if (myType == typeof(UInt64))
            {
                myTypeName = "ulong";
            }
            else if (myType == typeof(Int16))
            {
                myTypeName = "short";
            }
            else if (myType == typeof(UInt16))
            {
                myTypeName = "ushort";
            }
            else if (myType == typeof(Object))
            {
                myTypeName = "object";
            }
            else if (myType == typeof(String))
            {
                myTypeName = "string";
            }
            else if (myType == typeof(void))
            {
                myTypeName = "void";
            }

            return myTypeName;
        }

        public bool MakeHyperlink { get; set; }

        [NotNull]
        private static object ToolTipContent([NotNull] Type myType, StackPanel pp = null)
        {
            var provider = new CSharpCodeProvider();
            var codeTypeReference = new CodeTypeReference(myType);
            var q = codeTypeReference;
            var toolTipContent = new TextBlock
            {
                Text = provider.GetTypeOutput(q),
                FontSize = 20
                //, Margin = new Thickness ( 15 )
            };
            if (pp == null)
            {
                pp = new StackPanel { Orientation = Orientation.Vertical };
            }

            pp.Children.Insert(0, toolTipContent);
            var @base = myType.BaseType;
            if (@base != null)
            {
                ToolTipContent(@base, pp);
            }

            return pp;
        }


        private void PopulateControl ( [ CanBeNull ] Type myType )
        {
            if ( myType == null )
            {
                return ;
            }

            var p = new Span ( ) ;
            
            //GenerateControlsForType ( myType , Inlines , true ) ;
            // foreach (object o in m0)
            // {
                // if (o is Inline il)
                // {
                    // p.Inlines.Add(il);
                // } else
                // {
                    // throw new AppInvalidOperationException();
                // }
            // }
            // addChild.Inlines.Add(p);
        }

        public bool Detailed { get; set; }

        public void b()
        {
            _pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            // Enumerate the fonts and add them to the font family combobox.
            foreach (System.Windows.Media.FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                //fontFamilyCB.Items.Add(fontFamily.Source);
            }
            //fontFamilyCB.SelectedIndex = 7;

            // Load the font size combo box with common font sizes.
            // for (int i = 0; i < CommonFontSizes.Length; i++)
            // {
                // fontSizeCB.Items.Add(CommonFontSizes[i]);
            // }
            // fontSizeCB.SelectedIndex = 21;

            //Load capitals combo box
            //typographyMenuBar.Visibility = Visibility.Collapsed;

            //Set up the initial render state of the drawn text.
            if (_currentRendering == null)
            {
                _currentRendering = FontRendering.CreateInstance((double)12,//fontSizeCB.SelectedItem,
                    TextAlignment.Left,
                    null,
                    System.Windows.Media.Brushes.Black,
                    new Typeface("Arial"));
            }

            _UILoaded = true;    //All UI is loaded, can handle events now
            UpdateFormattedText(_pixelsPerDip);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            _drawing = (DrawingBrush) GetTemplateChild("myDrawingBrush");
            _textDest = (DrawingGroup)GetTemplateChild("textDest");
            b();
            //UpdateFormattedText(_pixelsPerDip);
        }

        private void UpdateFormattedText(double pixelsPerDip)
        {
            // Make sure all UI is loaded
            if (!_UILoaded)
                return;

            // Initialize the text store.
            _textStore = new CustomTextSource2(_pixelsPerDip);
            _textStore.Type = Type;
            int textStorePosition = 0;                //Index into the text of the textsource
            System.Windows.Point linePosition = new System.Windows.Point(0, 0);     //current line

            // Create a DrawingGroup object for storing formatted text.
            
            DrawingContext dc = _textDest.Open();

            // Update the text store.
            _textStore.Type = Type;
            _textStore.FontRendering = _currentRendering;

            // Create a TextFormatter object.
            TextFormatter formatter = TextFormatter.Create();

            // Format each line of text from the text store and draw it.
            while (textStorePosition < _textStore.Text.Length)
            {
                // Create a textline from the text store using the TextFormatter object.
                using (TextLine myTextLine = formatter.FormatLine(
                    _textStore,
                    textStorePosition,
                    96 * 6,
                    new GenericTextParagraphProperties(_currentRendering, _pixelsPerDip),
                    null))
                {
                    foreach (var textRunSpan in myTextLine.GetTextRunSpans())
                    {
                        var props = textRunSpan.Value.Properties;
                        if (props is GenericTextRunProperties p1)
                        {
                            p1.Typeface.TryGetGlyphTypeface(out var z12);
                            GlyphRun x =    new GlyphRun((float) _pixelsPerDip);


                            // DebugUtils.WriteLine(p1.SymbolDisplaYPart.Kind.ToString());
                        }

                        if (props != null) DebugUtils.WriteLine(props.ToString());
                    }
                    // Draw the formatted text into the drawing context.
                    myTextLine.Draw(dc, linePosition, InvertAxes.None);
                    TheLine = myTextLine;
                    var p2 = new Point(linePosition.X, linePosition.Y);
                    foreach (var indexedGlyphRun in myTextLine.GetIndexedGlyphRuns())

                    {
                        var box = indexedGlyphRun.GlyphRun.BuildGeometry().Bounds;
                        DebugUtils.WriteLine(box.ToString());
                        var last = indexedGlyphRun.GlyphRun.AdvanceWidths.Last();
                        p2.Offset(last, 0);
                        dc.DrawRectangle(null, new Pen(Brushes.Pink,2), box);
                        
                        
                            
                        DebugUtils.WriteLine(box.ToString());
                        //DebugUtils.WriteLine(indexedGlyphRun.GlyphRun.BuildGeometry().Bounds);
                        p2.Offset(box.Width, 0);    
                    }
                    
                    // Update the index position in the text store.
                    textStorePosition += myTextLine.Length;

                    // Update the line position coordinate for the displayed line.
                    linePosition.Y += myTextLine.Height;
                    if (myTextLine.Width >= max_x)
                    {
                        max_x = myTextLine.Width;
                    }
                    
                }

                _pos = linePosition;
            }

            // Persist the drawn text content.
            dc.Close();

            // Display the formatted text in the DrawingGroup object.
            _drawing.Drawing = _textDest;
            InvalidateMeasure();
            InvalidateVisual();

        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (TheLine != null)
                foreach (var indexedGlyphRun in TheLine.GetIndexedGlyphRuns())
                {
                    var b = indexedGlyphRun.GlyphRun.BuildGeometry().GetRenderBounds(new Pen(Brushes.Black, 1));

                    if (b.IsEmpty)
                        continue;


                    b.Offset(indexedGlyphRun.GlyphRun.BaselineOrigin.X, indexedGlyphRun.GlyphRun.BaselineOrigin.Y);
                    DebugUtils.WriteLine(b.ToString());
                    drawingContext.DrawRectangle(null, new Pen(Brushes.Green, 1), b);
                }
        }

        protected override void OnMouseMove(MouseEventArgs e)

        {

            DebugUtils.WriteLine(e.GetPosition(this).ToString());
            var gr = TheLine.GetIndexedGlyphRuns().Where(x =>
            {
                var computeAlignmentBox = x.GlyphRun.BuildGeometry().Bounds;
                DebugUtils.WriteLine(computeAlignmentBox.ToString());
                if (computeAlignmentBox.IsEmpty)
                {
                    return false;
                }
                computeAlignmentBox.Offset(x.GlyphRun.BaselineOrigin.X, x.GlyphRun.BaselineOrigin.Y);
                DebugUtils.WriteLine(computeAlignmentBox.ToString());
                var position = e.GetPosition(this);
                var alignmentBox = new Rect(new Point(0, 0),
                    new Size(position.X - computeAlignmentBox.X,
                        position.Y - computeAlignmentBox.Y));

                DebugUtils.WriteLine(alignmentBox.ToString());
                return computeAlignmentBox.Contains(position);
            }).ToList();
            if (gr.Any())
            {
                if (gr.Count() > 1)
                {
                    throw new AppInvalidOperationException();
                }

                DebugUtils.WriteLine(gr.First().GlyphRun.Characters.ToString());
            }

        }

        private HitTestResultBehavior ResultCallback(HitTestResult result)
        {
            DebugUtils.WriteLine(result.VisualHit.ToString());
            return HitTestResultBehavior.Continue;
        }

        private HitTestFilterBehavior FilterCallback(DependencyObject potentialhittesttarget)
        {
            
            DebugUtils.WriteLine(potentialhittesttarget.ToString());
            return HitTestFilterBehavior.Continue;
        }

        public HitTestResultBehavior MyCallback(HitTestResult result)
        {
            if (result.VisualHit.GetType() == typeof(System.Windows.Media.DrawingVisual))
            {
                ((System.Windows.Media.DrawingVisual)result.VisualHit).Opacity =
                    ((System.Windows.Media.DrawingVisual)result.VisualHit).Opacity == 1.0 ? 0.4 : 1.0;
            }

            // Stop the hit test enumeration of objects in the visual tree.
            return HitTestResultBehavior.Stop;
        }
    }


    // CustomTextSource is our implementation of TextSource.  This is required to use the WPF
    // text engine. This implementation is very simplistic as is DOES NOT monitor spans of text
    // for different properties. The entire text content is considered a single span and all 
    // changes to the size, alignment, font, etc. are applied across the entire text.
}
