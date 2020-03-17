using System ;
using System.CodeDom ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Documents ;
using System.Windows.Markup ;
using System.Windows.Navigation ;
using Microsoft.CSharp ;
using NLog ;

namespace WpfApp.Controls
{
    /// <summary>Control for displaying runtime type information.</summary>
    public partial class TypeControl : UserControl
    {
        private const string NavCancelledMessage = @"nav cancelled";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>The rendered type property</summary>
        public static readonly DependencyProperty
            RenderedTypeProperty = Props.RenderedTypeProperty ;

        /// <summary>The target name property</summary>
        public static readonly DependencyProperty TargetNameProperty =
            DependencyProperty.Register (
                                         "TargetName"
                                       , typeof ( string )
                                       , typeof ( TypeControl )
                                       , new PropertyMetadata ( default ( string ) )
                                        ) ;

        /// <summary>The target property</summary>
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register (
                                         "Target"
                                       , typeof ( Frame )
                                       , typeof ( TypeControl )
                                       , new PropertyMetadata ( default ( Frame ) )
                                        ) ;

        /// <summary>The detailed property</summary>
        public static readonly DependencyProperty DetailedProperty =
            DependencyProperty.Register (
                                         "Detailed"
                                       , typeof ( bool )
                                       , typeof ( TypeControl )
                                       , new PropertyMetadata ( default ( bool ) )
                                        ) ;

        /// <summary>The target detailed property</summary>
        public static readonly DependencyProperty TargetDetailedProperty =
            DependencyProperty.Register (
                                         "TargetDetailed"
                                       , typeof ( bool )
                                       , typeof ( TypeControl )
                                       , new PropertyMetadata ( default ( bool ) )
                                        ) ;

        /// <summary>Parameterless constructor.</summary>
        public TypeControl ( )
        {
            RenderedTypeChanged += OnRenderedTypeChanged ;
            InitializeComponent ( ) ;
            PopulateControl ( GetValue ( RenderedTypeProperty ) as Type ) ;
        }

        /// <summary>Gets or sets the type to be rendered.</summary>
        /// <value>The type to render in the control.</value>
        public Type RenderedType
        {
            // ReSharper disable once UnusedMember.Global
            get => GetValue ( RenderedTypeProperty ) as Type ;
            set => SetValue ( RenderedTypeProperty , value ) ;
        }

        /// <summary>Gets or sets the name of the target frame (unused).</summary>
        /// <value>The name of the target.</value>
        // ReSharper disable once UnusedMember.Global
        public string TargetName
        {
            get => ( string ) GetValue ( TargetNameProperty ) ;
            set => SetValue ( TargetNameProperty , value ) ;
        }

