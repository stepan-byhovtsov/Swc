using Swc.Core.Attributes;

namespace Swc.WpfClient.Controls;

public sealed class FloatMarshaller : Marshaller
{
   public FloatMarshaller(object? value)
   {
      Value = value;
   }

   public UnitAttribute Unit { get; set; } = new(Core.Attributes.Unit.Number);
   
   public override object? Value
   {
      get => GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }
}
