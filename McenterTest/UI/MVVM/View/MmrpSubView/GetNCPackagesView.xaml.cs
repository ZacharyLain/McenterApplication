using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using McenterTest.Services;
using Newtonsoft.Json;

namespace McenterTest.UI.MVVM.View.MmrpSubView
{
    /// <summary>
    /// Interaction logic for GetNCPackagesView.xaml
    /// </summary>
    public partial class GetNCPackagesView : UserControl
    {
        public GetNCPackagesView()
        {
            InitializeComponent();
        }
        
        private void getNCPackages(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Hit da btn");
            string responseText = HttpRequests.httpRequest("/mmrp/api/programs/v5/ncpackages", HttpMethod.Get, null).Content.ReadAsStringAsync().Result;


            var jsonObject = JsonConvert.DeserializeObject(responseText);
            string formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            outputBox.Text = formattedJson;
        }
    }
}
