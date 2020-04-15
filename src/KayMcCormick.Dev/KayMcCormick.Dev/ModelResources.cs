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
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using System.Runtime.Serialization ;
using Autofac ;
using Autofac.Core.Lifetime ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Interfaces ;

namespace KayMcCormick.Dev
{
    /// <summary>
    ///     ViewModel designed to expose a hierarchy of resources in an application.
    /// </summary>
    public sealed class ModelResources : ISupportInitializeNotification , IViewModel
    {
        private const string Objects_Key    = "Objects" ;
        private const string Converters_Key = "Converters" ;

        private readonly ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        /// <summary>
        /// </summary>
        internal readonly IObjectIdProvider _idProvider ;

        /// <summary>
        /// </summary>
        private readonly ILifetimeScope _lifetimeScope ;

        private bool _doPopulateAppContext = true ;


        /// <summary>
        /// </summary>
        public ModelResources ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <param name="idProvider"></param>
        public ModelResources ( ILifetimeScope lifetimeScope , IObjectIdProvider idProvider )
        {
            _lifetimeScope = lifetimeScope ;
            _idProvider    = idProvider ;
        }

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
        /// </summary>
        public bool DoPopulateAppContext
        {
            get { return _doPopulateAppContext ; }
            set { _doPopulateAppContext = value ; }
        }

        #region Implementation of ISerializable
        /// <summary>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        /// <summary>
        /// </summary>
        private void PopulateObjects ( )
        {
            IEnumerable < ResourceNodeInfo > ObjectsChildrenFunc (
                ResourceNodeInfo                            node
              , Func < object , object , ResourceNodeInfo > createNode
            )
            {
                var rootNodes = _idProvider.GetRootNodes ( ).ToList ( ) ;
                return rootNodes.Select (
                                         rootNode => {
                                             var componentInfo =
                                                 _idProvider.GetComponentInfo ( rootNode ) ;
                                             var regs = _lifetimeScope
                                                       .ComponentRegistry.Registrations.Where (
                                                                                               r => r.Id
                                                                                                    == rootNode
                                                                                              ) ;
                                             if ( regs.Any ( ) )
                                             {
                                                 var reg = regs.First ( ) ;
                                                 var myInfo = new ComponentInfo ( ) ;
                                                 foreach ( var inst in componentInfo.Instances )
                                                 {
                                                     myInfo.Instances.Add ( inst ) ;
                                                 }

                                                 componentInfo.Metadata = reg.Metadata ;
                                                 componentInfo          = myInfo ;
                                             }

                                             var resourceNodeInfo = createNode (
                                                                                rootNode
                                                                              , componentInfo
                                                                               ) ;
                                             resourceNodeInfo.GetChildrenFunc =
                                                 ComponentChildrenFunc ;
                                             resourceNodeInfo.IsChildrenLoaded = false ;
                                             return resourceNodeInfo ;
                                         }
                                        ) ;
            }

            var n1 = CreateNode ( Objects_Key , ObjectsChildrenFunc ) ;
            // foreach ( var rootNode in _idProvider.GetRootNodes ( ) )
            // {
            // var c = _idProvider.GetComponentInfo ( rootNode ) ;
            // var n2 = CreateNode ( n1 , rootNode , c , true ) ;
            // foreach ( var instanceInfo in c.Instances )
            // {
            // var n3 = CreateNode ( n2 , instanceInfo , instanceInfo , true ) ;
            // var type = instanceInfo.Instance.GetType ( ) ;
            // if ( type.IsGenericType
            // && type.GetGenericTypeDefinition ( ) == typeof ( Meta <> ) )
            // {
            // var metadata = ( IDictionary < string , object > ) type
            // .GetProperty (
            // "Metadata"
            // )
            // ?.GetValue (
            // instanceInfo
            // .Instance
            // ) ;
            // var c4 = CreateNode ( n3 , "Metadata" , metadata , true ) ;
            // if ( metadata != null )
            // {
            // foreach ( var keyValuePair in metadata )
            // {
            // CreateNode ( c4 , keyValuePair.Key , keyValuePair.Value , false ) ;
            // }
            // }
            // }

            // CreateNode ( n3 , instanceInfo.Instance , instanceInfo.Instance , false ) ;
            // if ( instanceInfo.Instance is IViewModel vm
            // && ReferenceEquals ( vm , this ) == false )
            // {
            // try
            // {
            // var x = new SoapFormatter ( ) ;
            // var serializationStream = new MemoryStream ( ) ;

            // x.Serialize ( serializationStream , vm ) ;
            // serializationStream.Flush ( ) ;
            // serializationStream.Seek ( 0 , SeekOrigin.Begin ) ;
            // var buffer = new byte[ serializationStream.Length ] ;
            // serializationStream.Read (
            // buffer
            // , 0
            // , ( int ) serializationStream.Length
            // ) ;
            // var s = Encoding.UTF8.GetString ( buffer ) ;
            // DebugUtils.WriteLine ( s ) ;
            // }
            // catch ( Exception )
            // {
            // ignored
            // }
            // }

            // TODO implement
            // ReSharper disable once UnusedVariable
            // if ( instanceInfo.Instance is LifetimeScope ls )
            // {
            // }

            // if ( instanceInfo.Instance is FrameworkElement fe )
            // {
            // var res = CreateNode ( n3 , "Resources" , fe.Resources , true ) ;
            // AddResourceNodeInfos ( res ) ;
            // }

            // var n4 = CreateNode ( n3 , "Parameters" , instanceInfo.Parameters , true ) ;
            // foreach ( var p in instanceInfo.Parameters )
            // {
            // CreateNode ( n4 , p , p , false ) ;
            // }
        }

