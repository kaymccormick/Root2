using System;
using System.Collections.Generic;
using System.ComponentModel;
using KayMcCormick.Dev.Interfaces;

namespace KayMcCormick.Lib.Wpf
{
    public interface IControlsProvider : IHaveObjectId
    {
        IEnumerable<Type> Types { get; }
        TypeDescriptionProvider Provider { get; }
    }
}