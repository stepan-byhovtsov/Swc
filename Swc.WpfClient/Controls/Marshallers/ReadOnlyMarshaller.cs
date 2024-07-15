namespace Swc.WpfClient.Controls;

public class ReadOnlyMarshaller : Marshaller
{
   public ReadOnlyMarshaller(object? value)
   {
      Value = value;
   }
   
   public override object? Value
   {
      get => GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }
}
