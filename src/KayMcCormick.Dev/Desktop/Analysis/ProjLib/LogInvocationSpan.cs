#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// LogInvocationSpan.cs
// 
// 2020-02-26-10:00 PM
// 
// ---
#endregion
using System ;
using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;

namespace ProjLib
{
    public class LogInvocationSpan : SpanObject < LogInvocation >
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public LogInvocationSpan (
            TextSpan                 span
          , LogInvocation            instance
          , Func < object , object > getResource
        ) : base ( span , instance )
        {
            _getResource = getResource ;
            _displayString = Instance.MethodDisplayName
                             + " "
                             + string.Join (
                                            ", "
                                          , Instance.Arguments.Select (
                                                                       ( argument , i )
                                                                           => argument.JSON
                                                                      )
                                           ) ;
            LogManager.GetCurrentClassLogger ( ).Info ( "{disp}" , _displayString ) ;
        }

        private Func < object , object > _getResource ;
        private string                   _displayString ;

        public Func < object , object > GetResource
        {
            get => _getResource ;
            set => _getResource = value ;
        }

        public override UIElement GetToolTipContent ( )
        {
            try
            {
                LogManager.GetCurrentClassLogger ( ).Info ( "tooltip {instance}" , Instance ) ;
                if ( Instance != null )
                {
                    var textBlock = new TextBlock ( ) { Text = _displayString } ;
                    
                    var contentPresenter = new ContentControl ( ) { } ;
                    contentPresenter.Content = this.Instance ;
                    var resource = GetResource ( typeof ( LogInvocation ) ) ;
                    LogManager.GetCurrentClassLogger().Info ( "Resource is {Resource}{t}" , resource ?? "null", resource?.GetType().FullName ?? "null" ) ;
                    contentPresenter.ContentTemplate =
                        resource as DataTemplate ;
                    LogManager.GetCurrentClassLogger().Info("{t}", contentPresenter.ContentTemplate?.DataType?.ToString() ?? "null");
                    // toolTipContent.Children.Add ( contentPresenter ) ;
                    // toolTipContent.Children.Add ( textBlock ) ;
                    try
                    {
                        Logger.Info ( "xaml = {xaml}" , XamlWriter.Save ( contentPresenter ) ) ;
                    } catch(Exception ex) {
                        }

                    return contentPresenter;
                }

                LogManager.GetCurrentClassLogger ( ).Error ( "no instance value" ) ;
                return base.GetToolTipContent ( ) ;
            } catch(Exception ex)
            {
                Logger.Error ( ex , "ui: " + ex.ToString()) ;
                return new TextBlock ( ) { Text = ex.ToString ( ) } ;
            }
        }
    }
}