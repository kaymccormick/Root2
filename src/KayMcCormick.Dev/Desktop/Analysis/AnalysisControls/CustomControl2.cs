using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
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
    ///     <MyNamespace:CustomControl2/>
    ///
    /// </summary>
    public class CustomControl2 : Control
    {


        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get { return (Type)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public IEnumerable<string> Attributes
        {
            get
            {
                List<string> r = new List<string>();
                void a(string s)
                {
                    r.Add(s);
                }
                if (Type.IsNested)
                {
                    a("Nested");
                    //yield return "Nested";
                }

                if (Type.IsGenericType)
                {
                    var genericType = "Generic Type";
                    //yield return genericType;
                    a(genericType);
                }

                if (Type.IsGenericParameter)

                {
                    var genericParameter = "Generic Parameter";
                    a(genericParameter);
                    //yield return genericParameter;
                }

                if (Type.IsClass)

                {
                    var cl = "Class";
                    a(cl);
                    //yield return cl;
                }

                if (Type.IsPrimitive)
                {
                    a("Primitive");

                }

                if (Type.IsAbstract)
                {
                    a("Abstract");
                }

                if (Type.IsArray)
                {
                    a("Array");
                }

                if (Type.IsEnum)
                {
                    a("Enum");

                }

                if (Type.IsInterface)
                {
                    a("Interface");
                }

                if (Type.IsPublic)
                {
                    a("Public");
                }

                if (Type.IsSealed)
                {
                    a("Sealed");

                }

                if (Type.IsGenericTypeDefinition)
                {
                    a("Generic Type Definition");
                }

                if (Type.IsConstructedGenericType)
                {
                    a("Constructed Generic Type");
                }

                if (Type.IsValueType)
                {
                    a("Value Type");

                }
                return r;

            }
            
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(Type), typeof(CustomControl2), new PropertyMetadata(null));

        private Border _border1;


        static CustomControl2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl2), new FrameworkPropertyMetadata(typeof(CustomControl2)));
        }

        public IEnumerable<Type> Ancestors
        {
            get
            {
                List<Type> a = new List<Type>();
                var b = Type;//Type.IsGenericType? Type.GetGenericTypeDefinition() : Type;
                while (b != null)
                {
                    a.Add(b);
                    b = b.BaseType;
                }

                return ((IEnumerable<Type>)a).Reverse();
            }
        }
        public CustomControl2()
        {
            
        }

        public override void OnApplyTemplate()
        {
            _border1 = (Border)GetTemplateChild("border1");
            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            DebugUtils.WriteLine($"{constraint.Width}x{constraint.Height}");
             _border1.Measure(constraint);
             var dwidth = _border1.DesiredSize.Width
                 ;
             var dheight = _border1.DesiredSize.Height;
             var measureOverride = new Size(dwidth, dheight);
             DebugUtils.WriteLine(measureOverride.ToString());
             return measureOverride;

        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }
    }
}
