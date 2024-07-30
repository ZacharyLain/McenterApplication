using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using McenterTest.Services;
using Newtonsoft.Json;

namespace McenterTest.UI.ModelViewViewModel.View.MmrtSubView;

public partial class GetToolInstancesView : UserControl
{
    public GetToolInstancesView()
    {
        InitializeComponent();
    }

    private void getToolInstances(object sender, RoutedEventArgs e)
    {
        string responseText = HttpRequests.httpRequest(" /toolassembly/v2/toolinstances", HttpMethod.Get, null).Content.ReadAsStringAsync().Result;
        var jsonObject = JsonConvert.DeserializeObject(responseText);
        string formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

        outputBox.Text = formattedJson;
        Console.Out.WriteLine("\n\n" + formattedJson + "\n\n");
    }
}