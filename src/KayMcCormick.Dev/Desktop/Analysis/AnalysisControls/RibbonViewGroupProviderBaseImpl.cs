using System;
using System.Windows.Controls;
using Autofac;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelDatePicker
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SelectedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTodayHighlighted { get; set; }
    }
}