using System;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    public class TestProperty1 : CustomPropertyDescriptor
    {
        private readonly bool _isReadOnly1;
        private readonly string _displayName;
        private readonly Type _propertyType1;

        public TestProperty1() : base("KMTest", Array.Empty<Attribute>())
        {
            _isReadOnly1 = true;
            _displayName = "Test property";
            _propertyType1 = typeof(string);
        }

        public override Type PropertyType
        {
            get { return _propertyType1; }
        }


        public override object GetValue(object component)
        {
            return "Random test value " + component;
        }

        public override bool IsReadOnly
        {
            get { return _isReadOnly1; }
        }

        public override string DisplayName
        {
            get { return _displayName; }
        }
    }
}