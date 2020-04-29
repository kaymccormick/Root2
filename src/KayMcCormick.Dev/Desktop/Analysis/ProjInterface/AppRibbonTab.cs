﻿using System.Collections.Generic;

namespace ProjInterface
{
    public class AppRibbonTab : IAppRibbonTab
    {
        public CategoryInfo Category { get; set; }
        public void AddComponent(IRibbonComponent value)
        {
            Components.Add(value);
        }

        public List<IRibbonComponent> Components { get; set; } = new List<IRibbonComponent>();
    }
}