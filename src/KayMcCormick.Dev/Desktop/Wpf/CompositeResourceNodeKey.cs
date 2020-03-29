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
    /// <summary>
    /// 
    /// </summary>
    public class CompositeResourceNodeKey
    {
        private readonly object _key ;
        private readonly object _conjugate ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="conjugate"></param>
        public CompositeResourceNodeKey ( object key , object conjugate )
        {
            _key       = key ;
            _conjugate = conjugate ;
        }

        #region Overrides of Object
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString ( ) { return $"${_key}:{_conjugate}" ; }
        #endregion
    }
}