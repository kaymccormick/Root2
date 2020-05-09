using System;
using System.Collections.Generic;
using Autofac.Core;

namespace Client2
{
    internal class MyInfo
    {
        public Guid Id { get; internal set; }
        public List<IComponentRegistration> Registrations { get; internal set; } = new List<IComponentRegistration>();
    }
}