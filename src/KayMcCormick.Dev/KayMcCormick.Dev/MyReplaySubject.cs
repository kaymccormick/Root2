using System;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace KmDevLib
{
    public class MyReplaySubject<T> : IMySubject, INotifyPropertyChanged
    {
        public bool ListView { get; set; } = true;
        private ReplaySubject<object> _s1 = new ReplaySubject<object>();
        private ReplaySubject<T> subject = new ReplaySubject<T>();
        private string _title;

        public MyReplaySubject()
        {
            Type = typeof(T);
            Title = Type.FullName;
            ObservableExtensions.Subscribe<T>(Subject, OnNext);
        }

        private void OnNext(T obj)
        {
            ObjectSubject.OnNext(obj);
        }

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

        public Type Type { get; set; }

        public ReplaySubject<T> Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public ReplaySubject<object> ObjectSubject
        {
            get { return _s1; }
            set { _s1 = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object InstanceObjectId { get; set; }
    }
}