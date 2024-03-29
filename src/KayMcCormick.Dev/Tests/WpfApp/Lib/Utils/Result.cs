﻿using System.Windows.Automation ;

namespace Tests.Lib.Utils
{
#pragma warning disable 1591

    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for Result
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Result
    {
        /// <summary>The success</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Success
        public bool Success { get ; }

        /// <summary>Initializes a new instance of the <see cref="Result" /> class.</summary>
        /// <param name="autoElem">The automatic elem.</param>
        /// <param name="success"></param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for #ctor
        // ReSharper disable once UnusedMember.Global
        public Result ( AutomationElement autoElem , bool success )
        {
            AutoElem = autoElem ;
            Success = success ;
        }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public AutomationElement AutoElem { get ; set ; }
    }
}