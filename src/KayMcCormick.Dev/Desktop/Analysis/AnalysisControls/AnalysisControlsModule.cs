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
using System.Collections.ObjectModel ;
using System.Diagnostics ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Markup ;
using System.Xaml ;
using AnalysisAppLib.ViewModel ;
using AnalysisControls.ViewModel ;
using AnalysisControls.Views ;
using Autofac ;
using Autofac.Core ;
using Autofac.Extras.AttributeMetadata ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
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
                   .AsImplementedInterfaces ( )
                   .WithMetadata ( "ImageSource" , new Uri("pack://application:,,,/KayMcCormick.Lib.Wpf;component/Assets/StatusAnnotations_Help_and_inconclusive_32xMD_color.png")) ;
            TypesViewModel v = new TypesViewModel();
            TypesViewModelContainer x = new TypesViewModelContainer();
            x.VDocelems = new DocumentCollection(v.Docelems);
            var xml = XamlWriter.Save(x);
            Debug.WriteLine ( xml ) ;
            builder.RegisterInstance(v).SingleInstance();
            
            //builder.RegisterType < TypesViewModel > ( ).As < ITypesViewModel > ( ) ;
            builder.RegisterType < SyntaxPanel > ( ).AsImplementedInterfaces ( ).AsSelf ( ) ;
            builder.RegisterType < SyntaxPanelViewModel > ( )
                   .AsImplementedInterfaces ( )
                   .AsSelf ( ) ;
#endif

            builder.Register (
                              ( c , p ) => {
                                  var listView = Func ( c , p ) ;
                                  return new ContentControlView { Content = listView } ;
                              }
                             )
                   .WithMetadata ( "Title" , "Syntax Token View" )
                   .As < IControlView > ( ) ;
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

    [ContentProperty("VDocelems")]
    public class TypesViewModelContainer
    {
        public DocumentCollection VDocelems { get ; set ; }

        public TypesViewModelContainer ( ) {
        }

        public TypesViewModelContainer (
            DocumentCollection vDocelems
        )
        {
            VDocelems = vDocelems ;
        }
    }

    public class DocumentCollection : IList, ICollection, IEnumerable 

    {
        private IList _list ;

        public DocumentCollection ( IList list ) { _list = list ; }

        public DocumentCollection ( ) {
        }

        #region Implementation of IEnumerable
        public IEnumerator GetEnumerator ( ) { return _list.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        public void CopyTo ( Array array , int index ) { _list.CopyTo ( array , index ) ; }

        public int Count
        {
            get { return _list.Count ; }
        }

        public object SyncRoot
        {
            get { return _list.SyncRoot ; }
        }

        public bool IsSynchronized
        {
            get { return _list.IsSynchronized ; }
        }
        #endregion
        #region Implementation of IList
        public int Add ( object value ) { return _list.Add ( value ) ; }

        public bool Contains ( object value ) { return _list.Contains ( value ) ; }

        public void Clear ( ) { _list.Clear ( ) ; }

        public int IndexOf ( object value ) { return _list.IndexOf ( value ) ; }

        public void Insert ( int index , object value ) { _list.Insert ( index , value ) ; }

        public void Remove ( object value ) { _list.Remove ( value ) ; }

        public void RemoveAt ( int index ) { _list.RemoveAt ( index ) ; }

        public object this [ int index ]
        {
            get { return _list[ index ] ; }
            set { _list[ index ] = value ; }
        }

        public bool IsReadOnly
        {
            get { return _list.IsReadOnly ; }
        }

        public bool IsFixedSize
        {
            get { return _list.IsFixedSize ; }
        }
        #endregion
    }
}