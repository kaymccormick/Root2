using System.ComponentModel;

namespace ProjTests
{
    [Editor(typeof(ExampleComponentEditor), typeof(ComponentEditor))]
    public class AppComponent : Component
    {
        [Description("Fun times")]
        [DisplayName("Test")]
        public string Test { get; set; }

        public ExampleUserControl X { get; set; }
    }
}