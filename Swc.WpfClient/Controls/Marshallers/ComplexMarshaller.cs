namespace Swc.WpfClient.Controls;

public sealed class ComplexMarshaller : Marshaller
{
   public Type[] Options { get; }
   public string[] OptionNames => Options.Select(c => c.Name).ToArray();
   private int _selectedIndex = -1;
   public int SelectedIndex
   {
      get => _selectedIndex;
      set
      {
         if (SelectedIndex != value)
         {
            Value = Options[value].GetConstructor(Type.EmptyTypes)!.Invoke(Array.Empty<object>());
         }

         _selectedIndex = value;
      } 
   }
   
   public ComplexMarshaller(object? obj, Type type)
   {
      var nestedTypes = type.GetNestedTypes();
      Options = nestedTypes;
      SelectedIndex = -1;
      
      if (obj is not null)
      {
         SelectedIndex = Array.FindIndex(Options, c => c == obj.GetType());
      }
      
      if (SelectedIndex != -1)
      {
         Value = obj ?? Options[SelectedIndex].GetConstructor(Type.EmptyTypes)!.Invoke(Array.Empty<object>());
      }
   }
   
   public override object? Value
   {
      get => GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }
}
