using System.Windows;
using System.Windows.Input;

namespace Swc.WpfClient.Windows;

public partial class SettingsWindow : Window
{
   public MainWindow MainWindow { get; }
   
   public SettingsWindow(MainWindow mainWindow)
   {
      InitializeComponent();
      MainWindow = mainWindow;
      Owner = mainWindow;
   }
   
   private void ExitWindow(object sender, ExecutedRoutedEventArgs e)
   {
      Hide();
   }
}
