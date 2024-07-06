using System.Numerics;
using Swc.Core.Attributes;

namespace Swc.WpfClient.Controls;

public sealed class Vector2Marshaller : Marshaller
{
   public Vector2Marshaller(object? value)
   {
      Value = value;
   }

   public override object? Value
   {
      get => GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }
   
   public UnitAttribute Unit { get; set; } = new(Core.Attributes.Unit.Number);

   public float X
   {
      get => ((Vector2) Value!).X;
      set
      {
         var vec = (Vector2) Value!;
         vec.X = value;
         Value = vec;
      }
   }
   
   public float Y
   {
      get => ((Vector2) Value!).Y;
      set
      {
         var vec = (Vector2) Value!;
         vec.Y = value;
         Value = vec;
      }
   }

}
