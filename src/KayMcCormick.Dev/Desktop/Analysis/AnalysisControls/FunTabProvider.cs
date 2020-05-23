using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AnalysisControls.RibbonModel;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class FunTabProvider : IRibbonModelProvider<RibbonModelTab>
    {
        private IEnumerable<IRibbonModelProvider<RibbonModelGroup>> _provs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provs"></param>
        public FunTabProvider(IEnumerable<IRibbonModelProvider<RibbonModelGroup>> provs)
        {
            _provs = provs;
        }

        /// <inheritdoc />
        public RibbonModelTab ProvideModelItem()
        {
            var tab = new RibbonModelTab {Header = "Fun tab"};

            var g = tab.CreateGroup("Group 1");
            g.CreateButton("Test Button");
            var files = g.CreateRibbonMenuButton("Files");
            var d = new DirectoryInfo(@"C:\temp\filesmenu"); //@ Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            files.Items= d.EnumerateFileSystemInfos();
            if (_provs.Any())
                foreach (var ribbonModelProvider in _provs)
                {
                    var item = ribbonModelProvider.ProvideModelItem();
                    tab.ItemsCollection.Add(item);
                }
            else
                DebugUtils.WriteLine("No providers for tab item groups");

            return tab;
        }
    }
}