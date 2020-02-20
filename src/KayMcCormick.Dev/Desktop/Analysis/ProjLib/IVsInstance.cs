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

namespace ProjLib
{
    public interface IVsInstance
    {
        string InstanceId { get ; }

        string InstallationName { get ; }

        string InstallationPath { get ; }

        string InstallationVersion { get ; }

        string DisplayName { get ; }

        string Description { get ; }

        DateTime InstallDate { get ; }

        VsInstanceState State { get ; }

        IList < SetupPackage > PackageCollection { get ; }

        SetupPackage Product { get ; }

        string ProductPath { get ; }
    }
}