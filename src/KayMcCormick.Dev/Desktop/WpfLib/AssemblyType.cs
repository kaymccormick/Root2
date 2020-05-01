using System;
using System.Reflection;

namespace KayMcCormick.Lib.Wpf
{
    public class AssemblyType
    {
        public Type Type { get; set; }
        public Assembly Assembly { get; set; }
        public string[] Elements { get; set; }
        public override string ToString()
        {
            return $"{Type.FullName} ({Assembly.GetName().Name})";
        }
    }
}