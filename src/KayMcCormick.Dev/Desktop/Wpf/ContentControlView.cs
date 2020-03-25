using System ;
using System.Collections.Generic ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using AnalysisAppLib.ViewModel ;
using Autofac ;
using Autofac.Core ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    [ContentProperty(nameof(Content))]
    public class ContentControlView : ContentControl, IControlView 
    {
    }
}