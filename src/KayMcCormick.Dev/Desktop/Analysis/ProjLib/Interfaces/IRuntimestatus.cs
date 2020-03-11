namespace ProjLib.Interfaces
{
    public interface IRuntimestatus
    {
        AppStatus          Status           { get ; }
        IDetailedOperation CurrentOperation { get ; }
    }
}