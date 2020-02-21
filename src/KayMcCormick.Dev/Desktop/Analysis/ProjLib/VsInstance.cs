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
using Microsoft.VisualStudio.Setup.Configuration ;

namespace ProjLib
{
    public class VsInstance : IVsInstance
    {
        private readonly IMruItemProvider _provider ;

#if false
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
            // Product             = product;
            ProductPath         = productPath;
        }
#endif

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public VsInstance ( IMruItemProvider provider )
        {
            _provider = provider ;
        }

        public List <IMruItem> MruItems => _provider.GetMruItemListFor(this);

        public string InstanceId { get; set ; }

        public string InstallationName { get; set ; }

        public string InstallationPath    { get; }
        public string InstallationVersion { get; set ; }

        public string DisplayName { get; set ; }

        public string Description { get; set ; }

        public DateTime            InstallDate { get; set ; }

        public VsInstanceState     State             { get; }
        public IList<SetupPackage> PackageCollection { get; }
        public ISetupPackageReference        Product { get; set ; }

        public string              ProductPath { get; set ; }
    }
}