namespace ProjLib
{
    public interface IRuntimestatus
    {
        AppStatus Status { get ; }
        IDetailedOperation CurrentOperation { get ; }
    }

    public interface IDetailedOperation
    {
        string DisplayName { get ; }
    }

    class DetailedOperation : IDetailedOperation
    {
        private string _displayName ;
        #region Implementation of IDetailedOperation
        public string DisplayName { get => _displayName ; set => _displayName = value ; }
        #endregion
    }
}