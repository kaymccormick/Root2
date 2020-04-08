#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// MenuWrapper.cs
// 
// 2020-04-08-5:30 AM
// 
// ---
#endregion
using System ;

namespace ConsoleApp1
{
    internal class MenuWrapper < T >
    {
        private readonly T                   _instance ;
        private readonly Func < T , string > _renderFunc ;

        public MenuWrapper ( T instance , Func < T , string > renderFunc )
        {
            _instance   = instance ;
            _renderFunc = renderFunc ;
        }

        public T Instance { get { return _instance ; } }

        #region Overrides of Object
        public override string ToString ( ) { return _renderFunc ( Instance ) ; }
        #endregion
    }
}