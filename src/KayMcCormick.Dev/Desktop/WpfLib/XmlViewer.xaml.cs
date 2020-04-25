﻿using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Xml ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    ///     Interaction logic for XmlViewer.xaml
    /// </summary>
    public partial class XmlViewer : UserControl
    {
        /// <summary>Identifies the <see cref="XmlDocument" /> dependency property.</summary>
        public static readonly DependencyProperty XmlDocumentProperty =
            DependencyProperty.Register (
                                         nameof ( XmlDocument )
                                       , typeof ( XmlDocument )
                                       , typeof ( XmlViewer )
                                       , new FrameworkPropertyMetadata (
                                                                        null
                                                                      , FrameworkPropertyMetadataOptions
                                                                           .None
                                                                      , PropertyChangedCallback
                                                                      , CoerceValueCallback
                                                                       )
                                        ) ;

        /// <summary>
        /// </summary>
        public XmlViewer ( ) { InitializeComponent ( ) ; }

        /// <summary>
        /// </summary>
        public XmlDocument XmlDocument
        {
            get { return ( XmlDocument ) GetValue ( XmlDocumentProperty ) ; }
            set { SetValue ( XmlDocumentProperty , value ) ; }
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

        /// <summary>
        /// </summary>
        /// <param name="document"></param>
        public void BindXMLDocument ( [ CanBeNull ] XmlDocument document )
        {
            if ( document == null )
            {
                xmlTree.SetCurrentValue ( ItemsControl.ItemsSourceProperty , null ) ;
                return ;
            }

            var provider = new XmlDataProvider { Document = document } ;
            var binding = new Binding { Source            = provider , XPath = "child::node()" } ;
            xmlTree.SetBinding ( ItemsControl.ItemsSourceProperty , binding ) ;
        }
    }
}