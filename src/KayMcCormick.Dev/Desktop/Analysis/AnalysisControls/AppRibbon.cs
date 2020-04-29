using System.Collections.Generic;
using AnalysisAppLib;
using Autofac.Features.Metadata;

namespace AnalysisControls
{
    public class AppRibbon : IAppRibbon
    {
        private List<IAppRibbonTab> _tabList;
        private List<IAppRibbonTab> _cat = new List<IAppRibbonTab>();

        public AppRibbon(AppRibbonTabSet tabSet, IEnumerable<Meta<IRibbonComponent>> components)
        {
            _tabList = new List<IAppRibbonTab>();
            _tabList.AddRange(tabSet.TabSet);
            Tabs = _tabList;
            foreach (var appRibbonTab in _tabList)
            {
                var cat = appRibbonTab.Category;
                var i = (int) cat.Category;
                if (_cat.Count <= i)
                {
                    while (_cat.Count <= i)
                    {
                        _cat.Add(null);
                    }
                }
                _cat[i] = appRibbonTab;
            }
            foreach (var component in components)
            {
                var props = MetaHelper.GetMetadataProps(component.Metadata);
                var xx = _cat[(int) props.Category];
                xx.AddComponent(component.Value);
            }
        }

        public IEnumerable<IAppRibbonTab> Tabs { get; }
    }
}