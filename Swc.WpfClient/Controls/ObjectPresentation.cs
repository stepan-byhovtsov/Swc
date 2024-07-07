using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection;
using System.Windows;
using Swc.Core.Attributes;
using Swc.Core.Helpers;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Swc.WpfClient.Controls;

public class ObjectPresentation : DependencyObject
{
   public Type Type { get; }
   
   public Type RealType { get; set; }
   
   public string Name { get; }
   
   public ObjectPresentation? Parent { get; }

   public PropertyInfo? MyPropertyInfo { get; }
   
   public UnitAttribute Unit { get; }
   
   public ObservableCollection<ObjectPresentation> Properties { get; } = new();

   public static readonly DependencyProperty ValueProperty =
      DependencyProperty.Register(nameof(Value), typeof(object), typeof(ObjectPresentation), new PropertyMetadata(ValueChanged));
   
   public static readonly DependencyProperty HasValidationErrorsProperty =
      DependencyProperty.Register(nameof(HasValidationErrors), typeof(bool), typeof(ObjectPresentation));

   public bool HasValidationErrors
   {
      get => (bool) GetValue(HasValidationErrorsProperty);
      private set => SetValue(HasValidationErrorsProperty, value);
   }

   public ObservableCollection<ValidationResult> ValidationErrors { get; } = new();
    
   private void UpdateValue(object? value)
   {
      if (value == null)
         Properties.Clear();
      
      if (value != null && !Type.IsArray && !IsSimpleType(Type))
      {
         RealType = value.GetType();
         GenerateProperties();
      }

      if (!IsSimpleType(Type) && value != null)
      {
         RefreshProperties();
      }

      if (Type.IsArray && value != null)
      {
         GenerateArrayProperties();
         RefreshArrayProperties();
      }

      if (value is not null && Parent != null)
      {
         ValidationErrors.Clear();
         var propertyInfo = Parent.Type.IsArray
            ? Parent!.Parent!.RealType.GetProperty(Parent.Name)!
            : Parent.RealType.GetProperty(Name)!;
         var validationAttributes = propertyInfo.GetCustomAttributes<ValidationAttribute>(true);
         
         if (Validator.TryValidateValue(Value, new ValidationContext(Parent.Value!), ValidationErrors, validationAttributes))
         {
            HasValidationErrors = false;
         }
         else
         {
            HasValidationErrors = true;
         }
      }

      if (Parent != null)
         Parent[Name] = value;
   }
   
   private static void ValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
   {
      var self = (ObjectPresentation) obj;
      var value = args.NewValue;
      self.UpdateValue(value);
   }
   
   public object? Value
   {
      get => GetValue(ValueProperty);
      set => SetValue(ValueProperty, value);
   }
   

   public object? this[string propertyName]
   {
      get
      {
         if (Type.IsArray)
            return ((Array) Value!).GetValue(int.Parse(propertyName)-1);
         return RealType.GetProperties().First(c => c.Name == propertyName).GetValue(Value)!;
      }
      set
      {
         if (Type.IsArray) 
            ((Array) Value!).SetValue(value, int.Parse(propertyName)-1);
         else
            RealType.GetProperties().First(c => c.Name == propertyName).SetValue(Value, value);
      }
   }

   public ObjectPresentation(string name, Type type, ObjectPresentation? parent = null, object? value = null, PropertyInfo? propertyInfo = null, bool readOnly = false)
   {
      Type = type;
      RealType = type;
      MyPropertyInfo = propertyInfo;
      
      if (!IsSimpleType(type))
      {
         GenerateProperties();
      }

      if (type.IsArray)
      {
         GenerateArrayProperties();
      }
      
      Name = name;
      Parent = parent;

      UnitAttribute? unit = null;
      if (propertyInfo != null)
      {
         unit = propertyInfo.GetCustomAttribute<UnitAttribute>();
      }

      Unit = unit ?? new UnitAttribute(Core.Attributes.Unit.Number);
      
      if (value != null)
         Value = value;
   }


   private void RefreshProperties()
   {
      foreach (var property in Properties)
      {
         property.Value = RealType.GetProperty(property.Name)!.GetValue(Value);
      }
   }

   private void RefreshArrayProperties()
   {
      var array = (Array) Value!;
      for (int i = 0; i < array.Length; i++)
      {
         Properties[i].Value = array.GetValue(i);
      }
   }
   
   private void GenerateArrayProperties()
   {
      Properties.Clear();
      
      Value ??= Array.CreateInstance(Type.GetElementType()!, 0);
      
      var array = (Array) Value!;
      for (int i = 0; i < array.Length; i++)
      {
         Properties.Add(new ObjectPresentation((i + 1).ToString(), Type.GetElementType()!, this, propertyInfo: MyPropertyInfo));
      }
   }
   
   private void GenerateProperties()
   {
      Properties.Clear();
      var properties = RealType.GetSerializedProperties();
      foreach (var property in properties)
      {
         var editable = property.GetCustomAttribute<NonEditableAttribute>() == null;
         if (editable)
         {
            Properties.Add(new ObjectPresentation(property.Name, property.PropertyType, this, propertyInfo: property));
         }
      }
   }

   public static object? Instantiate(Type type)
   {
      if (type == typeof(string))
      {
         return "";
      }
      
      var ctor = type.GetConstructor(Type.EmptyTypes);
      return ctor?.Invoke(null);
   }

   private static readonly HashSet<Type> _simpleTypes = new() {typeof(string), typeof(bool), typeof(int), typeof(float), typeof(Vector3), typeof(Vector2)};
   public static bool IsSimpleType(Type type)
   {
      if (type.IsArray)
         return true;
      
      return _simpleTypes.Contains(type);
   }
   
}
