using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Swc.WpfClient.Controls;

public partial class ObjectEditor : INotifyPropertyChanged
{
   public static readonly DependencyProperty ObjectProperty;

   static ObjectEditor()
   {
      ObjectProperty = DependencyProperty.Register(nameof(Object), typeof(Marshaller), typeof(ObjectEditor));
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

   public ObjectEditor()
   {
      InitializeComponent();
   }

   public event PropertyChangedEventHandler? PropertyChanged;
   public void OnPropertyChanged([CallerMemberName]string prop = "")
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
}
