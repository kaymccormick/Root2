using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KayMcCormick.Lib.Wpf
{
    public class CustomBinding : Binding
    {
        public CustomBinding()
        {
            NotifyOnTargetUpdated = true;
            NotifyOnSourceUpdated = true;
        }

        public CustomBinding(string path) : base(path)

        {
            NotifyOnTargetUpdated = true;
            NotifyOnSourceUpdated = true;

        }
    }
}
