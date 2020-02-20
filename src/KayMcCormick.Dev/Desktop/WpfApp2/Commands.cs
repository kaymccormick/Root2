using System.Windows.Input ;

namespace WpfApp2
{
    public static class Commands
    {
        public static readonly RoutedUICommand ShowXaml =
            new RoutedUICommand ( "Show XAML" , nameof(ShowXaml) , typeof ( Commands ) ) ;
    }
}
