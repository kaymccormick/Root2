#if VSSETTINGS
// <copyright file="Program.cs" company="Microsoft Corporation">
// Copyright (C) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics ;
using System.Linq;
using System.Reflection ;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.VisualStudio.Setup.Configuration;
using NLog ;
using Xunit ;

namespace ProjLib
{
    [Flags]
    public enum VsInstanceState : uint
    {
        /// <summary>The instance state has not been determined.</summary>
        None = 0,
        /// <summary>The instance installation path exists.</summary>
        Local = 1,
        /// <summary>A product is registered to the instance.</summary>
        Registered = 2,
        /// <summary>No reboot is required for the instance.</summary>
        NoRebootRequired = 4,
        /// <summary>No errors were reported for the instance.</summary>
        NoErrors = 8,
        /// <summary>The instance represents a complete install.</summary>
        Complete = 4294967295, // 0xFFFFFFFF
    }

    public class SetupPackage
    {
    }

    /// <summary>
    /// The main program class.
    /// </summary>
    public class VsCollector : IVsInstanceCollector
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly Func < IVsInstance > _insFunc ;
        public VsCollector ( Func < IVsInstance > insFunc ) { _insFunc = insFunc ; }
        private const int REGDB_E_CLASSNOTREG = unchecked((int)0x80040154);

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Command line arguments passed to the program.</param>
        /// <returns>The process exit code.</returns>
        public int PerformTask(string[] args)
        {
            try
            {
                var query = new SetupConfiguration();
                var query2 = (ISetupConfiguration2)query;
                var e = query2.EnumAllInstances();

                var helper = (ISetupHelper)query;

                int fetched;
                var instances = new ISetupInstance[1];
                do
                {
                    e.Next(1, instances, out fetched);
                    if (fetched > 0)
                    {
                        CollectInstance(instances[0], helper);
                    }
                }
                while (fetched > 0);

                return 0;
            }
            catch (COMException ex) when (ex.HResult == REGDB_E_CLASSNOTREG)
            {
//                Console.WriteLine("The query API is not registered. Assuming no instances are installed.");
                return 0;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
//                Console.Error.WriteLine($"Error 0x{ex.HResult:x8}: {ex.Message}");
                throw;
            }
        }

