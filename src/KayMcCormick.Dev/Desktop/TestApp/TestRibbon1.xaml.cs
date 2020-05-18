using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KayMcCormick.Dev;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for TestRibbon1.xaml
    /// </summary>
    public partial class TestRibbon1 : Window
    {
        public TestRibbon1()
        {
            InitializeComponent();
            foreach (var ribbonItem in Ribbon.Items)
            {
                if (ribbonItem is RibbonTab tab)
                {
                    foreach (var tabItem in tab.Items)
                    {
                        if (tabItem is RibbonGroup grp)
                        {
                            foreach (var grpItem in grp.Items)
                            {
                                if (grpItem is RibbonComboBox c)
                                {
                                    foreach (var cItem in c.Items)
                                    {
                                        if (cItem is RibbonGallery g)
                                        {
                                            foreach (var gItem in g.Items)
                                            {
                                                if (gItem is RibbonGalleryCategory cc)
                                                {
                                                    foreach (var ccItem in cc.Items)
                                                    {
                                                        if (ccItem is RibbonGalleryItem item)
                                                        {
                                                            DebugUtils.WriteLine(ccItem.ToString());
                                                        }
                                                        else
                                                        {
                                                            throw new AppInvalidOperationException();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    throw new AppInvalidOperationException();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            throw new AppInvalidOperationException();
                                        }
                                    }
                                }
                                else
                                {
                                    throw new AppInvalidOperationException();
                                }

                            }
                        } else
                        {
                            throw new AppInvalidOperationException();
                        }
                    }
                }
                else
                {
                    throw new AppInvalidOperationException();
                }
            }
        }
    }
}
