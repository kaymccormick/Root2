#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// AllResourcesTreeViewModel.cs
// 
// 2020-03-16-7:25 PM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Linq ;
using System.Runtime.Serialization ;
using System.Threading ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf.ViewModel
{
    /// <summary>
    /// A View model representing all of the resources in the application, building on
    ///<see cref="ModelResources"/>
    /// </summary>
    [ ContentProperty ( "ModelResources" ) ]
    public sealed class AllResourcesTreeViewModel : IViewModel , ISupportInitializeNotification
    {
        private ModelResources _modelResources ;

        private readonly ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        private ResourceNodeInfo _appNode ;
        // ReSharper disable once RedundantDefaultMemberInitializer
        private bool _doPopulateXamlSchema = false;

        /// <summary>
        /// 
        /// </summary>
        public AllResourcesTreeViewModel ( ) { }

        /// <summary>
        /// </summary>
        public ObservableCollection < ResourceNodeInfo > AllResourcesItemList { get ; set ; } =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        /// <summary>
        /// </summary>
        /// <param name="modelResources"></param>
        public AllResourcesTreeViewModel ( ModelResources modelResources )
        {
            _modelResources = modelResources ;
        }

        /// <summary>
        /// </summary>
        public ObservableCollection < ResourceNodeInfo > AllResourcesCollection
        {
            get { return _allResourcesCollection ; }
        }
#if false
        private void PopulateInstances (
            ResourceNodeInfo       res
, IComponentRegistration registration
        )
        {
            var n = CreateNode ( res , "Instances" , null , false ) ;
            foreach ( var instanceInfo in
                _m.GetInstanceByComponentRegistration ( registration ) )
            {
                var key = _idProvider.GetObjectInstanceIdentifier ( instanceInfo.Instance ) ;
                CreateNode (
                            n
          , new CompositeResourceNodeKey ( key , instanceInfo.Instance )
          , instanceInfo.Parameters
          , true
                           ) ;
            }
        }
#endif
        [ NotNull ]
        private ResourceNodeInfo CreateNode (
            [ CanBeNull ] IHierarchicalNode parent
          , object                          key
          , object                          data
          , bool ?                          isValueChildren = null
        )
        {
            var wrapped = WrapValue ( data ) ;
            var r = ResourceNodeInfo.CreateInstance ( ) ;
            r.Key = key ;
            r.Data = wrapped ;
            r.IsValueChildren = isValueChildren ;
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

        private void PopulateAppNode ( )
        {
            var current = Application.Current ;
            _appNode = CreateNode ( null , "Application" , current , false ) ;

            if ( DoPopulateXamlSchema )
            {
                PopulateXamlSchema ( ) ;
            }

            if ( current == null )
            {
                return ;
            }

            var appResources = CreateNode ( _appNode , "Resources" , current.Resources , true ) ;
            AddResourceNodeInfos ( appResources ) ;

            DebugUtils.WriteLine ( Thread.CurrentThread.ManagedThreadId.ToString ( ) ) ;
            foreach ( Window currentWindow in current.Windows )
            {
                HandleWindow ( currentWindow ) ;
            }
        }

        /// <summary>
        /// Whether or not to populate the tree with XAML Schema related nodes.
        /// </summary>
        public bool DoPopulateXamlSchema { get { return _doPopulateXamlSchema ; } set { _doPopulateXamlSchema = value ; } }

        private void PopulateXamlSchema ( )
        {
            var schemaContext = XamlReader.GetWpfSchemaContext ( ) ;
            var scmNode = CreateNode ( _appNode , schemaContext , schemaContext , false ) ;
            foreach ( var member1 in
                from ns in schemaContext.GetAllXamlNamespaces ( )
                let ns1 = CreateNode ( scmNode , ns , ns , false )
                from allXamlType in schemaContext.GetAllXamlTypes ( ns )
                let t1 = CreateNode ( ns1 , allXamlType , allXamlType , false )
                from xamlMember in allXamlType.GetAllMembers ( )
                select new { node = CreateNode ( t1 , xamlMember , xamlMember , false ) , xamlMember } )
            {
                var n = member1.xamlMember.DeclaringType.Name ;
                var n2 = member1.xamlMember.Name ;

                DebugUtils.WriteLine ( $"{n} {n2}" ) ;
            }
        }

        private void HandleWindow ( [ NotNull ] Window w )
        {
            var winNode = CreateNode (
                                      _appNode
                                    , w.GetType ( )
                                    , new ControlWrap < Window > ( w )
                                     ) ;
            winNode.TemplateKey = "WindowTemplate" ;


            // ReSharper disable once UnusedVariable
            var winRes = CreateNode ( winNode , "Resources" , w.Resources , true ) ;
            AddResourceNodeInfos ( winRes ) ;

            try
            {
                var log = CreateNode ( winNode , "Logical children" , null , false ) ;
                AddLogicalChildren ( w , log ) ;
            }
            catch
            {
                // ignored
            }
        }

        private void AddLogicalChildren ( [ NotNull ] DependencyObject w , IHierarchicalNode log )
        {
            DebugUtils.WriteLine ( w.ToString ( ) ) ;
            var children = LogicalTreeHelper.GetChildren ( w ) ;
            foreach ( var child in
                from object child in children
                let logChild = CreateNode ( log , child.ToString ( ) , null , false )
                select child )
            {
                // ReSharper disable once UnusedVariable
                if ( child is DependencyObject @do )
                {
                    // ReSharper disable once CommentTypo
                    //AddLogicalChildren ( ( DependencyObject ) @do , logchild ) ;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="resNode"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        // ReSharper disable once FunctionComplexityOverflow
        private void AddResourceNodeInfos ( [ NotNull ] ResourceNodeInfo resNode )
        {
            if ( resNode == null )
            {
                throw new ArgumentNullException ( nameof ( resNode ) ) ;
            }

            var resNodeData = resNode.Data ;
            var res = ( ResourceDictionary ) resNodeData ;

            if ( res == null )
            {
                return ;
            }

            foreach ( var mdr in res.MergedDictionaries.Select (
                                                                md => CreateNode (
                                                                                  resNode
                                                                                , md.Source
                                                                                , md
                                                                                , true
                                                                                 )
                                                               ) )
            {
                AddResourceNodeInfos ( mdr ) ;
            }

            try
            {
                foreach ( DictionaryEntry haveResourcesResource in res )
                {
                    if ( haveResourcesResource.Key      == null
                         || haveResourcesResource.Value == null )
                    {
                        continue ;
                    }

                    var key = haveResourcesResource.Key ;
                    if ( key is ResourceKey rkey )
                    {
                        switch ( rkey )
                        {
                            // ReSharper disable once UnusedVariable
                            case ComponentResourceKey componentResourceKey : break ;


                            // ReSharper disable once UnusedVariable
                            case ItemContainerTemplateKey itemContainerTemplateKey : break ;

                            // ReSharper disable once UnusedVariable
                            case DataTemplateKey dataTemplateKey : break ;

                            // ReSharper disable once UnusedVariable
                            case TemplateKey templateKey : break ;
                            default :
                                key = new ResourceKeyWrapper < ResourceKey > ( rkey ) ;
                                break ;
                        }
                    }



                    var resourceInfo = CreateNode ( resNode , key , haveResourcesResource.Value ) ;
                    switch ( haveResourcesResource.Value )
                    {
                        case FrameworkTemplate ft :
                        {
                            var resourcesNode = CreateNode (
                                                            resourceInfo
                                                          , "Resources"
                                                          , ft.Resources
                                                           ) ;
                            AddResourceNodeInfos ( resourcesNode ) ;
                            break ;
                        }
                        case Style sty :
                        {
                            var settersNode = CreateNode ( resourceInfo , "Setters" , null ) ;
                            settersNode.IsExpanded = true ;
                            foreach ( var setter in sty.Setters )
                            {
                                switch ( setter )
                                {
                                    case EventSetter _ : break ;
                                    case Setter setter1 :
                                        CreateNode (
                                                    settersNode
                                                  , setter1.Property
                                                  , setter1.Value
                                                   ) ;

                                        break ;
                                    default : throw new InvalidOperationException ( ) ;
                                }
                            }

                            break ;
                        }
                    }

                    if ( haveResourcesResource.Value is IDictionary dict )
                    {
                        foreach ( var key2 in dict.Keys )
                        {
                            CreateNode ( resourceInfo , key2 , dict[ key2 ] ) ;
                        }
                    }
                    else
                    {
                        if ( ! ( haveResourcesResource.Value is IEnumerable enumerable )
                             || haveResourcesResource.Value is string )
                        {
                            continue ;
                        }

                        foreach ( var child in enumerable )
                        {
                            CreateNode ( resourceInfo , child , null ) ;
                        }
                    }
                }
            }
            catch ( Exception )
            {
                // ignored
            }
        }

        #region Overrides of ModelResources
        /// <summary>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static object WrapValue ( object data )
        {
            if ( data is UIElement uie )
            {
                return new ControlWrap < UIElement > ( uie ) ;
            }

            return data ;
        }

        /// <summary>
        /// </summary>
        private void PopulateResourcesTree ( ) { PopulateAppNode ( ) ; }
        #endregion
        #region Implementation of ISupportInitialize
        /// <inheritdoc />
        public void BeginInit ( )
        {
            // if ( IsInitializing )
            // {
            //     throw new InvalidOperationException("Cannot initialize twice");
            // }
            //
            // IsInitializing = true ;
        }

//public bool IsInitializing { get ; set ; }

        /// <inheritdoc />
        public void EndInit ( )
        {
            foreach ( var resourceNodeInfo in ModelResources.AllResourcesCollection )
            {
                AllResourcesCollection.Add ( resourceNodeInfo ) ;
            }

            PopulateResourcesTree ( ) ;
            IsInitialized = true ;
        }
        #endregion
        #region Implementation of ISerializable
        /// <inheritdoc />
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion

        #region Implementation of ISupportInitializeNotification
        /// <inheritdoc />
        public bool IsInitialized { get ; private set ; }

        /// <summary>
        /// 
        /// </summary>
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Content ) ]
        public ModelResources ModelResources
        {
            get { return _modelResources ; }
            set { _modelResources = value ; }
        }

        /// <inheritdoc />
        public event EventHandler Initialized ;
        #endregion
    }
}