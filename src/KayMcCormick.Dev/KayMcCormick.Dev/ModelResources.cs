using System ;
using System.Collections ;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Linq ;
using System.Reactive.Concurrency ;
using System.Reactive.Linq ;
using System.Reflection ;
using System.Runtime.Serialization ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Lifetime ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Interfaces ;
using KayMcCormick.Dev.Metadata ;

// ReSharper disable InconsistentNaming

namespace KayMcCormick.Dev
{
    /// <summary>
    ///     ViewModel designed to expose a hierarchy of resources in an application.
    /// </summary>
    public sealed class ModelResources : ISupportInitializeNotification , IViewModel
    {
        public object InstanceObjectId { get; set; }

        private const string Objects_Key    = "Objects" ;
        private const string Converters_Key = "Converters" ;

        private readonly ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        /// <summary>
        /// </summary>
        internal readonly IObjectIdProvider _idProvider ;

        /// <summary>
        /// </summary>
        private ILifetimeScope _lifetimeScope ;

        // ReSharper disable once RedundantDefaultMemberInitializer
        private          bool                                    _doPopulateAppContext = true;
        private          ResourceNodeInfo                        _objects_node ;
        private readonly bool                                    _flatten_objects_node ;
        private IObservable < IComponentRegistration >  _regObservable ;
        private readonly ConcurrentDictionary < Type , MyInfo2 > _activators ;
        private          bool                                    _regSubscribed ;


