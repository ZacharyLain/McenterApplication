using System.Windows;
using McenterTest.Models;


namespace McenterTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    AuthToken authToken = new AuthToken();
    
    // Constructor for the main window
    // Need to handle the Mcenter creation stuff in here as well
    public MainWindow()
    {
        InitializeComponent();

        
        
    }
    
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        
    }

    private void getBearerToken(object sender, RoutedEventArgs e)
    {
        Output1.Text = authToken.getBearerToken() ?? throw new InvalidOperationException("Error when getting token/setting text");
    }
}
