using System.Windows.Input;

namespace Swc.WpfClient.Commands;

public class EditorCommands
{
   public static RoutedCommand NewFile { get; set; } = new("New File", typeof(MainWindow))
      {InputGestures = {new KeyGesture(Key.N, ModifierKeys.Control)}};

   public static RoutedCommand OpenFile { get; set; } = new("Open File", typeof(MainWindow))
      {InputGestures = {new KeyGesture(Key.O, ModifierKeys.Control | ModifierKeys.Shift)}};

   public static RoutedCommand SaveFile { get; set; } = new("Save File", typeof(MainWindow))
      {InputGestures = {new KeyGesture(Key.S, ModifierKeys.Control)}};

   public static RoutedCommand SaveFileAs { get; set; } = new("Save File As", typeof(MainWindow))
      {InputGestures = {new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)}};


   public static RoutedCommand DownloadFromDb { get; set; } = new("Download vehicle file from Database", typeof(MainWindow));
   public static RoutedCommand SaveToDb { get; set; } = new("Save To Database", typeof(MainWindow));
   public static RoutedCommand SelectVehicleFile { get; set; } = new("Save To Database", typeof(MainWindow));
   public static RoutedCommand DatabaseObserver { get; set; } = new("Database Observer", typeof(MainWindow));
   
   public static RoutedCommand OpenSettings { get; set; } = new("Open settings", typeof(MainWindow));
   //{ InputGestures = { new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift) }};
}
