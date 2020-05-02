using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace TestApp
{
    public class ObservableLoadedAssembliesCollection : ObservableCollection<Assembly>
    {
        public ObservableLoadedAssembliesCollection()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Add(assembly);
            }

            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
            {
                Add(args.LoadedAssembly);
            };
        }
    }
}