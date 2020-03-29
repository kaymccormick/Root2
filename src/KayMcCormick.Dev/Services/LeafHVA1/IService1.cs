namespace LeafHVA1
{
    public interface IService1
    {
        bool Start ( ) ;
        bool Stop ( ) ;
        bool Pause ( ) ;
        bool Continue ( ) ;
        bool Shutdown ( ) ;

        void PerformFunc1 ( ) ;
    }
}