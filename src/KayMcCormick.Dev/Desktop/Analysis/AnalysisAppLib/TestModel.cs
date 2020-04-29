using System;
using System.Collections.Generic;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class TestModel
    {
        IEnumerable<Category> c;

        public TestModel(IEnumerable<Category> c, IEnumerable<Meta<Lazy<IBaseLibCommand>>> cm)
        {
            this.c = c;
        }
    }
}