using System.Configuration;
using System.Data;
using System.Windows;
using McenterTest.Services.Requests.Infrastructure;
using McenterTest.Services.Requests.usecases.MMRP;

namespace McenterTest.Services.Requests;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost _host;
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }
}

internal interface IHost
{
    
}