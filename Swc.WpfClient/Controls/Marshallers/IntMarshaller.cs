using Swc.Core.Attributes;

namespace Swc.WpfClient.Controls;

public sealed class IntMarshaller : Marshaller
{
   public IntMarshaller(object? value)
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
