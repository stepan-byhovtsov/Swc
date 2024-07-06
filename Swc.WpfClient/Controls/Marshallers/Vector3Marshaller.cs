using System.Numerics;

using Swc.Core.Attributes;
namespace Swc.WpfClient.Controls;

public sealed class Vector3Marshaller : Marshaller
{
   public Vector3Marshaller(object? value)
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
      get => ((Vector3) Value!).X;
      set
      {
         var vec = (Vector3) Value!;
         vec.X = value;
         Value = vec;
      }
   }
   
   public float Y
   {
      get => ((Vector3) Value!).Y;
      set
      {
         var vec = (Vector3) Value!;
         vec.Y = value;
         Value = vec;
      }
   }
   
   public float Z
   {
      get => ((Vector3) Value!).Z;
      set
      {
         var vec = (Vector3) Value!;
         vec.Z = value;
         Value = vec;
      }
   }
}
