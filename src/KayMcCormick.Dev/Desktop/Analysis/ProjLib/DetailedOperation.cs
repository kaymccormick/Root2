using ProjLib.Interfaces ;

namespace ProjLib
{
    class DetailedOperation : IDetailedOperation
    {
        private string _displayName ;
        #region Implementation of IDetailedOperation
        public string DisplayName { get => _displayName ; set => _displayName = value ; }
        #endregion
    }
}