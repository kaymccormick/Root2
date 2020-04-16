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
    /// </summary>
    public sealed class AllResourcesTreeViewModel : IViewModel , ISupportInitializeNotification
    {
        private readonly ModelResources _modelResources ;

        private readonly ObservableCollection < ResourceNodeInfo > _allResourcesCollection =
            new ObservableCollection < ResourceNodeInfo > ( ) ;

        private ResourceNodeInfo _appNode ;

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
                parent.Children?.Add ( r ) ;

                r.Depth = parent.Depth + 1 ;
            }

            AllResourcesItemList.Add ( r ) ;
            return r ;
        }

        private void PopulateAppNode ( )
        {
            var current = Application.Current ;
            _appNode = CreateNode ( null , "Application" , current , false ) ;

            var schemaContext = XamlReader.GetWpfSchemaContext ( ) ;
            var scmNode = CreateNode ( _appNode , schemaContext , schemaContext , false ) ;
            foreach ( var ns in schemaContext.GetAllXamlNamespaces ( ) )
            {
                var ns1 = CreateNode ( scmNode , ns , ns , false ) ;
                foreach ( var allXamlType in schemaContext.GetAllXamlTypes ( ns) )
                {
                    var t1 = CreateNode ( ns1 , allXamlType , allXamlType , false ) ;
                    foreach ( var xamlMember in allXamlType.GetAllMembers ( ) )
                    {
                        // ReSharper disable once UnusedVariable
                        var member1 = CreateNode ( t1 , xamlMember , xamlMember , false ) ;
                    }
                }
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

        private void AddLogicalChildren ( [ NotNull ] DependencyObject w , ResourceNodeInfo log )
        {
            DebugUtils.WriteLine ( w.ToString ( ) ) ;
            var children = LogicalTreeHelper.GetChildren ( w ) ;
            foreach ( var child in children )
            {
                var logchild = CreateNode ( log , child.ToString() , null , false ) ;
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

            foreach ( var md in res.MergedDictionaries )
            {
                var mdr = CreateNode ( resNode , md.Source , md , true ) ;
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
                    if ( haveResourcesResource.Value is FrameworkTemplate ft )
                    {
                        var resourcesNode =
                            CreateNode ( resourceInfo , "Resources" , ft.Resources ) ;
                        AddResourceNodeInfos ( resourcesNode ) ;
                    }

                    if ( haveResourcesResource.Value is Style sty )
                    {
                        var settersNode = CreateNode ( resourceInfo , "Setters" , null ) ;
                        settersNode.IsExpanded = true ;
                        foreach ( var setter in sty.Setters )
                        {
                            switch ( setter )
                            {
                                case EventSetter _ : break ;
                                case Setter setter1 :
                                    CreateNode ( settersNode , setter1.Property , setter1.Value ) ;

                                    break ;
                                default : throw new InvalidOperationException ( ) ;
                            }
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
        private object WrapValue ( object data )
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
            foreach ( var resourceNodeInfo in _modelResources.AllResourcesCollection )
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
        public bool IsInitialized { get ; set ; }

        /// <inheritdoc />
        public event EventHandler Initialized ;
        #endregion
    }
}