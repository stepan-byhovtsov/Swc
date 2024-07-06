using System.Reflection;
using Swc.Core.Attributes;

namespace Swc.Core.Helpers;

public static class TypeHelper
{
   public static IEnumerable<PropertyInfo> GetSerializedProperties(this Type type)
   {
      return type.GetProperties()
         .Where(c => c is {CanRead: true, CanWrite: true});
   }
}