        [ NotNull ]
        private static IEnumerable < ResourceNodeInfo > ComponentChildrenFunc (
            [ NotNull ] ResourceNodeInfo                arg1
          , Func < object , object , ResourceNodeInfo > arg2
        )
        {
            var ci = ( ComponentInfo ) arg1.Data ;
            return ci.Instances.Select ( x => arg2 ( x , x ) ) ;
        }

        [ NotNull ]
        private ResourceNodeInfo CreateNode (
            string nodeKey
          , Func < ResourceNodeInfo , Func < object , object , ResourceNodeInfo > ,
                IEnumerable < ResourceNodeInfo > > getChildrenFunc
        )
        {
            var node = CreateNode ( null , nodeKey , null , false ) ;
            node.IsChildrenLoaded = false ;
            node.GetChildrenFunc  = getChildrenFunc ;
            return node ;
        }

        /// <summary>
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
          , bool                           addToChildren   = true
        )
        {
            var wrapped = WrapValue ( data ) ;
            var r = new ResourceNodeInfo
                    {
                        Key             = key
                      , Data            = wrapped
                      , IsValueChildren = isValueChildren
                      , CreateNodeFunc  = CreateNode
                    } ;
            if ( parent == null )
            {
                AllResourcesCollection.Add ( r ) ;
            }
            else
            {
                if ( addToChildren )
                {
                    parent.Children.Add ( r ) ;
                }

                r.Depth = parent.Depth + 1 ;
            }

            AllResourcesItemList.Add ( r ) ;
            return r ;
        }

        /// <summary>
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <param name="node"></param>
        private void PopulateLifetimeScope (
            [ NotNull ] ILifetimeScope lifetimeScope
          , ResourceNodeInfo           node
        )
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
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private object WrapValue ( object data )
        {
            var wrapped = data ;
            return wrapped ;
        }

        /// <summary>
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

