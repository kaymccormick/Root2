﻿using System;
using System.Composition;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [MetadataAttribute]
    public class ApplyTypesAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public ApplyTypesAttribute(Type type)
        {
            ApplyTypes = type;
        }

        /// <summary>
        /// 
        /// </summary>
        public Type ApplyTypes { get; set; }
    }
}