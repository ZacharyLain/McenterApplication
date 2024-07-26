using System.Windows;
using System.Windows.Controls;

namespace McenterTest.Navigation;

public class MainWindowNavigation
{
    public static void ChangePage(Page page)
    {
        var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        if (mainWindow != null)
        {
            var frame = mainWindow.FindName("MainFrame") as Frame;
            if (frame != null)
            {
                frame.Navigate(page);
            }
        }
    }
}