using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Swc.Core.Serialization;
using Swc.Core.Serialization.Json;
using Swc.Template;
using Swc.WpfClient.Windows;

namespace Swc.WpfClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
   public MongoDbHelper MongoDb { get; } = new();

   private readonly ISwcSerializer _serializer = new SwcJsonSerializer(); // new SwcXmlSerializer(typeof(Creation));

   public static readonly DependencyProperty FileNameProperty =
      DependencyProperty.Register(nameof(FileName), typeof(string), typeof(MainWindow));

   public DatabaseObserverWindow? DatabaseObserverWindow { get; private set; }
   public SettingsWindow? SettingsWindow { get; private set; }

   public string FileName
   {
      get => (string) GetValue(FileNameProperty);
      set => SetValue(FileNameProperty, value);
   }

   public MainWindow()
   {
      InitializeComponent();
      UpdateSystemButtonsVisibility();
      NewFile(null!, null!);
   }

   #region Window commands

   protected override void OnStateChanged(EventArgs e)
   {
      base.OnStateChanged(e);
      UpdateSystemButtonsVisibility();
   }


   private void UpdateSystemButtonsVisibility()
   {
      if (WindowState == WindowState.Maximized)
      {
         RestoreButton.Visibility = Visibility.Visible;
         MaximizeButton.Visibility = Visibility.Collapsed;
      }
      else
      {
         RestoreButton.Visibility = Visibility.Collapsed;
         MaximizeButton.Visibility = Visibility.Visible;
      }
   }

   private void ExitWindow(object sender, ExecutedRoutedEventArgs e)
   {
      Close();
   }

   private void Minimize(object sender, ExecutedRoutedEventArgs e)
   {
      WindowState = WindowState.Minimized;
   }

   private void Maximize(object sender, ExecutedRoutedEventArgs e)
   {
      WindowState = WindowState.Maximized;
   }

   private void Restore(object sender, ExecutedRoutedEventArgs e)
   {
      WindowState = WindowState.Normal;
   }

   #endregion

   #region File commands

   private void NewFile(object sender, ExecutedRoutedEventArgs e)
   {
      FileName = "";
      Inspector.Object = new SwcObject {AuthorInfos = [SettingsWindow.AuthorInfo]};
   }

   private void OpenFile(object sender, ExecutedRoutedEventArgs e)
   {
      var dialog = new OpenFileDialog();
      dialog.Filter = "SWC save (*.json)|*.json";
      if (!dialog.ShowDialog()!.Value) return;

      using var stream = dialog.OpenFile();
      Inspector.Object = (SwcObject) _serializer.Deserialize(typeof(SwcObject), stream)!;
      FileName = dialog.FileName;
   }

   private void SaveFile(object sender, ExecutedRoutedEventArgs e)
   {
      if (string.IsNullOrEmpty(FileName))
      {
         SaveFileAs(sender, e);
      }
      else
      {
         SaveFile();
      }
   }

   private void SaveFileAs(object sender, ExecutedRoutedEventArgs e)
   {
      var dialog = new SaveFileDialog();
      dialog.Filter = "SWC save (*.json)|*.json";
      if (dialog.ShowDialog() != true) return;

      FileName = dialog.FileName;
      SaveFile();
   }

   private void SaveFile()
   {
      using var stream = new FileStream(FileName, FileMode.Create);
      _serializer.Serialize(Inspector.Object!, typeof(SwcObject), stream);
      stream.Flush();
   }

   private async void SaveToDb(object sender, ExecutedRoutedEventArgs e)
   {
      await MongoDb.SaveToDb((SwcObject) Inspector.Object!);
   }

   #endregion

   private void DbObserver(object sender, ExecutedRoutedEventArgs e)
   {
      if (DatabaseObserverWindow is null || !DatabaseObserverWindow.IsLoaded)
         DatabaseObserverWindow = new DatabaseObserverWindow(this);

      DatabaseObserverWindow.Refresh();
      DatabaseObserverWindow.Show();
   }

   private void OpenSettings(object sender, ExecutedRoutedEventArgs e)
   {
      if (SettingsWindow is null || !SettingsWindow.IsLoaded)
         SettingsWindow = new SettingsWindow(this);

      SettingsWindow.Show();
   }

   private async void DbSelectFile(object sender, ExecutedRoutedEventArgs e)
   {
      OpenFileDialog fileDialog = new OpenFileDialog();
      fileDialog.Filter = "Stormworks vehicle save (*.xml)|*.xml";
      if (fileDialog.ShowDialog() == true)
      {
         var file = await File.ReadAllTextAsync(fileDialog.FileName);
         ((SwcObject) Inspector.Object!).File = file;
         await MongoDb.UploadFile((SwcObject) Inspector.Object);
      }
   }

   private async void DownloadFromDb(object sender, ExecutedRoutedEventArgs e)
   {
      await MongoDb.DownloadFile((SwcObject) Inspector.Object!);

      SaveFileDialog fileDialog = new SaveFileDialog();
      fileDialog.Filter = "Stormworks vehicle save (*.xml)|*.xml";
      if (fileDialog.ShowDialog() == true)
      {
         await File.WriteAllTextAsync(fileDialog.FileName, ((SwcObject) Inspector.Object!).File);
      }
   }

   private void Refresh(object sender, ExecutedRoutedEventArgs e)
   {
      var was = Inspector.Object;
      Inspector.Object = new SwcObject();
      Inspector.Object = was;
   }
}