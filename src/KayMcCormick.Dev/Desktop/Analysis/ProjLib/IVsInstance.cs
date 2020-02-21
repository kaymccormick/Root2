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
using Microsoft.VisualStudio.Setup.Configuration ;

namespace ProjLib
{
    public interface IVsInstance
    {
        string InstanceId { get ; set ; }

        string InstallationName { get ; set ; }

        string InstallationPath { get ; }

        string InstallationVersion { get ; set ; }

        string DisplayName { get ; set ; }

        string Description { get ; set ; }

        DateTime InstallDate { get ; set ; }

        VsInstanceState State { get ; }

        IList < SetupPackage > PackageCollection { get ; }

        ISetupPackageReference Product { get ; set ; }

        string ProductPath { get ; set ; }
    }
}