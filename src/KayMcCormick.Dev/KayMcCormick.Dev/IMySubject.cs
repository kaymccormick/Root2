using System;
using System.Reactive.Subjects;
using KayMcCormick.Dev.Interfaces;

namespace KmDevLib
{
    public interface IMySubject : IHaveObjectId
    {
        string Title { get; set; }
        Type Type { get; }
        ReplaySubject<object> ObjectSubject { get; }
        bool ListView { get; }
    }
}