            if ( DoPopulateAppContext )
            {
                PopulateAppContext (
                                    AppDomain.CurrentDomain
                                  , CreateNode (
                                                null
                                              , "Current App Domain"
                                              , AppDomain.CurrentDomain
                                              , true
                                               )
                                   ) ;
            }
        }

        private void PopulateAppContext (
            [ NotNull ] AppDomain currentDomain
          , ResourceNodeInfo      createNode
        )
        {
            // ReSharper disable once UnusedVariable
            var convNode = CreateNode ( createNode , Converters_Key , null , false ) ;

            foreach ( var assembly in currentDomain.GetAssemblies ( ) )
            {
                var anode = CreateNode (
                                        createNode
                                      , assembly.GetName ( ).Name
                                      , assembly
                                      , true
                                       ) ;

                if ( assembly.IsDynamic )
                {
                    continue ;
                }

                var res1 = CreateNode ( anode , "Resource" , null , false ) ;
                foreach ( var manifestResourceName in assembly.GetManifestResourceNames ( ) )
                {
                    var info = assembly.GetManifestResourceInfo ( manifestResourceName ) ;
                    var man1 = CreateNode ( res1 , manifestResourceName , info , true ) ;
                    var sub1 = CreateNode ( man1 , "Filename" ,           info.FileName ) ;
                }

                try
                {
                    var resName = assembly.GetName ( ).Name + ".g.resources" ;
                    using ( var stream = assembly.GetManifestResourceStream ( resName ) )
                    {
                        if ( stream != null )
                        {
                            using ( var reader = new System.Resources.ResourceReader ( stream ) )
                            {
                                foreach ( var dictionaryEntry in reader.Cast < DictionaryEntry > ( )
                                )
                                {
                                    UnmanagedMemoryStream s = ( UnmanagedMemoryStream ) dictionaryEntry.Value ;
                                    
                                    CreateNode (
                                                res1
                                              , dictionaryEntry.Key
                                              , dictionaryEntry.Value
                                              , false
                                               ) ;
                                }
                            }
                        }
                    }
                }
                catch
                {

                }

                var exported = CreateNode ( anode , "Exported Types" , null , false ) ;

                foreach ( var typ in assembly.ExportedTypes )
                {
                    // var cc = typ.GetCustomAttribute < TypeConverterAttribute > ( ) ;
                    var typnode = CreateNode ( exported , typ.FullName , typ , true ) ;
                    // var conv = CreateNode ( typnode , "Converter" , null , false ) ;

                    // TypeConverter converter = null ;
                    // try
                    // {
                    // converter = TypeDescriptor.GetConverter ( typ.UnderlyingSystemType ) ;
                    // }
                    // catch ( Exception ex )
                    // {

                    // }

                    // var c11 = CreateNode ( conv, converter , converter , true ) ;
                    // CreateNode(convNode, converter, converter, true);
                    var bases = CreateNode ( typnode , "Bases" ,      null , false ) ;
                    var atts = CreateNode ( typnode ,  "Attributes" , null , false ) ;
                    try
                    {
                        foreach ( var c in typ.CustomAttributes )
                        {
                            var at1 = CreateNode ( atts , c.AttributeType.FullName , c , false ) ;
                            for ( var i = 0 ; i < c.Constructor.GetParameters ( ).Length ; i ++ )
                            {
                                var ci = c.Constructor.GetParameters ( )[ i ] ;
                                CreateNode ( at1 , ci.Name , c.ConstructorArguments[ i ] , false ) ;
                            }
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    var bas = typ.BaseType ;
                    while ( bas != null )
                    {
                        // ReSharper disable once UnusedVariable
                        var basenode = CreateNode ( bases , bas.FullName , bas , true ) ;
                        bas = bas.BaseType ;
                    }

                    var props = CreateNode ( typnode , "Properties" , null , false ) ;
                    foreach ( var propertyInfo in typ.GetProperties (
                                                                     BindingFlags.Instance
                                                                     | BindingFlags.Public
                                                                    ) )
                    {
                        // ReSharper disable once UnusedVariable
                        var p = CreateNode ( props , propertyInfo.Name , propertyInfo , true ) ;
                    }

                    var methods = CreateNode ( typnode , "Methods" , null , false ) ;
                    foreach ( var methodInfo in typ
                                               .GetMethods (
                                                            BindingFlags.Instance
                                                            | BindingFlags.Public
                                                           )
                                               .Where ( m => ! m.IsSpecialName ) )
                    {
                        // ReSharper disable once UnusedVariable
                        var p = CreateNode (
                                            methods
                                          , methodInfo.ToString ( )
                                          , methodInfo
                                          , true
                                           ) ;
                    }
                }
            }
        }

        #region Implementation of ISupportInitialize
        /// <summary>
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
        /// </summary>
        public void EndInit ( )
        {
            PopulateResourcesTree ( ) ;
            IsInitialized = true ;
        }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <inheritdoc />
        public bool IsInitialized { get ; set ; }

        /// <inheritdoc />
        public event EventHandler Initialized ;
        #endregion
    }
}