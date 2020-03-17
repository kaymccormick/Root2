using ProjLib.Interfaces ;

namespace ProjLib
{
    public class DesignRuntimeStatus : IRuntimestatus
    {
        private static DesignRuntimeStatus _instance ;

        protected DesignRuntimeStatus ( )
        {
            CurrentOperation = new DetailedOperation { DisplayName = "Waiting for user input" } ;
        }

        public static DesignRuntimeStatus Instance
        {
            get { return _instance ?? ( _instance = new DesignRuntimeStatus ( ) ) ; }
        }

        private AppStatus _status ;
        private IDetailedOperation _currentOperation ;
        #region Implementation of IRuntimestatus
        public AppStatus Status { get => _status ; set => _status = value ; }

        public IDetailedOperation CurrentOperation { get => _currentOperation ; set => _currentOperation = value ; }
        #endregion
    }
}