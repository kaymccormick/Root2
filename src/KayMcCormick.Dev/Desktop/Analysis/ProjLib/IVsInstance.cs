#if VSSETTINGS
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
    [TypeConverter(typeof(VsInstanceConverter))]
    public interface IVsInstance
    {
        string InstanceId { get ; set ; }

        string InstallationName { get ; set ; }

        string InstallationPath { get ; set ; }

        string InstallationVersion { get ; set ; }

        string DisplayName { get ; set ; }

        string Description { get ; set ; }

        DateTime InstallDate { get ; set ; }

        VsInstanceState State { get ; }

        IList < SetupPackage > PackageCollection { get ; }

        ISetupPackageReference Product { get ; set ; }

        string ProductPath { get ; set ; }

        IList<IMruItem> MruItems { get ; }

        ulong Version { get ; set ; }

        IDictionary < string , object > Properties { get ; set ; }

        bool IsPrerelease { get ; set ; }

        ICollection < Workload > Workloads { get ; }

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
#endif
