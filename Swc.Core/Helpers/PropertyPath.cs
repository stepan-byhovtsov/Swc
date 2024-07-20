using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;

namespace Swc.Core.Helpers;


public class Query
{
   public object GetValue(object obj)
   {
      foreach (var elem in Elements)
      {
         switch (elem)
         {
            case QueryPropertyElement prop:
               var type = obj.GetType();
               var propertyInfo = type.GetProperty(prop.PropertyName);
               if (propertyInfo == null)
                  obj = type.GetField(prop.PropertyName)!.GetValue(obj)!;
               else
                  obj = propertyInfo.GetValue(obj)!;
               
               break;
            case QueryArrayIndexElement:
               var array1 = (Array) obj;
               obj = array1.GetValue(0)!;
               break;
            case QueryArrayLengthElement:
               var array2 = (Array) obj;
               obj = array2.Length;
               break;
            case QueryArrayTypeElement prop:
               var array = (Array) obj;
               foreach (var item in array)
               {
                  if (item != null && item.GetType().Name == prop.TypeName)
                  {
                     obj = item;
                     break;
                  }
               }
               break;
         }

      }

      return obj;
   }
   
   public List<QueryElement> Elements { get; set; } = [];
   
   public Query Clone()
   {
      return new Query {Elements = [..Elements]};
   }

   public Query Add(QueryElement element)
   {
      Elements.Add(element);
      return this;
   }

   public Query BranchAdd(QueryElement element)
   {
      return Clone().Add(element);
   }
   
   public override string ToString()
   {
      var builder = new StringBuilder();
      foreach (var element in Elements)
      {
         element.AppendVisualPath(builder);
      }

      return builder.ToString();
   }
}

public abstract class QueryElement
{
   public abstract void AppendVisualPath(StringBuilder builder);
}

public class QueryPropertyElement(string propertyName) : QueryElement
{
   public string PropertyName { get; } = propertyName;
   public override void AppendVisualPath(StringBuilder builder)
   {
      if (builder.Length > 0) builder.Append('.');
      builder.Append(PropertyName);
   }
}

public class QueryArrayAllElement : QueryElement
{
   public override void AppendVisualPath(StringBuilder builder)
   {
      builder.Append("[+]");
   }
}

public class QueryArrayAnyElement : QueryElement
{
   public override void AppendVisualPath(StringBuilder builder)
   {
      builder.Append("[*]");
   }
}

public class QueryArrayLengthElement : QueryElement
{
   public override void AppendVisualPath(StringBuilder builder)
   {
      builder.Append("#Count");
   }
}

public class QueryArrayIndexElement(int index) : QueryElement
{
   public int Index { get; } = index;
   
   public override void AppendVisualPath(StringBuilder builder)
   {
      builder.Append('[');
      builder.Append(Index);
      builder.Append(']');
   }
}

public class QueryArrayTypeElement(string typeName) : QueryElement
{
   public string TypeName { get; } = typeName;
   
   public override void AppendVisualPath(StringBuilder builder)
   {
      builder.Append('[');
      builder.Append(TypeName);
      builder.Append(']');
   }
}

public class QueryTypeElement(string typeName) : QueryElement
{
   public string TypeName { get; } = typeName;
   
   public override void AppendVisualPath(StringBuilder builder)
   {
      builder.Append('(');
      builder.Append(TypeName);
      builder.Append(')');
   }
}