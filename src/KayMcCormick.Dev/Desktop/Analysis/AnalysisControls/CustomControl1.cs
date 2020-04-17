using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Windows ;
using System.Windows.Controls ;
using AnalysisAppLib.Syntax ;

namespace AnalysisControls
{
    /// <summary>
    ///     Follow steps 1a or 1b and then 2 to use this custom control in a XAML
    ///     file.
    ///     Step 1a) Using this custom control in a XAML file that exists in the
    ///     current project.
    ///     Add this XmlNamespace attribute to the root element of the markup file
    ///     where it is
    ///     to be used:
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///     Step 1b) Using this custom control in a XAML file that exists in a
    ///     different project.
    ///     Add this XmlNamespace attribute to the root element of the markup file
    ///     where it is
    ///     to be used:
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
    ///     You will also need to add a project reference from the project where the
    ///     XAML file lives
    ///     to this project and Rebuild to avoid compilation errors:
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///     Step 2)
    ///     Go ahead and use your control in the XAML file.
    ///     <MyNamespace:CustomControl1 />
    /// </summary>
    public sealed class CustomControl1 : Control
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RootItemsSourceProperty =
            DependencyProperty.Register (
                                         "RootItemsSource"
                                       , typeof ( IEnumerable < AppTypeInfo > )
                                       , typeof ( CustomControl1 )
                                       , new FrameworkPropertyMetadata ( ( object ) null )
                                        ) ;
        private static readonly DependencyPropertyKey RootItemsPropertyKey =
            DependencyProperty.RegisterReadOnly (
                                                 "RootItems"
                                               , typeof ( ObservableCollection < AppTypeInfo > )
                                               , typeof ( CustomControl1 )
                                               , new FrameworkPropertyMetadata (
                                                                                new
                                                                                    ObservableCollection
                                                                                    < AppTypeInfo
                                                                                    > ( )
                                                                               )
                                                ) ;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RootItemsProperty =
            RootItemsPropertyKey.DependencyProperty ;

        private static readonly DependencyPropertyKey FactoryMethodsPropertyKey =
            DependencyProperty.RegisterReadOnly(
                                                "FactoryMethods"
                                              , typeof(ObservableCollection<AppMethodInfo>)
                                              , typeof(CustomControl1)
                                              , new FrameworkPropertyMetadata(
                                                                              new
                                                                                  ObservableCollection
                                                                                  <AppMethodInfo
                                                                                  >()
                                                                             )
                                               );

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty FactoryMethodsProperty =
            FactoryMethodsPropertyKey.DependencyProperty;
        static CustomControl1 ( )
        {
            DefaultStyleKeyProperty.OverrideMetadata (
                                                      typeof ( CustomControl1 )
                                                    , new FrameworkPropertyMetadata (
                                                                                     typeof (
                                                                                         CustomControl1
                                                                                     )
                                                                                    )
                                                     ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public CustomControl1 ( ) {
            SetValue (RootItemsPropertyKey, new ObservableCollection<AppTypeInfo>()  );
            SetValue (FactoryMethodsPropertyKey, new ObservableCollection<AppMethodInfo>()  );
        }

        #region Overrides of FrameworkElement
        /// <inheritdoc />
        public override void OnApplyTemplate ( )
        {
            var treeView = GetTemplateChild ( "treeView" ) as TreeView ;
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection <AppMethodInfo> FactoryMethods { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < AppTypeInfo > RootItems
        {
            get { return ( ObservableCollection < AppTypeInfo > ) GetValue ( RootItemsProperty ) ; }
            set { SetValue ( RootItemsProperty , value ) ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AppTypeInfo> RootItemsSource
        {
            get { return ( IEnumerable < AppTypeInfo > ) GetValue ( RootItemsSourceProperty ) ; }
            set { SetValue ( RootItemsSourceProperty , value ) ;  }
        }
        #endregion
    }
}