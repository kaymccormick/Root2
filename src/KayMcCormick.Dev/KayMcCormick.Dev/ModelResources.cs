#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Lib.Wpf
// ModelResources.cs
// 
// 2020-03-30-3:32 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO ;
using System.Runtime.Serialization ;
using System.Runtime.Serialization.Formatters.Soap ;
using System.Text ;
using Autofac ;
using Autofac.Core.Lifetime ;
using Autofac.Features.Metadata ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Interfaces ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// ViewModel designed to expose a hierarchy of resources in an application.
    /// </summary>
    public sealed class ModelResources: ISupportInitializeNotification, IViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        internal readonly IObjectIdProvider _idProvider ;
        /// <summary>
        /// 
        /// </summary>
        private readonly ILifetimeScope _lifetimeScope ;
        private readonly ObservableCollection < ResourceNodeInfo > _allResourcesCollection =new ObservableCollection < ResourceNodeInfo > ();

        /// <summary>
        /// </summary>
        public bool IsEnabledPopulateObjects { get ; } = true ;

        /// <summary>
        /// </summary>
        public ObservableCollection < ResourceNodeInfo > AllResourcesCollection
        {
            get { return _allResourcesCollection ; }
        }

        /// <summary>
        /// </summary>
        // ReSharper disable once CollectionNeverQueried.Global
        public ObservableCollection < ResourceNodeInfo > AllResourcesItemList { get ; } =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <param name="idProvider"></param>
        public ModelResources(
            ILifetimeScope    lifetimeScope
          , IObjectIdProvider idProvider
        )
        {
            _lifetimeScope = lifetimeScope;
            _idProvider    = idProvider;

        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateObjects ( )
        {
            var n1 = CreateNode ( null , "Objects" , null , false ) ;
            foreach ( var rootNode in _idProvider.GetRootNodes ( ) )
            {
                var c = _idProvider.GetComponentInfo ( rootNode ) ;
                var n2 = CreateNode ( n1 , rootNode , c , true ) ;
                foreach ( var instanceInfo in c.Instances )
                {
                    var n3 = CreateNode ( n2 , instanceInfo , instanceInfo , true ) ;
                    var type = instanceInfo.Instance.GetType ( ) ;
                    if ( type.IsGenericType
                         && type.GetGenericTypeDefinition ( ) == typeof ( Meta <> ) )
                    {
                        var metadata = ( IDictionary < string , object > ) type
                                                                          .GetProperty (
                                                                                        "Metadata"
                                                                                       )
                                                                         ?.GetValue (
                                                                                     instanceInfo
                                                                                        .Instance
                                                                                    ) ;
                        var c4 = CreateNode ( n3 , "Metadata" , metadata , true ) ;
                        if ( metadata != null )
                        {
                            foreach ( var keyValuePair in metadata )
                            {
                                CreateNode ( c4 , keyValuePair.Key , keyValuePair.Value , false ) ;
                            }
                        }
                    }

                    CreateNode ( n3 , instanceInfo.Instance , instanceInfo.Instance , false ) ;
                    if ( instanceInfo.Instance is IViewModel vm && ReferenceEquals(vm, this) == false)
                    {
                        try
                        {
                            SoapFormatter x = new SoapFormatter ( ) ;
                            var serializationStream = new MemoryStream ( ) ;

                            x.Serialize ( serializationStream , vm ) ;
                            serializationStream.Flush ( ) ;
                            serializationStream.Seek ( 0 , SeekOrigin.Begin ) ;
                            byte[] buffer = new byte[ serializationStream.Length ] ;
                            serializationStream.Read (
                                                      buffer
                                                    , 0
                                                    , ( int ) serializationStream.Length
                                                     ) ;
                            var s = Encoding.UTF8.GetString ( buffer ) ;
                            DebugUtils.WriteLine ( s ) ;
                        }
                        catch ( Exception )
                        {
                            // ignored
                        }
                    }
                    // TODO implement
                    // ReSharper disable once UnusedVariable
                    if ( instanceInfo.Instance is LifetimeScope ls )
                    {
                    }

                    // if ( instanceInfo.Instance is FrameworkElement fe )
                    // {
                        // var res = CreateNode ( n3 , "Resources" , fe.Resources , true ) ;
                        // AddResourceNodeInfos ( res ) ;
                    // }

                    var n4 = CreateNode ( n3 , "Parameters" , instanceInfo.Parameters , true ) ;
                    foreach ( var p in instanceInfo.Parameters )
                    {
                        CreateNode ( n4 , p , p , false ) ;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="isValueChildren"></param>
        /// <returns></returns>
        [ NotNull ]
        public ResourceNodeInfo CreateNode (
            [ CanBeNull ] ResourceNodeInfo parent
          , object                         key
          , object                         data
          , bool ?                         isValueChildren = null
        )
        {
            var wrapped = WrapValue ( data ) ;
            var r = new ResourceNodeInfo
                    {
                        Key = key , Data = wrapped , IsValueChildren = isValueChildren
                    } ;
            if ( parent == null )
            {
                AllResourcesCollection.Add ( r ) ;
            }
            else
            {
                parent.Children.Add ( r ) ;
                r.Depth = parent.Depth + 1 ;
            }

            AllResourcesItemList.Add ( r ) ;
            return r ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <param name="node"></param>
        private void PopulateLifetimeScope ( [ NotNull ] ILifetimeScope lifetimeScope , ResourceNodeInfo node )
        {
            var regs = CreateNode (
                                   node
                                 , nameof ( lifetimeScope.ComponentRegistry.Registrations )
                                 , lifetimeScope.ComponentRegistry.Registrations
                                 , true
                                  ) ;

            foreach ( var reg in lifetimeScope.ComponentRegistry.Registrations )
            {
                var n2 = CreateNode ( regs , reg ,        reg ,          true ) ;
                var s = CreateNode ( n2 ,    "Services" , reg.Services , true ) ;
                foreach ( var regService in reg.Services )
                {
                    CreateNode ( s , regService.Description , regService , false ) ;
                }

                // PopulateInstances ( n2 , reg ) ;
            }

            if ( lifetimeScope is LifetimeScope ls )
            {
                var parentScope = ls.ParentLifetimeScope ;
                if ( parentScope != null )
                {
                    var parent = CreateNode ( node , "ParentLifetimeScope" , ls , true ) ;
                    PopulateLifetimeScope ( parentScope , parent ) ;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private object WrapValue ( object data )
        {
            var wrapped = data ;
            return wrapped ;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopulateResourcesTree ( )
        {
            if ( IsEnabledPopulateObjects )
            {
                PopulateObjects ( ) ;
            }

            PopulateLifetimeScope (
                                   _lifetimeScope
                                 , CreateNode ( null , "LifetimeScope" , _lifetimeScope , true )
                                  ) ;

        }

        #region Implementation of ISupportInitialize
        /// <summary>
        /// 
        /// </summary>
        public void BeginInit ( )
        {
            // if (IsInitializing)
            // {
            //     throw new InvalidOperationException("Cannot initialize twice");
            // }
            //
            // IsInitializing = true;
        }

        // public bool IsInitializing { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public void EndInit ( )
        {
            PopulateResourcesTree();
            IsInitialized = true ;
        }
        #endregion
        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <inheritdoc />
        public bool IsInitialized { get ; set ;  }

        /// <inheritdoc />
        public event EventHandler Initialized ;
        #endregion
    }
}