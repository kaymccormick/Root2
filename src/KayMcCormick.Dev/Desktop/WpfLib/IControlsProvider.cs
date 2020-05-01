using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace KayMcCormick.Lib.Wpf
{
    public interface IControlsProvider
    {
        IEnumerable<Type> Types { get; }
        TypeDescriptionProvider Provider { get; }
    }
}