        /// <summary>Gets or sets the target for any navigation.</summary>
        /// <value>The target.</value>
        public Frame Target
        {
            get => ( Frame ) GetValue ( TargetProperty ) ;
            set => SetValue ( TargetProperty , value ) ;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this
        ///     <see cref="TypeControl" /> is in its detailed form.
        /// </summary>
        /// <value>
        ///     <see language="true"/> if detailed; otherwise, <see language="false"/>.
        /// </value>
        public bool Detailed
        {
            get => ( bool ) GetValue ( DetailedProperty ) ;
            set => SetValue ( DetailedProperty , value ) ;
        }

        /// <summary>Gets or sets a value indicating whether followed links will be detailed.</summary>
        /// <value>
        ///     <see language="true"/> if [target detailed]; otherwise, <see language="false"/>.
        /// </value>
        public bool TargetDetailed
        {
            get => ( bool ) GetValue ( TargetDetailedProperty ) ;
            set => SetValue ( TargetDetailedProperty , value ) ;
        }

        /// <summary>Gets or sets the flow document.</summary>
        /// <value>The flow document.</value>
        public FlowDocument FlowDocument { get ; set ; }


        /// <summary>Occurs when rendered type is changed.</summary>
        public event RoutedPropertyChangedEventHandler < Type > RenderedTypeChanged
        {
            add => AddHandler ( Props.RenderedTypeChangedEvent , value ) ;
            remove => RemoveHandler ( Props.RenderedTypeChangedEvent , value ) ;
        }

        private void OnRenderedTypeChanged (
            object                                  sender
          , RoutedPropertyChangedEventArgs < Type > e
        )
        {
            PopulateControl ( e.NewValue ) ;
        }

        private void PopulateControl ( Type myType )
        {
            IAddChild addChild ;
            if ( Detailed )
            {
                var paragraph = new Paragraph ( ) ;
                FlowDocument = new FlowDocument ( paragraph ) ;
                var reader = new FlowDocumentReader { Document = FlowDocument } ;
                addChild = paragraph ;
                Content  = reader ;
            }
            else
            {
                addChild = new TextBlock ( ) ;
                Content  = addChild ;

                // Container.Children.Clear();
                // Container.Children.Add ( block ) ;
            }

            if ( myType == null )
            {
                return ;
            }


            if ( Detailed )
            {
                var elem = new List { MarkerStyle = TextMarkerStyle.None } ;

                var baseType = myType.BaseType ;
                while ( baseType != null )
                {
                    var paragraph = new Paragraph ( ) ;
                    var listItem = new ListItem ( paragraph ) ;

                    GenerateControlsForType ( baseType , paragraph , false ) ;
                    elem.ListItems.Add ( listItem ) ;
                    //Container.Children.Insert ( 0 , new TextBlock ( new Hyperlink()) ( baseType.Name ) ) ) ;
                    baseType = baseType.BaseType ;
                }

                FlowDocument.Blocks.InsertBefore ( FlowDocument.Blocks.FirstBlock , elem ) ;
            }

            var p = new Span ( ) ;
            GenerateControlsForType ( myType , p , true ) ;
            addChild.AddChild ( p ) ;
            // Viewer.Document.Blocks.Add ( block ) ;
            // Container.Children.Add ( ) ;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        private void GenerateControlsForType ( Type myType , IAddChild addChild , bool toolTip )
        {
            // TextBlock tb = new TextBlock();
            // var old = addChild ;
            // addChild = tb ;


            var hyperLink = new Hyperlink ( new Run ( myType.Name ) ) ;
            Uri.TryCreate (
                           "obj://" + Uri.EscapeUriString ( myType.Name )
                         , UriKind.Absolute
                         , out var res
                          ) ;

            hyperLink.NavigateUri = res ;
            // hyperLink.Command          = MyAppCommands.VisitTypeCommand ;
            // hyperLink.CommandParameter = myType ;
            if ( toolTip )
            {
                hyperLink.ToolTip = new ToolTip { Content = ToolTipContent ( myType ) } ;
            }

            hyperLink.RequestNavigate += HyperLinkOnRequestNavigate ;
            addChild.AddChild ( hyperLink ) ;
            if ( myType.IsGenericType )
            {
                addChild.AddText ( "<" ) ;
                const int i = 0 ;
                foreach ( var arg in myType.GenericTypeArguments )
                {
                    GenerateControlsForType ( arg , addChild , true ) ;
                    if ( i < myType.GenericTypeArguments.Length )
                    {
                        addChild.AddText ( ", " ) ;
                    }
                }

                addChild.AddText ( ">" ) ;
            }

            //old.AddChild ( tb ) ;
        }

        private static object ToolTipContent ( Type myType , StackPanel pp = null )
        {
            var provider = new CSharpCodeProvider ( ) ;
            var codeTypeReference = new CodeTypeReference ( myType ) ;
            var q = codeTypeReference ;
            var toolTipContent = new TextBlock
                                 {
                                     Text = provider.GetTypeOutput ( q ) , FontSize = 20
                                     //, Margin = new Thickness ( 15 )
                                 } ;
            if ( pp == null )
            {
                pp = new StackPanel { Orientation = Orientation.Vertical } ;
            }

            pp.Children.Insert ( 0 , toolTipContent ) ;
            var @base = myType.BaseType ;
            if ( @base != null )
            {
                ToolTipContent ( @base , pp ) ;
            }

            return pp ;
        }

        // ReSharper disable once UnusedMember.Local
        private string NameForType ( Type myType )
        {
            // todo move to a better place
            using var provider = new CSharpCodeProvider ( ) ;
            if ( myType.IsGenericType )
            {
                var type = myType.GetGenericTypeDefinition ( ) ;
                myType = type ;
            }

            var codeTypeReference = new CodeTypeReference ( myType ) ;
            var q = codeTypeReference ;
            //myType.GetGenericTypeParameters()
            return provider.GetTypeOutput ( q ) ;
            // return myType.IsGenericType ? myType.GetGenericTypeDefinition ( ).Name : myType.Name ;
        }

        private void HyperLinkOnRequestNavigate ( object sender , RequestNavigateEventArgs e )
        {
            //
            // if(findName == null)
            // {
            // 	Logger.Debug ( "Cant find " + findName) ;
            // }
            Logger.Debug ( $"{nameof ( HyperLinkOnRequestNavigate )}: Uri={e.Uri}" ) ;
            var uie = ( ContentElement ) sender ;
            try
            {
                var navigationService = NavigationService ( ) ;

                if ( navigationService != null )
                {
                    var targetDetailed = Detailed || TargetDetailed ;
                    var value = uie.GetValue ( Props.RenderedTypeProperty ) as Type ;
                    var typeControl2 = new TypeControl2 ( ) ;
                    typeControl2.SetValue ( Props.RenderedTypeProperty , value ) ;
                    var navigationState = new NavState
                                          {
                                              Detailed = targetDetailed , RenderedType = value
                                          } ;
                    if ( ! navigationService.Navigate ( typeControl2 , navigationState ) )
                    {
                        Logger.Info(NavCancelledMessage) ;
                    }

                    e.Handled = true ;
                }
                else
                {
                    Logger.Info ( "find other way to navigate type" ) ;
                }
            }
            catch ( Exception ex )
            {
                Logger.Warn ( ex , ex.Message ) ;
            }
        }

        private NavigationService NavigationService ( )
        {
            var navigationService = Target != null
                                        ? System.Windows.Navigation.NavigationService
                                                .GetNavigationService (
                                                                       Target.Content as
                                                                           DependencyObject
                                                                       ?? throw new
                                                                           InvalidOperationException ( )
                                                                      )
                                        : System.Windows.Navigation.NavigationService
                                                .GetNavigationService ( this ) ;
            return navigationService ;
        }
    }

    internal class NavState
    {
        public bool Detailed { get ; set ; }

        public Type RenderedType { get ; set ; }
    }
}
