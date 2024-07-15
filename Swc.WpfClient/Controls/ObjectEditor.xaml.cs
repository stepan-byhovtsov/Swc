using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Swc.WpfClient.Controls;

public partial class ObjectEditor : INotifyPropertyChanged
{
   public static readonly DependencyProperty ObjectProperty;
   public static readonly DependencyProperty InspectorProperty;

   static ObjectEditor()
   {
      ObjectProperty = DependencyProperty.Register(nameof(Object), typeof(Marshaller), typeof(ObjectEditor),
         new PropertyMetadata(ObjectChangedCallback));
      InspectorProperty =
         DependencyProperty.Register(nameof(Inspector), typeof(ObjectInspector), typeof(ObjectEditor));
   }

   private static void ObjectChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      ((ObjectEditor) d).OnIsVisibleChanged();
   }

   public Marshaller? Object
   {
      get => (Marshaller?) GetValue(ObjectProperty);
      set
      {
         SetValue(ObjectProperty, value);
         OnPropertyChanged();
      }
   }

   public ObjectInspector? Inspector
   {
      get => (ObjectInspector?) GetValue(InspectorProperty);
      set => SetValue(InspectorProperty, value);
   }

   public ObjectEditor()
   {
      InitializeComponent();
   }

   public event PropertyChangedEventHandler? PropertyChanged;

   public void OnPropertyChanged([CallerMemberName] string prop = "")
   {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
   }

   private void ValidateFloat(object sender, TextCompositionEventArgs e)
   {
      var text = ((TextBox) sender).Text + e.Text;
      if (text.Length == 0)
         text = "0";
      if (!float.TryParse(text, CultureInfo.InvariantCulture, out _))
         e.Handled = true;
   }

   private void ValidateInt(object sender, TextCompositionEventArgs e)
   {
      var text = ((TextBox) sender).Text + e.Text;
      if (text.Length == 0)
         text = "0";
      if (!int.TryParse(text, CultureInfo.InvariantCulture, out _))
         e.Handled = true;
   }

   private void TextBox_GotFocus(object sender, RoutedEventArgs e)
   {
      ((TextBox) sender).SelectAll();
   }

   private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
   {
      var textBox = (TextBox) sender;
      if (textBox.IsKeyboardFocusWithin) return;

      textBox.Focus();
      e.Handled = true;
   }

   private void TextBox_KeyDown(object sender, KeyEventArgs e)
   {
      if (e.Key == Key.Tab)
      {
         var tree = Inspector!.TreeObserver;
         var item = (ObjectPresentation) tree.SelectedItem;

         var list = new List<TextBox>();
         GetChildrenOfType(list, this);
         foreach (var textbox in list)
         {
            textbox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
         }

         if (item.Properties.Count > 0)
         {
            var container = ContainerFromItemRecursive(tree.ItemContainerGenerator, item)!;
            if (container.IsExpanded)
            {
               ContainerFromItemRecursive(tree.ItemContainerGenerator, item.Properties[0])!.IsSelected = true;
            }
            else
            {
               var focusedElement = Keyboard.FocusedElement;
               container.IsExpanded = true;
               Dispatcher.BeginInvoke(
                  DispatcherPriority.ContextIdle,
                  () => focusedElement.Focus());
               
               Dispatcher.BeginInvoke(
                  DispatcherPriority.ContextIdle,
                  () => ContainerFromItemRecursive(tree.ItemContainerGenerator, item.Properties[0])!.IsSelected = true);
            }
         }
         else
         {
            var current = item;
            while (current.Parent != null)
            {
               var properties = current.Parent!.Properties;
               var index = properties.IndexOf(current);
               if (index < properties.Count - 1)
               {
                  ContainerFromItemRecursive(tree.ItemContainerGenerator,
                     properties[index + 1])!.IsSelected = true;
                  break;
               }

               current = current.Parent;
            }
         }
      }
   }

   public static TreeViewItem? ContainerFromItemRecursive(ItemContainerGenerator? root, object item)
   {
      if (root == null)
         return null;
      var treeViewItem = root.ContainerFromItem(item) as TreeViewItem;
      if (treeViewItem != null)
         return treeViewItem;
      foreach (var subItem in root.Items)
      {
         treeViewItem = root.ContainerFromItem(subItem) as TreeViewItem;
         var search = ContainerFromItemRecursive(treeViewItem?.ItemContainerGenerator!, item);
         if (search != null)
            return search;
      }

      return null;
   }

   private void OnIsVisibleChanged()
   {
      var textBox = GetChildOfType<TextBox>(this);
      if (textBox != null)
      {
         Dispatcher.BeginInvoke(
            DispatcherPriority.ContextIdle,
            () => textBox.Focus());
      }
   }

   public static T? GetChildOfType<T>(DependencyObject depObj)
      where T : DependencyObject
   {
      if (depObj == null) return null;

      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
      {
         var child = VisualTreeHelper.GetChild(depObj, i);

         var result = (child as T) ?? GetChildOfType<T>(child);
         if (result != null)
         {
            return result;
         }
      }

      return null;
   }

   public static void GetChildrenOfType<T>(List<T> list, DependencyObject depObj)
      where T : DependencyObject
   {
      if (depObj == null) return;

      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
      {
         var child = VisualTreeHelper.GetChild(depObj, i);

         var result = (child as T) ?? GetChildOfType<T>(child);
         if (result != null)
         {
            list.Add(result);
         }
      }
   }

   private void TextBox_VisibilityChanged(object sender, RoutedEventArgs routedEventArgs)
   {
      OnIsVisibleChanged();
   }

   private void ComboBox_GotFocus(object sender, KeyboardFocusChangedEventArgs e)
   {
      if (sender is not ComboBox comboBox)
         return;

      comboBox.IsDropDownOpen = true;
   }
}