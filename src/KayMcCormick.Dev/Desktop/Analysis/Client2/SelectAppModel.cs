using System;
using System.Collections;

namespace Client2
{
    internal class SelectAppModel : ISelectAppModel
    {


        public void SetSelectedApp(object item)
        {
            SelectedItem = item;
            

        }

        public IEnumerable Items { get; set; }
        public object SelectedItem { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}