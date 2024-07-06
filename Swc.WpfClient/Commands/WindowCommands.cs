using System.Windows.Input;

namespace Swc.WpfClient.Commands;

public class WindowCommands
{
   public static RoutedCommand Exit { get; set; } = new("Exit", typeof(MainWindow));
   public static RoutedCommand Maximize { get; set; } = new("Maximize", typeof(MainWindow));
   public static RoutedCommand Restore { get; set; } = new("Restore", typeof(MainWindow));
   public static RoutedCommand Minimize { get; set; } = new("Minimize", typeof(MainWindow));
}
