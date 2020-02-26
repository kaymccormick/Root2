using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProjLib;
using Xunit.Sdk;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for AnalyzeResults.xaml
    /// </summary>
    public partial class AnalyzeResults : Window
    {
        public IWorkspacesViewModel ViewModel { get ; }
        public AnalyzeResults ( IWorkspacesViewModel workspacesViewModel )
        {
            ViewModel = workspacesViewModel ;
            InitializeComponent();
        }
    }
    public class CodeConverter  : IValueConverter
    {
        /// <summary>Converts a value. </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            StatementSyntax st = null ;
            switch ( value )
            {
                case LogInvocation lv: st = lv.Statement ;
                    break ;
                    case StatementSyntax syn: st = syn ;
                        break ;
            }
            if(st == null)
            {
                return null ;

            }
            
            TextBlock  block = new TextBlock();
            Visitor v = new Visitor(block, o => null,null,  SyntaxWalkerDepth.Trivia);
            v.Visit ( st ) ;
            return block ;
        }

        private object ProcessNode ( SyntaxNodeOrToken arg1 , int arg2 )
        {

            if ( arg1.IsToken )
            {
                var run = new Run ( arg1.AsToken ( ).ToString ( ) ) { Foreground = Brushes.Green } ;
            }
            else
            {
                var nodes = arg1.ChildNodesAndTokens ( ).Select ( ProcessNode ) ;
            }

            return null ;
        }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { throw new NotImplementedException ( ) ; }
    }

}
