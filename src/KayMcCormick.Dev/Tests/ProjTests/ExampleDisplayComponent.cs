using System;
using System.Drawing;
using System.IO;

namespace ProjTests
{
    public class ExampleDisplayComponent
    {
        public DateTime When { get; set; }
        public Icon Icon { get; set; }

        public string[] S { get; set; }
        public Color Color { get; set; }
        public DayOfWeek DayfOfWeek { get; set; }
        public FileAttributes FileAttributes { get; set; }
    }
}