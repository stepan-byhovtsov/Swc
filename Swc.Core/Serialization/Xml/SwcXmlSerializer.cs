using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Swc.Core.Serialization.Xml;

public class SwcXmlSerializer : ISwcSerializer
{
   private List<Type> AdditionalTypes { get; } = new List<Type>();

   public SwcXmlSerializer(Type type)
   {
      AddTypesFrom(type);
   }

   private void AddTypesFrom(Type type)
   {
      foreach (var property in type.GetProperties())
      {
         if (property.PropertyType.IsAbstract)
         {
            AdditionalTypes.AddRange(property.PropertyType.GetNestedTypes());
         }
      }
   }
   
   public void Serialize(object obj, Type type, Stream stream)
   {
      var serializer = new XmlSerializer(type, AdditionalTypes.ToArray());
      serializer.Serialize(stream, obj);
   }

   public object? Deserialize(Type type, Stream stream)
   {
      var serializer = new XmlSerializer(type, AdditionalTypes.ToArray());
      return serializer.Deserialize(stream);
   }
}