        private void CollectInstance(ISetupInstance instance, ISetupHelper helper)
        {   
            
            var instance2 = (ISetupInstance2)instance;
            Logger.Trace("collecting vs instance {instance}", instance.GetDisplayName());
            var state = instance2.GetState();
            var ft = instance2.GetInstallDate();
            long qwResult;

            // Copy the time into a quadword.
            qwResult = (((long)ft.dwHighDateTime) << 32) + (long)ft.dwLowDateTime;

          
            
            var fromFileTime = DateTime.FromFileTime(qwResult);
             //IVsInstance  dataObj= new VsInstance();//);
             //( ) ;
             IVsInstance dataObj = _insFunc ( ) ;
            dataObj.Description = instance.GetDescription ( ) ;
            // dataObj.InstallDate = instance.GetInstallDate ( ) ;
            dataObj.ProductPath = instance2.GetProductPath ( ) ;
            dataObj.DisplayName = instance.GetDisplayName ( ) ;
            dataObj.InstallationName = instance.GetInstallationName ( ) ;
            dataObj.InstallationVersion = instance.GetInstallationVersion ( ) ;
            dataObj.InstanceId = instance.GetInstanceId ( ) ;
            dataObj.Product = instance2.GetProduct ( ) ;
            //
            dataObj.InstallationPath = instance.GetInstallationPath ( ) ;
            
            // var fieldInfos = typeof(IVsInstance).GetProperties(BindingFlags.Public | BindingFlags.Instance) ;
            // var methodInfos = typeof(ISetupInstance).GetMethods().Concat(typeof(ISetupInstance).GetMethods()) ;
            // var x = from method in 
            // methodInfos join
            // field_ in fieldInfos
            // on method.Name.Substring(4) equals field_.Name select Tuple.Create(method, field_);
            //
            // foreach ( var propertyInfo in fieldInfos )
            // {
            //     var m = instance.GetType ( ).GetMethod ( "Get" + propertyInfo.Name ) ;
            //     Debug.WriteLine ( m ) ;
            //     foreach ( var methodInfo in instance
            //                                .GetType ( )
            //                                .GetInterfaces ( )
            //                                .Select (
            //                                         t => instance.GetType ( ).GetInterfaceMap ( t )
            //                                        )
            //                                .SelectMany ( mapping => mapping.TargetMethods ) )
            //     {
            //         var tt = methodInfo.Name ;
            //
            //     }
            // }
            // foreach ( var y in x )
            // {
            //     y.Item2.SetValue (
            //                       dataObj
            //                     , y.Item1.Invoke ( instance , Array.Empty < object > ( ) )
            //                      ) ;
            // }
            //
            //
            //
            // // }( instance )).Select()
            // // dataObj.InstallDate = instance.GetInstallDate();
            // // dataObj.Description = instance.GetDescription();
            // dataObj.ProductPath = 
// /            var dataObj = new VsInstance(
            // instance.GetInstanceId()
            // , instance.GetInstallationName()
            // , instance.GetInstallationPath()
            // , instance.GetInstallationVersion()
            // , instance.GetDisplayName()
            // , instance.GetDescription()
            // , fromFileTime
            // , new List<SetupPackage>()
            // , new SetupPackage()
            // , instance2.GetProductPath()
            
            Instances.Add(dataObj);

//            Console.WriteLine($"InstanceId: {instance2.GetInstanceId()} ({(state == InstanceState.Complete ? "Complete" : "Incomplete")})");

            var installationVersion = instance.GetInstallationVersion();
            var version = helper.ParseVersion(installationVersion);
            dataObj.Version = version ;

//            Console.WriteLine($"InstallationVersion: {installationVersion} ({version})");

            if ((state & InstanceState.Local) == InstanceState.Local)
            {
//                Console.WriteLine($"InstallationPath: {instance2.GetInstallationPath()}");
            }

            var catalog = instance as ISetupInstanceCatalog;
            if (catalog != null)
            {
                   Logger.Info($"IsPrerelease: {catalog.IsPrerelease()}");
                   dataObj.IsPrerelease = catalog.IsPrerelease ( ) ;
            }

            if ((state & InstanceState.Registered) == InstanceState.Registered)
            {
//                Console.WriteLine($"Product: {instance2.GetProduct().GetId()}");
//                Console.WriteLine("Workloads:");

                //PrintWorkloads(instance2.GetPackages(), dataObj);
            }

            var properties = instance2.GetProperties();
            if (properties != null)
            {
//                Console.WriteLine("Custom properties:");
                var props1 = PrintProperties(properties);
                dataObj.Properties = props1 ;
            }

            properties = catalog?.GetCatalogInfo();
            if (properties != null)
            {
//                Console.WriteLine("Catalog properties:");
                var props = PrintProperties(properties);
            }

//            Console.WriteLine();
        }

        public List<IVsInstance> Instances { get; set; } = new List<IVsInstance>();

        private static IDictionary < string , dynamic > PrintProperties(ISetupPropertyStore store)
        {
            var properties = from name in store.GetNames()
                             orderby name
                             select new { Name = name, Value = store.GetValue(name) };

            IDictionary<string, dynamic> props=new Dictionary < string , dynamic > ();
            foreach (var prop in properties)
            {
                props[ prop.Name ] = prop.Value ;
                
//                Console.WriteLine($"    {prop.Name}: {prop.Value}");
            }

            return props ;
        }

        private static void PrintWorkloads (
            ISetupPackageReference[] packages
          , IVsInstance              dataObj
        )
        {
            var workloads = from package in packages
                            where string.Equals(package.GetType(), "Workload", StringComparison.OrdinalIgnoreCase)
                            orderby package.GetId()
                            select package;

            foreach (var workload in workloads)
            {
                Logger.Info ( "workload {x}" , workload.GetId ( ) ) ;
                dataObj.AddWorkload (
                                     workload.GetId ( )
                                   , workload.GetBranch ( )
                                   , workload.GetChip ( )
                                   , workload.GetVersion ( )
                                   , workload.GetIsExtension ( )
                                   , workload.GetLanguage ( )
                                   , workload.GetType ( )
                                   , workload.GetUniqueId ( )
                                    ) ;
//                Console.WriteLine($"    {workload.GetId()}");
            }
        }

        public IList < IVsInstance > CollectVsInstances ( )
        {
            PerformTask(null);
            return Instances;
        }
    }
}
#endif
