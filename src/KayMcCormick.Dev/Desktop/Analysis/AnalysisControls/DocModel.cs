using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;
using JetBrains.Annotations;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty("Content")]
    public class DocModel : DependencyObject, INotifyPropertyChanged
    {
        private bool _isVisible;
        private bool _isActive;
        private object _content;
        public override string ToString() => $"<DocModel>: \"{Title}\"";
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DocModel CreateInstance(string title = null)
        {
            return new DocModel(){Title = title};
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

        public ViewSpec CurrentView
        {
            get
            {
                return Content is IControlWithViews vs ? vs.CurrentView : null;
            }
            set
            {
                if (Content is IControlWithViews vs)
                {
                    vs.CurrentView = value;
                }
            }
        }

        public string GroupHeader { get; set; }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(DocModel), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
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
        public virtual object Content
        {
            get { return _content; }
            set
            {
                if (Equals(value, _content)) return;
                _content = value;
                if (_content is IControlWithViews cviews)
                {

                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable ContextualTabGroupHeaders { get; set; } = new ObservableCollection<object>();

        public virtual IEnumerable RibbonItems { get;  } = new ObservableCollection<object>();

        public virtual bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (value == _isActive) return;
                _isActive = value;
                OnPropertyChanged();
            }
        }

        public object LargeImageSource { get; set; }
        public ObservableCollection<ViewSpec> Views
        {
            get { return Content is IControlWithViews vs ? vs.Views : null; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}