        /// <summary>
        /// </summary>
        public ModelResources ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="lifetimeScope"></param>
        /// <param name="idProvider"></param>
        /// <param name="test"></param>
        public ModelResources (
            ILifetimeScope                          lifetimeScope
          , IObjectIdProvider                       idProvider
          , bool                                    test          = true
          , IObservable < IComponentRegistration >  regObservable = null
          , ConcurrentDictionary < Type , MyInfo2 > activators    = null
        )
        {
            _lifetimeScope        = lifetimeScope ;
            _idProvider           = idProvider ;
            _flatten_objects_node = test ;
            _regObservable        = regObservable ;
            _activators           = activators ;
            if (activators != null)
                DebugUtils.WriteLine(
                    string.Join(
                        ", "
                        , activators
                            .Values.Where(x => x.Registrations.Count > 1)
                            .Select(x2 => x2.LimitType)
                    )
                );
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
            ObjectsNode = _flatten_objects_node
                              ? CreateNode ( Objects_Key , FlatObjectsChildrenFunc )
                              : CreateNode ( Objects_Key , ObjectsChildrenFunc ) ;

            // foreach ( var rootNode in _idProvider.GetRootNodes ( ) )
            // {
            // var c = _idProvider.GetComponentInfo ( rootNode ) ;
            // var n2 = CreateNode ( n1 , rootNode , c , true ) ;
            // foreach ( var instanceInfo in c.Instances )
            // {Inst
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="createNode"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public IEnumerable < ResourceNodeInfo > FlatObjectsChildrenFunc (
            ResourceNodeInfo                            node
          , Func < object , object , ResourceNodeInfo > createNode
        )
        {
            var rootNodes = _idProvider.GetRootNodes ( ).ToList ( ) ;
            return rootNodes.SelectMany (
                                         rootNode => LifetimeScope
                                                    .ComponentRegistry.Registrations
                                                    .Where ( reg1 => reg1.Id == rootNode )
                                                    .Select (
                                                             reg1 => new ComponentInfo
                                                                     {
                                                                         Id       = rootNode
                                                                       , Metadata = reg1.Metadata
                                                                       , InstanceEnumeration =
                                                                             _idProvider
                                                                                .GetComponentInfo (
                                                                                                   reg1
                                                                                                      .Id
                                                                                                  )
                                                                                .Instances
                                                                                .Select (
                                                                                         i1
                                                                                             => new
                                                                                                InstanceInfo
                                                                                                {
                                                                                                    Instance
                                                                                                        = i1
                                                                                                           .Instance
                                                                                                  , Metadata
                                                                                                        = reg1
                                                                                                           .Metadata
                                                                                                }
                                                                                        )
                                                                     }
                                                            )
                                        )
                            .SelectMany (
                                         x11 => x11
                                               .Instances
                                               .Select (
                                                        ii1 => new
                                                            Flattened < ComponentInfo , InstanceInfo
                                                            > ( x11 , ii1 )
                                                       )
                                               .Select ( xx => createNode ( xx , xx ) )
                                        ) ;
        }

        [ JetBrains.Annotations.NotNull ]
        private IEnumerable < ResourceNodeInfo > ObjectsChildrenFunc (
            ResourceNodeInfo                            node
          , Func < object , object , ResourceNodeInfo > createNode
        )
        {
            var rootNodes = _idProvider.GetRootNodes ( ).ToList ( ) ;
            return rootNodes.Select (
                                     rootNode => {
                                         var componentInfo =
                                             _idProvider.GetComponentInfo ( rootNode ) ;
                                         var regs =
                                             LifetimeScope.ComponentRegistry.Registrations.Where (
                                                                                                  r => r.Id
                                                                                                       == rootNode
                                                                                                 ) ;
                                         var componentRegistrations =
                                             regs as IComponentRegistration[] ?? regs.ToArray ( ) ;
                                         if ( componentRegistrations.Any ( ) )
                                         {
                                             var reg = componentRegistrations.First ( ) ;
                                             var myInfo = new ComponentInfo ( ) ;
                                             // ReSharper disable once UnusedVariable
                                             foreach ( var ii in componentInfo.Instances.Select (
                                                                                                 inst
                                                                                                     => new
                                                                                                        InstanceInfo
                                                                                                        {
                                                                                                            Instance
                                                                                                                = inst
                                                                                                                   .Instance
                                                                                                          , Metadata
                                                                                                                = reg
                                                                                                                   .Metadata
                                                                                                        }
                                                                                                ) )
                                             {
                                             }

                                             componentInfo.Metadata = reg.Metadata ;
                                             componentInfo          = myInfo ;
                                         }

                                         var componentInfoNode =
                                             createNode ( rootNode , componentInfo ) ;
                                         componentInfoNode.GetChildrenFunc =
                                             ComponentChildrenFunc ;
                                         componentInfoNode.IsChildrenLoaded = false ;
                                         return componentInfoNode ;
                                     }
                                    ) ;
        }

        [ JetBrains.Annotations.NotNull ]
        private static IEnumerable < ResourceNodeInfo > ComponentChildrenFunc (
            [ JetBrains.Annotations.NotNull ] ResourceNodeInfo                arg1
          , Func < object , object , ResourceNodeInfo > arg2
        )
        {
            var ci = ( ComponentInfo ) arg1.Data ;
            return ci.Instances.Select ( x => arg2 ( x , x ) ) ;
        }

        [ JetBrains.Annotations.NotNull ]
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
        /// <param name="addToChildren"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public ResourceNodeInfo CreateNode (
            [ CanBeNull ] ResourceNodeInfo parent
          , object                         key
          , object                         data
          , bool ?                         isValueChildren = null
          , bool                           addToChildren   = true
        )
        {
            var wrapped = WrapValue ( data ) ;
            var r = ResourceNodeInfo.CreateInstance ( ) ;
            r.Parent          = parent ;
            r.Key             = key ;
            r.Data            = wrapped ;
            r.IsValueChildren = isValueChildren ;
            r.CreateNodeFunc  = CreateNode ;
            if ( parent == null )
            {
                AllResourcesCollection.Add ( r ) ;
            }
            else
            {
                if ( addToChildren )
                {
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
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
            [ JetBrains.Annotations.NotNull ] IComponentContext lifetimeScope
          , ResourceNodeInfo              node
        )
        {
            DebugUtils.WriteLine ( "Creating regs node" ) ;
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

            if ( ! ( lifetimeScope is LifetimeScope ls ) )
            {
                return ;
            }

            var parentScope = ls.ParentLifetimeScope ;
            if ( parentScope == null )
            {
                return ;
            }

            var parent = CreateNode ( node , "ParentLifetimeScope" , ls , true ) ;
            PopulateLifetimeScope ( parentScope , parent ) ;
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

            if ( LifetimeScope != null )
            {
                var resourceNodeInfo =
                    CreateNode ( null , "LifetimeScope" , LifetimeScope , true ) ;
                if ( ! _regSubscribed )
                {
                   
                }

                PopulateLifetimeScope ( LifetimeScope , resourceNodeInfo ) ;
            }

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

        // ReSharper disable once FunctionComplexityOverflow
        private void PopulateAppContext (
            [ JetBrains.Annotations.NotNull ] AppDomain currentDomain
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
                    CreateNode ( man1 , "Filename" , info.FileName ) ;
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
                    // ignored
                }

                var exported = CreateNode ( anode , "Exported Types" , assembly , false ) ;
                exported.GetChildrenFunc = AssemblyTypes ;
                

#if false
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
                        // ReSharper disable once UnusedVariable
                        foreach ( var at1 in typ.CustomAttributes.Select (
                                                                          c => CreateNode (
                                                                                           atts
                                                                                         , c
                                                                                          .AttributeType
                                                                                          .FullName
                                                                                         , c
                                                                                         , false
                                                                                          )
                                                                         ) ) { }
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
                    // ReSharper disable once UnusedVariable
                    foreach ( var p in typ
                                      .GetProperties ( BindingFlags.Instance | BindingFlags.Public )
                                      .Select (
                                               propertyInfo
                                                   => CreateNode (
                                                                  props
                                                                , propertyInfo.Name
                                                                , propertyInfo
                                                                , true
                                                                 )
                                              ) ) { }

                    var methods = CreateNode ( typnode , "Methods" , null , false ) ;
                    // ReSharper disable once UnusedVariable
                    foreach ( var p in typ
                                      .GetMethods ( BindingFlags.Instance | BindingFlags.Public )
                                      .Where ( m => ! m.IsSpecialName )
                                      .Select (
                                               methodInfo
                                                   => CreateNode (
                                                                  methods
                                                                , methodInfo.ToString ( )
                                                                , methodInfo
                                                                , true
                                                                 )
                                              ) ) { }
                }
#endif
            }
        }

