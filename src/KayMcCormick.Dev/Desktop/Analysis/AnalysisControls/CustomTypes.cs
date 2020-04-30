using System;
using System.Collections.Generic;

namespace AnalysisControls
{
    public class CustomTypes
    {
        private readonly List<Type> _kayTypes;

        public CustomTypes(List<Type> kayTypes)
        {
            _kayTypes = kayTypes;
        }

        public IEnumerable<Type> CustomTypeList => _kayTypes;
    }
}