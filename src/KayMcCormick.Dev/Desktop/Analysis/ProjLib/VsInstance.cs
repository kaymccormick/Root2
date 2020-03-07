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
using System.ComponentModel ;
using System.Runtime.InteropServices ;
using Microsoft.VisualStudio.Setup.Configuration ;

namespace ProjLib
{
    [TypeConverter( typeof(VsInstanceConverter))]
    public class VsInstance : IVsInstance
    {
        private readonly IMruItemProvider _provider ;
        private List < IMruItem > _mruItems ;
        private ulong _version ;
        private IDictionary < string , object > _properties ;
        private ICollection < Workload > _workloads  = new List < Workload > ();
        private bool _isPrerelease ;

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

        public List < IMruItem > MruItems
        {
            get
            {
                try
                {
                    if ( _mruItems == null)
                    {
                        _mruItems = _provider.GetMruItemListFor ( this ) ;
                    }
                } catch(COMException)
                {
                    }

                return _mruItems ;
            }
        }

        public ulong Version { get => _version ; set => _version = value ; }

        public IDictionary < string , object > Properties { get => _properties ; set => _properties = value ; }

        public bool IsPrerelease { get => _isPrerelease ; set => _isPrerelease = value ; }

        public void AddWorkload (
            string id
          , string branch
          , string chip
          , string version
          , bool   isExtension
          , string language
          , string type
          , string uniqueId
        )
        {
            var workload = new Workload (
                                         id
                                       , branch
                                       , chip
                                       , version
                                       , isExtension
                                       , language
                                       , type
                                       , uniqueId
                                        ) ;
            Workloads.Add ( workload ) ;
        }

        public ICollection <Workload> Workloads { get { return _workloads ; } }

        public string InstanceId { get; set ; }

        public string InstallationName { get; set ; }

        public string InstallationPath    { get; set ; }
        public string InstallationVersion { get; set ; }

        public string DisplayName { get; set ; }

        public string Description { get; set ; }

        public DateTime            InstallDate { get; set ; }

        public VsInstanceState     State             { get; }
        public IList<SetupPackage> PackageCollection { get; }
        public ISetupPackageReference        Product { get; set ; }

        public string              ProductPath { get; set ; }

        IList < IMruItem > IVsInstance.MruItems
        {
            get { return _provider.GetMruItemListFor ( this ) ; }
        } 
    }

    public class Workload
    {
        public string Id { get ; }

        public string Branch { get ; }

        public string Chip { get ; }

        public string Version { get ; }

        public bool IsExtension { get ; }

        public string Language { get ; }

        public string Type { get ; }

        public string UniqueId { get ; }

        public Workload ( string id , string branch , string  chip , string version , bool isExtension , string language , string type , string uniqueId )
        {
            Id = id ;
            Branch = branch ;
            Chip = chip ;
            Version = version ;
            IsExtension = isExtension ;
            Language = language ;
            Type = type ;
            UniqueId = uniqueId ;
        }
    }
}