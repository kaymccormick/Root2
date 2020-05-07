using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;

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
    public class CustomControl2 : Control, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TypeInfoProviderProperty = DependencyProperty.Register(
            "TypeInfoProvider", typeof(ITypeInfoProvider), typeof(CustomControl2), new PropertyMetadata(default(ITypeInfoProvider)));

        public ITypeInfoProvider TypeInfoProvider
        {
            get { return (ITypeInfoProvider) GetValue(TypeInfoProviderProperty); }
            set { SetValue(TypeInfoProviderProperty, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get
            {
                var value = (Type)GetValue(TypeProperty);
                return value;
            }
            set
            {
                SetValue(TypeProperty, value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Attributes
        {
            get
            {
                if (Type == null)
                {
                    return Enumerable.Empty<string>();
                }
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

                if (TypeIsGenericType)
                {
                    var genericType = "Generic Type";
                    //yield return genericType;
                    a(genericType);
                }

                if (TypeIsGenericParameter)

                {
                    var genericParameter = "Generic Parameter";
                    a(genericParameter);
                    //yield return genericParameter;
                }

                if (TypeIsClass)

                {
                    var cl = "Class";
                    a(cl);
                    //yield return cl;
                }

                if (TypeIsPrimitive)
                {
                    a("Primitive");

                }

                if (TypeIsAbstract)
                {
                    a("Abstract");
                }

                if (TypeIsArray)
                {
                    a("Array");
                }

                if (TypeIsEnum)
                {
                    a("Enum");

                }

                if (TypeIsInterface)
                {
                    a("Interface");
                }

                if (TypeIsPublic)
                {
                    a("Public");
                }

                if (TypeIsSealed)
                {
                    a("Sealed");

                }

                if (TypeIsGenericTypeDefinition)
                {
                    a("Generic Type Definition");
                }

                if (TypeIsConstructedGenericType)
                {
                    a("Constructed Generic Type");
                }

                if (TypeIsValueType)
                {
                    a("Value Type");

                }
                return r;

            }
            
        }

        private bool TypeIsValueType
        {
            get { return Type.IsValueType; }
        }

        private bool TypeIsConstructedGenericType
        {
            get { return Type.IsConstructedGenericType; }
        }

        private bool TypeIsGenericTypeDefinition
        {
            get { return Type.IsGenericTypeDefinition; }
        }

        private bool TypeIsSealed
        {
            get { return Type.IsSealed; }
        }

        private bool TypeIsPublic
        {
            get { return Type.IsPublic; }
        }

        private bool TypeIsInterface
        {
            get { return Type.IsInterface; }
        }

        private bool TypeIsEnum
        {
            get { return Type.IsEnum; }
        }

        private bool TypeIsArray
        {
            get { return Type.IsArray; }
        }

        private bool TypeIsAbstract
        {
            get { return Type.IsAbstract; }
        }

        private bool TypeIsPrimitive
        {
            get { return Type.IsPrimitive; }
        }

        private bool TypeIsClass
        {
            get { return Type.IsClass; }
        }

        private bool TypeIsGenericParameter
        {
            get { return Type.IsGenericParameter; }
        }

        private bool TypeIsGenericType
        {
            get { return Type.IsGenericType; }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(Type), typeof(CustomControl2),
                new PropertyMetadata(null, new PropertyChangedCallback(_OnTypeChanged), CoerceType));

        private static object CoerceType(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static readonly RoutedEvent TypeChangedEvent =
    EventManager.RegisterRoutedEvent(
                                      "TypeChanged"
                                    , RoutingStrategy.Bubble
                                    , typeof(RoutedPropertyChangedEventHandler<
                                          Type>)
                                    , typeof(CustomControl2)
                                     );

        private static void _OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomControl2 cc2 = (CustomControl2)d;
            var newValue = (Type)e.NewValue;
            cc2.OnTypeChanged(newValue);

            // var evt = TypeChangedEvent;
            // var ev = new RoutedPropertyChangedEventArgs<Type>((Type) e.OldValue, (Type) e.NewValue, evt);
            // switch (d)
            // {
                // case UIElement uie:
                    // uie.RaiseEvent(ev);
                    // break;
                // case ContentElement ce:
                    // ce.RaiseEvent(ev);
                    // break;
                // default:
                    // break;

            
        }

        private void OnTypeChanged(Type newValue)
        {
            Descriptor = null;
            PropertyDescriptorCollection = null;
            Provider = null;
            var d = Descriptor;
            var c = PropertyDescriptorCollection;
            var p = Provider;

        }

        private Border _border1;
        private TypeDescriptionProvider _provider;
        private ICustomTypeDescriptor _descriptor;
        private PropertyDescriptorCollection _propertyDescriptorCollection;

        static CustomControl2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl2), new FrameworkPropertyMetadata(typeof(CustomControl2)));
        }

        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        public CustomControl2()
        {
            // AddHandler(TypeChangedEvent, new DependencyPropertyChangedEventHandler((sender, args) =>
            // {
                // Provider = null;
                // Descriptor = null;
                // PropertyDescriptorCollection = null;
            // }));
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            _border1 = (Border)GetTemplateChild("border1");
            base.OnApplyTemplate();
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyDescriptorCollection PropertyDescriptorCollection
        {
            get
            {
                if (_propertyDescriptorCollection == null && Descriptor != null)
                {
                    PropertyDescriptorCollection = Descriptor.GetProperties();
                }
                return _propertyDescriptorCollection;
            }
            set
            {
                if (Equals(value, _propertyDescriptorCollection)) return;
                _propertyDescriptorCollection = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summarym
        public TypeDescriptionProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    DebugUtils.WriteLine("Provider is null");
                    if (Type != null)
                    {
                        DebugUtils.WriteLine("Type is " + Type.FullName);
                        _provider = TypeDescriptor.GetProvider(Type);
                        DebugUtils.WriteLine("PRovider is " + _provider);
                    }
                }
                    DebugUtils.WriteLine("REturning provider " + _provider);
                return _provider;
            }
            set
            {
                if (Equals(value, _provider)) return;
                _provider = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Descriptor));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICustomTypeDescriptor Descriptor
        {
            get
            {
                if (_descriptor == null && Provider != null && Type != null)
                {
                    _descriptor = Provider.GetTypeDescriptor(Type);
                }

                return _descriptor;
            }
            set
            {
                if (Equals(value, _descriptor)) return;
                _descriptor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertyDescriptorCollection));
            }
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            //DebugUtils.WriteLine($"{constraint.Width}x{constraint.Height}");
             _border1.Measure(constraint);
             var dwidth = _border1.DesiredSize.Width;
             
             var dheight = _border1.DesiredSize.Height;
             var measureOverride = new Size(dwidth, dheight);
             //DebugUtils.WriteLine(measureOverride.ToString());
             return measureOverride;

        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }
    }

    public class TypeInfoProvider : ITypeInfoProvider
    {
        public bool IsNested { get; }
    }

    public interface ITypeInfoProvider
    {
        bool IsNested { get; }
        
    }

    public class TypeInfoProvider2 : ITypeInfoProvider
    {
        private ITypeSymbol typeSymbol;

        public TypeInfoProvider2(ITypeSymbol typeSymbol)
        {
            this.typeSymbol = typeSymbol;
            
        }

        public bool IsNested => typeSymbol.ContainingType != null;
    }
}
