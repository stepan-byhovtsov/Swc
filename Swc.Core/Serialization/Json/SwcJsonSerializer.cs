using System.Diagnostics;
using System.Numerics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using MongoDB.Bson;
using Swc.Core.Helpers;

namespace Swc.Core.Serialization.Json;

public class SwcJsonSerializer : ISwcSerializer
{
   private static bool WriteIndented => Debugger.IsAttached;
   
   public void Serialize(object obj, Type type, Stream stream)
   {
      var json = Serialize(obj, type)!;
      var writer = new Utf8JsonWriter(stream,
         new JsonWriterOptions {Indented = WriteIndented, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
      json.WriteTo(writer);
      writer.Flush();
   }

   public static JsonNode SerializeToJsonNode<T>(object obj)
   {
      return SerializeToJsonNode(obj, typeof(T));
   }
   
   public static JsonNode SerializeToJsonNode(object obj, Type type)
   {
      return Serialize(obj, type)!;
   }

   private static JsonNode? Serialize(object? obj, Type type)
   {
      if (obj is null)
      {
         return null;
      }

      if (obj is float or string or int or bool)
      {
         return JsonValue.Create(obj);
      }

      if (obj is Vector3 vec3)
      {
         return new JsonObject {{"X", vec3.X}, {"Y", vec3.Y}, {"Z", vec3.Z}};
      }

      if (obj is Vector2 vec2)
      {
         return new JsonObject {{"X", vec2.X}, {"Y", vec2.Y}};
      }
      
      if (obj is ObjectId id)
      {
         return JsonValue.Create(id.ToString());
      }


      if (type.IsAbstract)
      {
         return SerializeAbstractType(obj, type);
      }

      if (type.IsArray)
      {
         return SerializeArrayType(obj, type);
      }

      return SerializeComplexType(obj, type);
   }

   private static JsonArray SerializeArrayType(object o, Type type)
   {
      var json = new JsonArray();
      foreach (var obj in (Array) o)
      {
         json.Add(Serialize(obj, type.GetElementType()!));
      }

      return json;
   }

   private static JsonObject SerializeComplexType(object o, Type type)
   {
      var json = new JsonObject();
      foreach (var property in type.GetSerializedProperties())
      {
         json.Add(property.Name, Serialize(property.GetValue(o), property.PropertyType));
      }

      return json;
   }

   private static JsonObject SerializeAbstractType(object obj, Type type)
   {
      var json = SerializeComplexType(obj, obj.GetType());
      json.Add("@Type", obj.GetType().Name);
      return json;
   }

   public object? Deserialize(Type type, Stream stream)
   {
      var json = JsonNode.Parse(stream);

      return Deserialize(json, type);
   }

   public static T Deserialize<T>(JsonNode? value)
   {
      return (T) Deserialize(value, typeof(T))!;
   }
   
   public static object? Deserialize(JsonNode? value, Type type)
   {
      if (value == null)
         return null;

      if (type.IsAbstract)
         return DeserializeAbstractType(value, type);
      
      if (type == typeof(ObjectId?))
         return ObjectId.Parse(value.GetValue<string>());

      return value.GetValueKind() switch
      {
         JsonValueKind.Array => DeserializeArrayType(value.AsArray(), type),
         JsonValueKind.Object => DeserializeComplexType(value.AsObject(), type),
         _ => value.Deserialize(type)
      };
   }

   private static object? DeserializeArrayType(JsonArray value, Type type)
   {
      var array = Array.CreateInstance(type.GetElementType()!, value.Count);
      for (int i = 0; i < array.Length; i++)
      {
         array.SetValue(Deserialize(value[i], type.GetElementType()!), i);
      }

      return array;
   }

   private static object? DeserializeComplexType(JsonObject value, Type type)
   {
      if (type == typeof(Vector3))
      {
         return new Vector3(value["X"]!.GetValue<float>(),
            value["Y"]!.GetValue<float>(),
            value["Z"]!.GetValue<float>());
      }
      
      if (type == typeof(Vector2))
      {
         return new Vector2(value["X"]!.GetValue<float>(),
            value["Y"]!.GetValue<float>());
      }

      var obj = type.GetConstructor(Type.EmptyTypes)!.Invoke(Array.Empty<object>());
      foreach (var property in type.GetSerializedProperties())
      {
         if (value.ContainsKey(property.Name))
         {
            property.SetValue(obj, Deserialize(value[property.Name]!, property.PropertyType));
         }
      }

      return obj;
   }

   private static object? DeserializeAbstractType(JsonNode value, Type type)
   {
      var obj = value.AsObject();
      var typeName = obj["@Type"]!.AsValue().GetValue<string>();
      var realType = type.GetNestedType(typeName);

      if (realType is null)
      {
         throw new ArgumentException("Input json is obsolete");
      }

      return DeserializeComplexType(obj, realType);
   }
}
