#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// CompositeResourceNodeKey.cs
// 
// 2020-03-21-10:34 PM
// 
// ---
#endregion
namespace KayMcCormick.Lib.Wpf
{
    public class CompositeResourceNodeKey
    {
        private readonly object _key ;
        private readonly object _conjugate ;

        public CompositeResourceNodeKey ( object key , object conjugate )
        {
            _key       = key ;
            _conjugate = conjugate ;
        }

        #region Overrides of Object
        public override string ToString ( ) { return $"${_key}:{_conjugate}" ; }
        #endregion
    }
}