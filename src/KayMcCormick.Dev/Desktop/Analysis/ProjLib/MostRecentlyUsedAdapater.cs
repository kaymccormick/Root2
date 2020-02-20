using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.Settings;

namespace ProjLib
{
    public class MostRecentlyUsedAdapater
    {
        public void VsMrus(IVsInstance vsInstance)
        {
            var path = Path.Combine(vsInstance.InstallationPath, vsInstance.ProductPath);
            try
            {
                var ext = ExternalSettingsManager.CreateForApplication(path);
                var store = ext.GetReadOnlySettingsStore(SettingsScope.UserSettings);
                var mru = @"MRUItems\{a9c4a31f-f9cb-47a9-abc0-49ce82d0b3ac}\Items";
                foreach (var name in store.GetPropertyNames(mru))
                {
                    var value = store.GetString(mru, name);
                    Debug.WriteLine("Property name: {0}, value: {1}", name, value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}