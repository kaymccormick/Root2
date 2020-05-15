#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// ElementWrapper.cs
// 
// 2020-04-01-8:41 AM
// 
// ---
#endregion
namespace ConsoleAnalysis
{
    // ReSharper disable once UnusedType.Global
    public class ElementWrapper < T >
    {
        private T _element ;

        public ElementWrapper ( T element ) { Element = element ; }

        public T Element { get { return _element ; } set { _element = value ; } }
    }
}