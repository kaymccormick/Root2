using System;
using System.Windows;
using System.Windows.Controls;
using AnalysisControls;
using RoslynCodeControls;
using Xunit;

namespace NetCoreTest1
{
    public class UnitTest1
    {
        [WpfFact]
        public void Test1()
        {
            // FormattedTextControl c = new FormattedTextControl();
        }

  [WpfFact]
        public void TestDocumentPage()
        {
            PaginatingRoslynCodeControl x = new PaginatingRoslynCodeControl();
            x.Filename = @"C:\temp\dockingmanager.cs";
            x.AddHandler(RoslynCodeControl.RenderCompleteEvent, new RoutedEventHandler((sender, args) =>
            {
                PrintDialog xx = new PrintDialog();
                // xx.
                xx.ShowDialog();

                xx.PrintDocument(x.DocumentPaginator, "");
            }));
        }
    }
}
