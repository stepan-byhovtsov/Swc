using System.Configuration;
using System.Data;
using System.Windows;
using Swc.WpfClient.Properties;

namespace Swc.WpfClient;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
   protected override void OnExit(ExitEventArgs e)
   {
      Settings.Default.Save();
   }
}
