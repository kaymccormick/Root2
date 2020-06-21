using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using JetBrains.Annotations;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty("Content")]
    public class DocModel : INotifyPropertyChanged
    {
        private string _title;
        private bool _isVisible;
        public override string ToString() => $"<DocModel>: \"{Title}\"";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DocModel CreateInstance(string Title = null)
        {
            return new DocModel(){Title = Title};
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
        public string Title
        {
            get { return _title; }
            set
            {
                if (value == _title) return;
                _title = value;
                OnPropertyChanged();
            }
        }

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
        public bool IsVisible   
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual object Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable ContextualTabGroupHeaders { get; set; } = new ObservableCollection<object>();

        public virtual IEnumerable RibbonItems { get;  } = new ObservableCollection<object>();
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}