using System.Windows;

namespace Swc.WpfClient.Controls;

public abstract class Marshaller : DependencyObject
{
   public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(Marshaller));
   public abstract object? Value { get; set; }
}
