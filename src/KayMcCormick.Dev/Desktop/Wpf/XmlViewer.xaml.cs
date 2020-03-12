using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Xml ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Interaction logic for XmlViewer.xaml
    /// </summary>
    public partial class XmlViewer : UserControl
    {
        /// <summary>Identifies the <see cref="XmlDocument"/> dependency property.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("WpfAnalyzers.DependencyProperty", "WPF0005:Name of PropertyChangedCallback should match registered name.", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("WpfAnalyzers.DependencyProperty", "WPF0023:The callback is trivial, convert to lambda.", Justification = "<Pending>")]
        public static readonly DependencyProperty XmlDocumentProperty =
            DependencyProperty.Register (
                                         nameof(XmlDocument)
                                       , typeof ( XmlDocument )
                                       , typeof ( XmlViewer )
                                       , new FrameworkPropertyMetadata (
                                                                        null
                                                                      , FrameworkPropertyMetadataOptions
                                                                           .None
                                                                      , PropertyChangedCallback
#pragma warning disable WPF0006 // Name of CoerceValueCallback should match registered name.
                                                                      , CoerceValueCallback
#pragma warning restore WPF0006 // Name of CoerceValueCallback should match registered name.
                                                                       )
                                        ) ;

        public XmlDocument XmlDocument
        {
            // ReSharper disable once UnusedMember.Global
            get => ( XmlDocument ) GetValue ( XmlDocumentProperty ) ;
            set => SetValue ( XmlDocumentProperty , value ) ;
        }

        private static object CoerceValueCallback ( DependencyObject d , object basevalue )
        {
            return basevalue ;
        }

        private static void PropertyChangedCallback (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
            if ( d is XmlViewer viewer )
            {
                viewer.BindXMLDocument ( ( XmlDocument ) e.NewValue ) ;
            }
        }

        public XmlViewer ( ) { InitializeComponent ( ) ; }

        public void BindXMLDocument ( XmlDocument document )
        {
            if ( document == null )
            {
                xmlTree.SetCurrentValue(ItemsControl.ItemsSourceProperty, null) ;
                return ;
            }

            var provider = new XmlDataProvider { Document = document } ;
            var binding = new Binding { Source = provider , XPath = "child::node()" } ;
            xmlTree.SetBinding ( ItemsControl.ItemsSourceProperty , binding ) ;
        }
    }
}