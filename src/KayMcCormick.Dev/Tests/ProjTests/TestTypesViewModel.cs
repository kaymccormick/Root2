using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisAppLib.Syntax;
using AnalysisControls;
using KayMcCormick.Dev;
using Xunit;

namespace ProjTests
{
    public class TestTypesViewModel
    {
        [WpfFact]
        public void TestXamlLoad()
        {
            var model = TypesViewModelFactory.CreateModel();
            foreach (var appTypeInfo in model.GetAppTypeInfos())
            {
                foreach (var appMethodInfo in appTypeInfo.FactoryMethods)
                {
                    DebugUtils.WriteLine(appMethodInfo.MethodName);
                    if (appMethodInfo.XmlDoc != null) DebugUtils.WriteLine(appMethodInfo.XmlDoc.ToString());
                    foreach (AppParameterInfo appParameterInfo in appMethodInfo.Parameters)
                    {
                        DebugUtils.WriteLine(appParameterInfo.Name);
                        DebugUtils.WriteLine(appParameterInfo.ParameterType.Name);
                    }
                }
            }
        }
    }
}
