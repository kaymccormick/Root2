#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// VsInstance.cs
// 
// 2020-02-19-7:03 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;

namespace ProjLib
{
    public class VsInstance : IVsInstance
    {
        public VsInstance(string instanceId, string installationName, string installationPath, string installationVersion, string displayName, string description, DateTime installDate, IList<SetupPackage> packageCollection, SetupPackage product, string productPath)
        {
            InstanceId          = instanceId;
            InstallationName    = installationName;
            InstallationPath    = installationPath;
            InstallationVersion = installationVersion;
            DisplayName         = displayName;
            Description         = description;
            InstallDate         = installDate;
            PackageCollection   = packageCollection;
            Product             = product;
            ProductPath         = productPath;
        }

        public string InstanceId          { get; }
        public string InstallationName    { get; }
        public string InstallationPath    { get; }
        public string InstallationVersion { get; }
        public string DisplayName         { get; }
        public string Description         { get; }

        public DateTime            InstallDate       { get; }
        public VsInstanceState     State             { get; }
        public IList<SetupPackage> PackageCollection { get; }
        public SetupPackage        Product           { get; }
        public string              ProductPath       { get; }
    }
}