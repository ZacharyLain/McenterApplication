using System.Windows;
using System.Windows.Controls;
using McenterTest.Navigation;

namespace McenterTest.Pages;

public partial class MainPage : Page
{
    private MmrpPage mmrpPage = new();
    
    public MainPage()
    {
        InitializeComponent();
    }
    
    private void goToMmrt(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void goToMmrp(object sender, RoutedEventArgs e)
    {
        MainWindowNavigation.ChangePage(mmrpPage);
    }

    private void goToAmp(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}