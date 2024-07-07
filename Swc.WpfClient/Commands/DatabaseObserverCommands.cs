using System.Windows.Input;
using Swc.WpfClient.Windows;

namespace Swc.WpfClient.Commands;

public class DatabaseObserverCommands
{
   public static RoutedCommand Refresh { get; set; } = new("Refresh", typeof(DatabaseObserverWindow))
      {InputGestures = {new KeyGesture(Key.Enter)}};
   public static RoutedCommand AddFilter { get; set; } = new("Add filter", typeof(DatabaseObserverWindow));

   public static RoutedCommand DeleteSelectedFilters { get; set; } =
      new("Delete selected filters", typeof(DatabaseObserverWindow))
         {InputGestures = {new KeyGesture(Key.Delete)}};
   
   //   {InputGestures = {new KeyGesture(Key.N, ModifierKeys.Control)}};
}
