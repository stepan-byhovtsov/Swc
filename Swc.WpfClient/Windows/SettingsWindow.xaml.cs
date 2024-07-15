using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Input;
using Swc.Core.Serialization.Json;
using Swc.Template;
using Swc.WpfClient.Properties;

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

   public static AuthorInfo AuthorInfo
   {
      get => SwcJsonSerializer.Deserialize<AuthorInfo>(JsonNode.Parse(Settings.Default.AuthorInfo));
      set => Settings.Default.AuthorInfo = SwcJsonSerializer.SerializeToJsonNode<AuthorInfo>(value).ToJsonString();
   }
   
   private void ExitWindow(object sender, ExecutedRoutedEventArgs e)
   {
      AuthorInfo = (AuthorInfo) AuthorInfoInspector.Object!;
      Hide();
   }
}
