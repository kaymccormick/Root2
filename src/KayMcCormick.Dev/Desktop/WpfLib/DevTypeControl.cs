using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using JetBrains.Annotations;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KayMcCormick.Lib.Wpf"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:KayMcCormick.Lib.Wpf;assembly=KayMcCormick.Lib.Wpf"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DevTypeControl/>
    ///
    /// </summary>
    public class DevTypeControl : Control
    {
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            "Type", typeof(Type), typeof(DevTypeControl), new PropertyMetadata(default(Type)));

        public Type Type
        {
            get { return (Type) GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }   
        static DevTypeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DevTypeControl), new FrameworkPropertyMetadata(typeof(DevTypeControl)));
        }

        private void GenerateControlsForType([NotNull] Type myType
            , List<object> m
            , bool toolTip)
        {
            if ( myType == null )
            {
                throw new ArgumentNullException ( nameof ( myType ) ) ;
            }

            //List<object> m = new List<object>();
            var myTypeName = myType.Name;
            if (myType == typeof(Boolean))
            {
                myTypeName = "bool";
            } else if (myType == typeof(Byte))
            {
                myTypeName = "byte";
            } else if (myType == typeof(SByte))
            {
                myTypeName = "sbyte";
            }
            else if (myType == typeof(Char))
            {
                myTypeName = "char";
            } else if (myType == typeof(Decimal))
            {
                myTypeName = "decimal";
            } else if (myType == typeof(Double))
            {
                myTypeName = "double";
            } else if (myType == typeof(Single))
            {
                myTypeName = "float";
            } else if (myType == typeof(Int32))
            {
                myTypeName = "int";
            } else if(myType == typeof(UInt32))
            {
                myTypeName = "uint";

            } else if (myType == typeof(Int64))
            {
                myTypeName = "long";
            } else if (myType == typeof(UInt64))
            {

                myTypeName = "ulong";

            } else if(myType == typeof(Int16))
            {
                myTypeName = "short";
            }
            else if (myType == typeof(UInt16))
            {
                myTypeName = "ushort";
            }
            else if (myType == typeof(Object))
            {
                myTypeName = "object";
            } else if (myType == typeof(String))
            {
                myTypeName = "string";
            } else if (myType == typeof(void))
            {
                myTypeName = "void";
            }

            
            if (MakeHyperlink)
            {
                
                var hyperLink = new Hyperlink(new Run(myTypeName));
                if (typeof(IEnumerator).IsAssignableFrom(myType))
                {
                    hyperLink.Foreground = new SolidColorBrush {Color = Colors.DarkOrange};
                }

                Uri.TryCreate(
                    "obj:///"
                    + Uri.EscapeUriString(
                        myType.AssemblyQualifiedName
                        ?? throw new InvalidOperationException()
                    )
                    , UriKind.Absolute
                    , out var res
                );

                hyperLink.NavigateUri = res;
                // hyperLink.Command          = MyAppCommands.VisitTypeCommand ;
                // hyperLink.CommandParameter = myType ;
                if (toolTip)
                {
                    hyperLink.ToolTip = new ToolTip {Content = ToolTipContent(myType)};
                }

                //hyperLink.RequestNavigate += HyperLinkOnRequestNavigate;
                m.Add(hyperLink);
            
            }
            else
            {
                TextBlock tb = new TextBlock() {Text = myTypeName};
                m.Add(tb);
            }


            if ( ! myType.IsGenericType )
            {
                return ;
            }

            m.Add ( "<" ) ;
            const int i = 0 ;
            foreach ( var arg in myType.GenericTypeArguments )
            {
                GenerateControlsForType ( arg , m , true ) ;
                if ( i < myType.GenericTypeArguments.Length )
                {
                    m.Add( ", " ) ;
                }
            }

            m.Add( ">" ) ;

            //old.AddChild ( tb ) ;
        }

        public bool MakeHyperlink { get; set; }

        private object ToolTipContent(Type myType)
        {
            throw new NotImplementedException();
        }

        private void PopulateControl ( [ CanBeNull ] Type myType )
        {            IAddChild addChild ;
            //DebugUtils.WriteLine ( myType?.FullName ?? "null" ) ;
            if ( Detailed )
            {
                var paragraph = new Paragraph ( ) ;
                //FlowDocument = new FlowDocument ( paragraph ) ;
                //var reader = new FlowDocumentReader { Document = FlowDocument } ;
                addChild = paragraph ;
                //SetCurrentValue ( ContentProperty , reader ) ;
            }
            else
            {
                addChild = new TextBlock ( ) ;
               // SetCurrentValue ( ContentProperty , addChild ) ;

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
                    var m = new List<object>();
                    var paragraph = new Paragraph ( ) ;
                    var listItem = new ListItem ( paragraph ) ;

                    GenerateControlsForType ( baseType , m, false ) ;
                    elem.ListItems.Add ( listItem ) ;
                    //Container.Children.Insert ( 0 , new TextBlock ( new Hyperlink()) ( baseType.Name ) ) ) ;
                    baseType = baseType.BaseType ;
                }

                //FlowDocument.Blocks.InsertBefore ( FlowDocument.Blocks.FirstBlock , elem ) ;
            }

            var p = new Span ( ) ;
            var m0 = new List<object>();
            GenerateControlsForType ( myType , m0 , true ) ;
            addChild.AddChild ( p ) ;
            // Viewer.Document.Blocks.Add ( block ) ;
            // Container.Children.Add ( ) ;
        }

        public bool Detailed { get; set; }
    }
}
