#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// AnalysisControlsModule.cs
// 
// 2020-03-06-12:50 PM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Reflection ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Markup ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.Properties ;
using AnalysisControls.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using Autofac.Core ;
using JetBrains.Annotations ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Module = Autofac.Module ;
using XamlWriter = System.Windows.Markup.XamlWriter ;

namespace AnalysisControls
{
    // made internal 3/11
    /// <summary>
    /// 
    /// </summary>
    public sealed class AnalysisControlsModule : Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load ( [ NotNull ] ContainerBuilder builder )
        {
#if false
            builder.RegisterAssemblyTypes(ThisAssembly).Where(type => {
                                                                  var isAssignableFrom = typeof ( IViewModel )
                                                                                            .IsAssignableFrom (
                                                                                                               type
                                                                                                              )
                                                                                         || typeof ( IView1 )
                                                                                            .IsAssignableFrom (
                                                                                                               type
                                                                                                              ) ;
                                                                  return isAssignableFrom ;
                                                              }              ).AsImplementedInterfaces().AsSelf().WithAttributedMetadata();

#else
            builder.RegisterType < TypesView > ( )
                   .AsSelf ( )
                   .As<IControlView> (  )
                   .WithMetadata (
                                  "ImageSource"
                                , 
                                           "pack://application:,,,/KayMcCormick.Lib.Wpf;component/Assets/StatusAnnotations_Help_and_inconclusive_32xMD_color.png"
                                          
                                 )
                   .WithMetadata ( "Ribbon" , true ) ;



            //
            // var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            // foreach ( var name in names )
            // {
            //     var info = Assembly.GetExecutingAssembly ( ).GetManifestResourceInfo ( name ) ;
            //     Debug.WriteLine ( info.ResourceLocation ) ;
            //
            // }

            builder.Register ( ( context , parameters ) => {
                var stream = Assembly.GetExecutingAssembly ( )
                                     .GetManifestResourceStream (
                                                                 "AnalysisControls.TypesViewModel.xaml"
                                                                 
                                                                ) ;
                if ( stream == null )
                {
                    return new TypesViewModel();
                } else 
                                  {
                                      try
                                      {
                                          TypesViewModel v =
                                              ( TypesViewModel ) XamlReader.Load ( stream ) ;
                                          stream.Close ( ) ;
                                          return v ;
                                      }
                                      catch ( Exception )
                                      {
                                          return new TypesViewModel();
                    }
                                  }
                              }
                             )
                   .AsSelf ( )
                   .AsImplementedInterfaces ( ) ;
            
            //TypesViewModelContainer x = new TypesViewModelContainer();
            //x.VDocelems = new DocumentCollection(v.Docelems);
            // var xml = XamlWriter.Save(v);
            // Debug.WriteLine ( xml ) ;
            // builder.RegisterInstance(v).As<ITypesViewModel> (  ).SingleInstance();
            
            //builder.RegisterType < TypesViewModel > ( ).As < ITypesViewModel > ( ) ;
            builder.RegisterType < SyntaxPanel > ( )
                   .Keyed < IControlView > ( typeof ( CompilationUnitSyntax ) )
                   .AsSelf ( ) ;
            builder.RegisterType < SyntaxPanelViewModel > ( )
                   .AsImplementedInterfaces ( )
                   .AsSelf ( ) ;
#endif

            // builder.Register (
            //                   ( c , p ) => {
            //                       var listView = Func ( c , p ) ;
            //                       return new ContentControlView { Content = listView } ;
            //                   }
            //                  )
            //        .WithMetadata ( "Title" , "Syntax Token View" )
            //        .As < IControlView > ( ) ;
            // builder.RegisterType < PythonViewModel > ( ).AsSelf ( ) ;
        }

        [ NotNull ]
        private FrameworkElement Func ( [ NotNull ] IComponentContext c1 , IEnumerable < Parameter > p1 )
        {
            var gridView = new GridView ( ) ;
            gridView.Columns.Add (
                                  new GridViewColumn
                                  {
                                      DisplayMemberBinding = new Binding ( "SyntaxKind" )
                                    , Header               = "Kind"
                                  }
                                 ) ;
            gridView.Columns.Add (
                                  new GridViewColumn
                                  {
                                      DisplayMemberBinding = new Binding ( "Token" )
                                    , Header               = "Token"
                                  }
                                 ) ;
            gridView.Columns.Add (
                                  new GridViewColumn
                                  {
                                      Header               = "Raw Kind"
                                    , DisplayMemberBinding = new Binding ( "RawKind" )
                                  }
                                 ) ;

            var binding =
                new Binding ( "SyntaxItems" )
                {
                    Source = c1.Resolve < ISyntaxTokenViewModel > ( )
                } ;
            var listView = new ListView { View = gridView } ;
            listView.SetBinding ( ItemsControl.ItemsSourceProperty , binding ) ;
            return listView ;
        }


        // builder.RegisterAssemblyTypes ( ThisAssembly )
        // .Where (
        // type => typeof ( IViewWithTitle ).IsAssignableFrom ( type )
        // || typeof ( IViewModel ).IsAssignableFrom ( type )
        // || typeof ( AppWindow ).IsAssignableFrom ( type )
        // )
        // .AsSelf ( )
        // .AsImplementedInterfaces ( ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    [ContentProperty("VDocelems")]
    public class TypesViewModelContainer
    {
        /// <summary>
        /// 
        /// </summary>
        public DocumentCollection VDocelems { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public TypesViewModelContainer ( ) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vDocelems"></param>
        public TypesViewModelContainer (
            DocumentCollection vDocelems
        )
        {
            VDocelems = vDocelems ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DocumentCollection : IList, ICollection, IEnumerable 

    {
        private IList _list ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public DocumentCollection ( IList list ) { _list = list ; }

        /// <summary>
        /// 
        /// </summary>
        public DocumentCollection ( ) {
            _list = new ArrayList();
        }

        #region Implementation of IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator ( ) { return _list.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo ( Array array , int index ) { _list.CopyTo ( array , index ) ; }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return _list.Count ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot
        {
            get { return _list.SyncRoot ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get { return _list.IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add ( object value ) { return _list.Add ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains ( object value ) { return _list.Contains ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        public void Clear ( ) { _list.Clear ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf ( object value ) { return _list.IndexOf ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert ( int index , object value ) { _list.Insert ( index , value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Remove ( object value ) { _list.Remove ( value ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt ( int index ) { _list.RemoveAt ( index ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public object this [ int index ]
        {
            get { return _list[ index ] ; }
            set { _list[ index ] = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return _list.IsReadOnly ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize
        {
            get { return _list.IsFixedSize ; }
        }
        #endregion
    }
}