        private IEnumerable < ResourceNodeInfo > AssemblyTypes (
            ResourceNodeInfo                            arg1
          , Func < object , object , ResourceNodeInfo > arg2
        )
        {
            var a = (Assembly) arg1.Data;
            return a.ExportedTypes.Select(t => arg2(t.FullName, t));
        }

#region Implementation of ISupportInitialize
        /// <summary>
        /// </summary>
        public void BeginInit ( )
        {
            // if (IsInitializing)
            // {
            //     throw new AppInvalidOperationException("Cannot initialize twice");
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

        /// <summary>
        /// 
        /// </summary>
        public ResourceNodeInfo ObjectsNode
        {
            get { return _objects_node ; }
            set { _objects_node = value ; }
        }

        /// <summary>
        /// </summary>
        public ILifetimeScope LifetimeScope
        {
            get { return _lifetimeScope ; }
            set { _lifetimeScope = value ; }
        }

        /// <inheritdoc />
        public event EventHandler Initialized ;
#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public sealed class Flattened < T , T1 > : IProvidesKey
    {
        private readonly T _arg1 ;

        // ReSharper disable once NotAccessedField.Local
        private readonly T1 _arg2 ;

        /// <summary>
        /// 
        /// </summary>
        public Flattened ( T arg1 , T1 arg2 )
        {
            _arg1 = arg1 ;
            _arg2 = arg2 ;
        }

#region Implementation of IProvidesKey
        /// <inheritdoc />
        public object GetKey ( ) { return _arg1 ; }
#endregion
    }
}