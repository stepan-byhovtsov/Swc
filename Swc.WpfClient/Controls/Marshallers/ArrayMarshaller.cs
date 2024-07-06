using System.Reflection;

namespace Swc.WpfClient.Controls;

public sealed class ArrayMarshaller : Marshaller
{
   public Type Type { get; }

   public Array Data => (Array) Value!;

   public int Length
   {
      get => Data.Length;
      set
      {
         if (value > 999)
            value = 999;
         
         if (Length != value)
         {
            ChangeArraySize(value);
         }
      }
   }

   private void ChangeArraySize(int newSize)
   {
      var prevSize = Data.Length;
      
      var prevD = Data;
      var newD = Array.CreateInstance(Data.GetType().GetElementType()!, newSize);

      for (int i = 0; i < newSize; i++)
      {
         newD.SetValue(i < prevSize ? prevD.GetValue(i)! : ObjectPresentation.Instantiate(Type.GetElementType()!)!, i);
      }

      Value = newD;
   }

   public ArrayMarshaller(object? data, Type type)
   {
      Type = type;
      Value = (Array) data!;
   }

   public override object? Value
   {
      get => GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }
}
