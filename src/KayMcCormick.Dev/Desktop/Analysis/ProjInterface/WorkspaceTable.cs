#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// WorkspaceTable.cs
// 
// 2020-03-02-7:55 AM
// 
// ---
#endregion
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Controls.Ribbon ;

namespace ProjInterface
{
    public class WorkspaceTable : RibbonWindow
    {
        public WorkspaceTable ( )
        {
            var ribbon = new Ribbon ( ) ;
            var qat = new RibbonQuickAccessToolBar ( ) ;
            qat.Items.Add ( new RibbonButton ( ) ) ;
            qat.Items.Add ( new RibbonButton ( ) ) ;
            ribbon.QuickAccessToolBar = qat ;
            var appMenu = new RibbonApplicationMenu ( ) ;
            ribbon.ApplicationMenu = appMenu ;
            appMenu.Items.Add ( new RibbonApplicationMenuItem { Header = "Fun Times" } ) ;
            var filesTab = new RibbonTab { Header                      = "Files" } ;
            ribbon.Items.Add ( filesTab ) ;
            var group = new RibbonGroup { Header         = "Create" } ;
            group.Items.Add ( new RibbonButton { Content = "CSharp" } ) ;
            group.Items.Add ( new RibbonButton { Content = "XAML" } ) ;
            var content = new Grid ( ) ;
            content.RowDefinitions.Add ( new RowDefinition { Height = GridLength.Auto } ) ;
            content.RowDefinitions.Add (
                                        new RowDefinition
                                        {
                                            Height = new GridLength ( 1 , GridUnitType.Star )
                                        }
                                       ) ;
            ribbon.SetValue ( Grid.RowProperty ,    0 ) ;
            ribbon.SetValue ( Grid.ColumnProperty , 0 ) ;
            // ribbon.SetValue ( Grid.ColumnSpanProperty , content.ColumnDefinitions.Count ) ;
            content.Children.Add ( ribbon ) ;
            Content = content ;
        }
    }
}