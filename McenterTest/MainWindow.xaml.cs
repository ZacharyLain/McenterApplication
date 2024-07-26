using System.Net.Http;
using System.Text.Json;
using System.Windows;
using McenterTest.Models;
using McenterTest.Services;
using Newtonsoft.Json;

namespace McenterTest;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    AuthToken authToken = new AuthToken();
    private JsonSerializerOptions serializerOptions;
    
    // Constructor for the main window
    // Need to handle the Mcenter creation stuff in here as well
    public MainWindow()
    {
        InitializeComponent();

        serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
    
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        
    }

    private void getBearerToken(object sender, RoutedEventArgs e)
    {
        Output1.Text = authToken.getBearerToken() ?? throw new InvalidOperationException("Error when getting token/setting text");
    }

    private void getNCPackages(object sender, RoutedEventArgs e)
    {
        string responseText = HttpRequests.httpRequest("/mmrp/api/programs/v5/ncpackages", HttpMethod.Get, null).Content.ReadAsStringAsync().Result;
        var jsonObject = JsonConvert.DeserializeObject(responseText);
        string formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

        Output2.Text = formattedJson;
        Console.Out.WriteLine("\n\n" + formattedJson + "\n\n");
    }
}
