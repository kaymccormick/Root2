using System;

namespace KayMcCormick.Dev
{
    [Flags]
    public enum DebugCategory
    {
        None = 0,
        Syntax = 1,
        Misc = 2,
        TextFormatting = 4,

        DataBinding = 8,
        Ribbon = 16,
        Status = 32
    }
}