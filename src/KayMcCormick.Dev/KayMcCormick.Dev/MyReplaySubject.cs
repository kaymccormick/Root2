using System;
using System.Reactive.Subjects;

namespace KmDevLib
{
    public class MyReplaySubject<T> : IMySubject
    {
        public bool ListView { get; set; } = true;
        private ReplaySubject<object> _s1 = new ReplaySubject<object>();
        private ReplaySubject<T> subject = new ReplaySubject<T>();

        public MyReplaySubject()
        {
            Type1 = typeof(T);
            ObservableExtensions.Subscribe<T>(Subject1, OnNext);
                }

        private void OnNext(T obj)
        {
            ObjectSubject.OnNext(obj);
        }

        public Type Type1 { get; set; }

        public ReplaySubject<T> Subject1
        {
            get { return subject; }
            set { subject = value; }
        }

        public ReplaySubject<object> ObjectSubject
        {
            get { return _s1; }
            set { _s1 = value; }
        }
    }

    public class MyReplaySubjectImpl2 : MyReplaySubject<IMySubject>
    {
    }
}