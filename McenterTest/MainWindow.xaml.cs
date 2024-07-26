using System.Windows;
using McenterTest.Navigation;
using McenterTest.Pages;

namespace McenterTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        MainWindowNavigation.ChangePage(new MainPage());
    }
}
