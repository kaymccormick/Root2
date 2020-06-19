using System;
using System.Reactive.Subjects;

namespace KmDevLib
{
    public interface IMySubject
    {
        Type Type1 { get; }
        ReplaySubject<object> ObjectSubject { get; }
        bool ListView { get; }
    }
}