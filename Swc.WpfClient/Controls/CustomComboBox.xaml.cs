using System.Collections;
using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Swc.WpfClient.Controls;

public partial class CustomComboBox : UserControl
{
   public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
      typeof(IEnumerable), typeof(CustomComboBox), new PropertyMetadata(ItemsSourceChanged));

   private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      var self = (CustomComboBox) d;
      self.UpdateItemsSource((IEnumerable) e.NewValue);
   }

   private void UpdateItemsSource(IEnumerable? newValue)
   {
      SearchTextBox.Text = "";
      SelectedItems.Clear();
      if (newValue == null)
         return;
      foreach (var item in newValue)
      {
         SelectedItems.Add(item?.ToString() ?? "null");
      }
   }
   
   private void UpdateSelectedIndex(int newValue)
   {
      if (newValue < 0 || newValue >= SelectedItems.Count)
         return;
      
      SearchTextBox.Text = SelectedItems[newValue];
      Value = Value;
   }

   public static readonly DependencyProperty SelectedIndexProperty =
      DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(CustomComboBox), new PropertyMetadata(SelectedIndexPropertyChanged));

   private static void SelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
   {
      ((CustomComboBox) d).UpdateSelectedIndex((int) e.NewValue);
   }

   public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register(nameof(Value), typeof(string), typeof(CustomComboBox));

   public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof(SelectedItems),
      typeof(ObservableCollection<string>), typeof(CustomComboBox));

   public IEnumerable ItemsSource
   {
      get => (IEnumerable) GetValue(ItemsSourceProperty);
      set => SetValue(ItemsSourceProperty, value);
   }

   public ObservableCollection<string> SelectedItems
   {
      get => (ObservableCollection<string>) GetValue(SelectedItemsProperty);
      set => SetValue(SelectedItemsProperty, value);
   }

   public int SelectedIndex
   {
      get => (int) GetValue(SelectedIndexProperty);
      set => SetValue(SelectedIndexProperty, value);
   }
   
   public string Value
   {
      get => SearchTextBox.Text;
      set => SetValue(ValueProperty, value);
   }

   public CustomComboBox()
   {
      InitializeComponent();
      SelectedItems = [];
   }

   private void SearchTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
   {
      Popup.IsOpen = true;
   }

   private void SearchTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
   {
      Popup.IsOpen = false;
   }

   private Stack<List<string>> _removedProperties = [];

   private void FilterOut()
   {
      var text = SearchTextBox.Text[0 .. SearchTextBox.SelectionStart];
      var propertiesToRemove = SelectedItems.Where(property =>
         !property.StartsWith(text) &&
         !property[(property.LastIndexOf('.') + 1)..].StartsWith(text)).ToList();
      _removedProperties.Push(propertiesToRemove);
      foreach (var property in propertiesToRemove)
      {
         SelectedItems.Remove(property);
      }
   }

   private void FilterIn()
   {
      foreach (var property in _removedProperties.Pop())
      {
         SelectedItems.Add(property);
      }
   }

   private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
   {
      foreach (var change in e.Changes)
      {
         for (int i = 0; i < change.RemovedLength; i++) 
         {
            FilterIn();
         }
         
         for (int i = 0; i < change.AddedLength; i++)
         {
            FilterOut();
         }

         if (change.AddedLength > 0)
         {
            if (SelectedItems.Count > 0)
            {
               if (SelectedItems[0].StartsWith(SearchTextBox.Text) && SearchTextBox.Text != SelectedItems[0])
               {
                  var caret = SearchTextBox.CaretIndex;
                  SearchTextBox.Text = SelectedItems[0];
                  SearchTextBox.SelectionStart = caret;
                  SearchTextBox.SelectionLength = SearchTextBox.Text.Length - SearchTextBox.SelectionStart;
               }
            }
            else
            {
               SearchTextBox.SelectionLength = 0;
            }
         }
      }
   }

   private void OnKeyDown(object sender, KeyEventArgs e)
   {
      if (e.Key == Key.LeftShift)
         return;
      if (e.Key == Key.LeftCtrl)
         return;
      if (e.Key == Key.Tab)
      {
         if (SelectedIndex < 0 || SelectedIndex >= SelectedItems.Count)
            SelectedIndex = 0;
         
         if (SelectedItems.Count < 0)
            return;
         
         SearchTextBox.Text = SelectedItems[SelectedIndex];
         SearchTextBox.CaretIndex = SearchTextBox.Text.Length;
         SearchTextBox.SelectionLength = 0;
         e.Handled = true;
      }
      
      if (e.Key == Key.Up)
      {
         SelectedIndex--;
         e.Handled = true;
      }
      
      if (e.Key == Key.Down)
      {
         SelectedIndex++;
         e.Handled = true;
      }

      if (SearchTextBox.SelectionLength > 0 && char.ToLower(e.Key.ToString()[0]) ==
          char.ToLower(SearchTextBox.Text[SearchTextBox.SelectionStart]))
      {
         SearchTextBox.SelectionStart++;
         e.Handled = true;
      }

      if (e.Key is Key.Enter)
      {
         // MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
         // e.Handled = true;
      }
   }
}