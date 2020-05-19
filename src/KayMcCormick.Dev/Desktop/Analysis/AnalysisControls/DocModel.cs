using System;
using System.Collections.ObjectModel;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DocModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DocModel CreateInstance()
        {
            return new DocModel();
        }

        /// <summary>
        /// 
        /// </summary>
        private DocModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastActivationTimeStamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<object> ContextualTabGroupHeaders { get; set; } = new ObservableCollection<object>();
    }
}