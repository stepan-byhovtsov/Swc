namespace Swc.WpfClient.Controls;

public sealed class NonEditableMarshaller : Marshaller
{
   public NonEditableMarshaller(object? value)
   {
      Value = value;
   }

   public override object? Value
   {
      get => GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }
}
