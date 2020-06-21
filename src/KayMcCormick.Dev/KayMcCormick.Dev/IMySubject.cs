using System;
using System.Reactive.Subjects;

namespace KmDevLib
{
    public interface IMySubject
    {
        string Title { get; set; }
        Type Type1 { get; }
        ReplaySubject<object> ObjectSubject { get; }
        bool ListView { get; }
    }
}