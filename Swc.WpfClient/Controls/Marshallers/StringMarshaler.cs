namespace Swc.WpfClient.Controls;

public sealed class StringMarshaller : Marshaller
{
   public StringMarshaller(object? value)
   {
      Value = value;
   }
   
   public override object? Value
   {
      get => GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }
}
