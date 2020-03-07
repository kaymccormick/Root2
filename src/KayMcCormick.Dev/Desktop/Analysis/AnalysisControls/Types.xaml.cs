using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Linq ;
using System.Reflection ;
using System.Text ;
using System.Text.RegularExpressions ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Documents ;
using System.Windows.Input ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using System.Windows.Navigation ;
using System.Windows.Shapes ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using ProjLib ;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for Types.xaml
    /// </summary>
    public partial class Types : UserControl , IView < ITypesViewModel > , IView1
    {
        private ITypesViewModel _viewModel ;

        public Types ( ITypesViewModel viewModel )
        {
            _viewModel = viewModel ;
            InitializeComponent ( ) ;
        }

        #region Implementation of IView<ITypesViewModel>
        public ITypesViewModel ViewModel { get => _viewModel ; set => _viewModel = value ; }
        #endregion

        #region Implementation of IView1
        public string ViewTitle => "Types View" ;
        #endregion
    }

    public interface ITypesViewModel : IViewModel
    {
        AppTypeInfo Root { get ; set ; }
    }

    internal class TypesViewModel : ITypesViewModel
    {
        private AppTypeInfo                       root ;
        private List < Type >                     _nodeTypes ;
        private Dictionary < Type , AppTypeInfo > map = new Dictionary < Type , AppTypeInfo > ( ) ;

        public TypesViewModel ( )
        {
            var rootR = typeof ( CSharpSyntaxNode ) ;
            _nodeTypes = rootR.Assembly.GetExportedTypes ( )
                              .Where ( t => typeof ( CSharpSyntaxNode ).IsAssignableFrom ( t ) )
                              .ToList ( ) ;
            Root = CollectTypeInfos ( rootR ) ;
            var methodInfos = typeof ( SyntaxFactory )
                             .GetMethods ( BindingFlags.Static | BindingFlags.Public )
                             .ToList ( ) ;

            foreach ( var methodInfo in methodInfos.Where (
                                                           info => typeof ( SyntaxNode )
                                                              .IsAssignableFrom ( info.ReturnType )
                                                          ) )
            {
                var info = map[ methodInfo.ReturnType ] ;
                info.FactoryMethods.Add ( methodInfo ) ;
                LogManager.GetCurrentClassLogger ( )
                          .Info ( "{methodName}" , methodInfo.ToString ( ) ) ;
            }

            foreach ( var pair in map.Where ( pair => pair.Key.IsAbstract == false ) )
            {
                foreach ( var propertyInfo in pair.Key.GetProperties (
                                                                      BindingFlags.DeclaredOnly
                                                                      | BindingFlags.Instance
                                                                      | BindingFlags.Public
                                                                     ) )
                {
                    var t = propertyInfo.PropertyType ;
                    if ( t == typeof ( SyntaxToken ) )
                    {
                        continue ;
                    }

                    var isList = false ;
                    AppTypeInfo typeInfo = null ;
                    if ( t.IsGenericType )
                    {
                        var targ = t.GenericTypeArguments[ 0 ] ;
                        if ( typeof ( SyntaxNode ).IsAssignableFrom ( targ )
                             && typeof ( IEnumerable ).IsAssignableFrom ( t ) )
                        {
                            LogManager.GetCurrentClassLogger ( )
                                      .Info (
                                             "{name} {prop} list of {}"
                                           , pair.Key.Name
                                           , propertyInfo.Name
                                           , targ.Name
                                            ) ;
                            isList   = true ;
                            typeInfo = map[ targ ] ;
                        }
                    }
                    else
                    {
                        map.TryGetValue ( t , out typeInfo ) ;
                    }

                    if ( typeInfo == null )
                    {
                        continue ;
                    }

                    pair.Value.Components.Add (
                                               new ComponentInfo ( )
                                               {
                                                   IsList       = isList
                                                 , TypeInfo     = typeInfo
                                                 , PropertyName = propertyInfo.Name
                                               }
                                              ) ;
                    LogManager.GetCurrentClassLogger ( ).Info ( t.ToString ( ) ) ;
                }
            }
        }

        public AppTypeInfo Root { get => root ; set => root = value ; }

        private AppTypeInfo CollectTypeInfos ( Type rootR )
        {
            var r = new AppTypeInfo ( ) { Type = rootR } ;
            foreach ( var type1 in _nodeTypes.Where ( type => type.BaseType == rootR ) )
            {
                r.SubTypeInfos.Add ( CollectTypeInfos ( type1 ) ) ;
            }

            map[ rootR ] = r ;
            return r ;
        }
    }

    public class ComponentInfo
    {
        private string      _propertyName ;
        private AppTypeInfo _typeInfo ;
        private bool        _isList ;
        public  string      PropertyName { get => _propertyName ; set => _propertyName = value ; }

        public AppTypeInfo TypeInfo { get => _typeInfo ; set => _typeInfo = value ; }

        public bool IsList { get => _isList ; set => _isList = value ; }
    }

    public class AppTypeInfo
    {
        private Type                   _type ;
        private string                 _title ;
        private List < MethodInfo >    _factoryMethods = new List < MethodInfo > ( ) ;
        private List < ComponentInfo > _components     = new List < ComponentInfo > ( ) ;

        public Type Type
        {
            get => _type ;
            set
            {
                _type = value ;
                var title = _type.Name.Replace ( "Syntax" , "" ) ;
                Title = Regex.Replace ( title , "([a-z])([A-Z])" , @"$1 $2" ) ;
            }
        }

        public string Title { get => _title ; set => _title = value ; }

        public ObservableCollection < AppTypeInfo > SubTypeInfos { get ; } =
            new ObservableCollection < AppTypeInfo > ( ) ;

        public List < MethodInfo > FactoryMethods
        {
            get => _factoryMethods ;
            set => _factoryMethods = value ;
        }

        public List < ComponentInfo > Components
        {
            get => _components ;
            set => _components = value ;
        }
    }
}