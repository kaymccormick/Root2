using System;

namespace ProjTests
{
    public class ExampleComponentEditor : System.Windows.Forms.Design.WindowsFormsComponentEditor
    {
        // This method override returns an type array containing the type of 
        // each component editor page to display.
        protected override Type[] GetComponentEditorPages()
        {
            return new Type[]
            {
                typeof(ExampleComponentEditorPage),
                typeof(ExampleComponentEditorPage)
            };
        }

        // This method override returns the index of the page to display when the 
        // component editor is first displayed.
        protected override int GetInitialComponentEditorPageIndex()
        {
            return 1;
        }
    }
}