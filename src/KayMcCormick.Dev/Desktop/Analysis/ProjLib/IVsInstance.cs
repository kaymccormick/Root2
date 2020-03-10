#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// IVsInstance.cs
// 
// 2020-02-19-7:01 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using Microsoft.VisualStudio.Setup.Configuration ;

namespace ProjLib
{
#if VSSETTINGS
    [TypeConverter(typeof(VsInstanceConverter))]
#endif
    public interface IVsInstance
    {
        string InstanceId { get ; set ; }

        string InstallationName { get ; set ; }

        string InstallationPath { get ; set ; }

        string InstallationVersion { get ; set ; }

        string DisplayName { get ; set ; }

        string Description { get ; set ; }

        DateTime InstallDate { get ; set ; }
#if VSSETTINGS
        VsInstanceState State { get ; }
        
        IList < SetupPackage > PackageCollection { get ; }

        ISetupPackageReference Product { get ; set ; }
#endif
        string ProductPath { get ; set ; }

        IList<IMruItem> MruItems { get ; }

        ulong Version { get ; set ; }

        IDictionary < string , object > Properties { get ; set ; }

        bool IsPrerelease { get ; set ; }
#if VSSETTINGS
        ICollection < Workload > Workloads { get ; }
#endif
        void AddWorkload (
            string id
          , string branch
          , string chip
          , string version
          , bool   isExtension
          , string language
          , string type
          , string uniqueId
        ) ;
    }
}
