using System.Configuration;
using System.Data;
using System.Windows;
using McenterTest.Infrastructure;
using McenterTest.usecases.MMRP;

namespace McenterTest;

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