using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using McenterTest.Models;
using McenterTest.Services;
using Newtonsoft.Json;

namespace McenterTest;

public partial class MmrpPage
{
    ApiAuth apiAuth = new ApiAuth();
    private JsonSerializerOptions serializerOptions;
    
    // Constructor for the main window
    // Need to handle the Mcenter creation stuff in here as well
    public MmrpPage()
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
        Output1.Text = apiAuth.getBearerToken() ?? throw new InvalidOperationException("Error when getting token/setting text");
    }

    private void getNCPackages(object sender, RoutedEventArgs e)
    {
        string responseText = HttpRequests.httpRequest("/mmrp/api/programs/v5/ncpackages", HttpMethod.Get, null).Content.ReadAsStringAsync().Result;
        var jsonObject = JsonConvert.DeserializeObject(responseText);
        string formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

        Output2.Text = formattedJson;
        Console.Out.WriteLine("\n\n" + formattedJson + "\n\n");
    }

    private void createNCPackages(object sender, RoutedEventArgs e)
    {
        NcPackage ncPackage = new()
        {
            Name = createPack_Name.Text,
            WorkpieceId = createPack_Id.Text,
            Description = createPack_Desc.Text,
            CreatedBy = createPack_Author.Text,
            TrialRun = createPack_Trial.Text,
            ManualVerificationAfterTrialRun = createPack_ManVerification.Text
        };

        var body = JsonContent.Create(ncPackage);
        
        string responseText = HttpRequests.httpRequest("/mmrp/api/programs/v5/ncpackages", HttpMethod.Post, body).Content.ReadAsStringAsync().Result;
        var jsonObject = JsonConvert.DeserializeObject(responseText);
        string formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

        createPack_Output.Text = formattedJson;
        
        resetCreatePack();
    }
    
    private void resetCreatePack()
    {
        createPack_Name.Text = "";
        createPack_Id.Text = "";
        createPack_Desc.Text = "";
        createPack_Author.Text = "";
        createPack_Trial.Text = "";
        createPack_ManVerification.Text = "";
        createPack_Output.Text = "";
    }
}