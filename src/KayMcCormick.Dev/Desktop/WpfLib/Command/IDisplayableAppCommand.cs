﻿#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Lib.Wpf
// IDisplayableAppCommand.cs
// 
// 2020-04-08-5:38 AM
// 
// ---
#endregion
namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    public interface IDisplayableAppCommand : IAppCommand , IDisplayable
    {
        /// <summary>
        /// 
        /// </summary>
        object LargeImageSourceKey { get ; set ; }
    }
}