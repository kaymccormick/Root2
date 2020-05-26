using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty("Content")]
    public class DocModel
    {
        public override string ToString() => Title;
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
        protected DocModel()
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
        public virtual object Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable ContextualTabGroupHeaders { get; set; } = new ObservableCollection<object>();
    }
}