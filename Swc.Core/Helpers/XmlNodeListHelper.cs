using System.Xml;

namespace Swc.Core.Helpers;

public static class XmlNodeListHelper
{
   public static IEnumerable<T> Select<T>(this XmlNodeList src, Func<XmlNode, T> conv)
   {
      var res = new List<T>();
      foreach (XmlNode item in src)
      {
         res.Add(conv(item));
      }

      return res;
   }
}
