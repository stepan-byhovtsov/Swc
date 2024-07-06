using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Swc.Core.Attributes;
using Swc.Template;

namespace Swc.WpfClient.Controls;

public partial class CreationInspector : UserControl
{

   public SwcObject? Creation
   {
      get => (SwcObject?) Presentation.Value;
      set => Presentation.Value = value;
   }

   public ObjectPresentation Presentation { get; set; } = new("Creation", typeof(SwcObject));
   
   public CreationInspector()
   {
      Creation = new SwcObject();
      InitializeComponent();
   }
   
   private void OnSelectedChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
   {
      if (e.NewValue is ObjectPresentation newPres)
      {
         Editor.Object = InstantiateMarshaller(newPres.Value, newPres.Type, newPres);
         BindingOperations.SetBinding(newPres, ObjectPresentation.ValueProperty, new Binding("Value") { Source = Editor.Object, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged});
      }
      else
      {
         Editor.Object = null;
      }
   }

   private static Marshaller? InstantiateMarshaller(object? obj, Type type, ObjectPresentation presentation)
   {
      if (obj is string)
      {
         return new StringMarshaller(obj);
      }
      
      if (obj is float)
      {
         return new FloatMarshaller(obj) { Unit = presentation.Unit };
      }
      
      if (obj is int)
      {
         return new IntMarshaller(obj) { Unit = presentation.Unit };
      }
      
      if (obj is Vector3)
      {
         return new Vector3Marshaller(obj) { Unit = presentation.Unit };
      }
      
      if (obj is Vector2)
      {
         return new Vector2Marshaller(obj) { Unit = presentation.Unit };
      }

      if (type.IsAbstract)
      {
         return new ComplexMarshaller(obj, type);
      }

      if (type.IsArray)
      {
         return new ArrayMarshaller(obj, type);
      }

      return new NonEditableMarshaller(obj);
   }
}
