using System;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DocModel
    {
        public DocModel()
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
    }
}