using System.Reflection;
using System.Text.RegularExpressions;

namespace Swc.Core.Helpers;

public class PropertyPath
{
   public PropertyPath(IProperty[] properties)
   {
      Properties = properties;
   }

   public static PropertyPath FromVisualPath(string str)
   {
      var (args, left, _) = SimplifyPath(str, "");

      var names = args.PathInArrays;
      if (names.Length == 0)
         return new PropertyPath(left.Split('.').Select(name => (IProperty) new Property(name)).ToArray());

      List<IProperty> properties = [];

      foreach (var arrayDef in names)
      {
         properties.AddRange(arrayDef.path.Split('.').Select(name => new Property(name)));
         properties.Add(new ArrayProperty(arrayDef.type));
      }
      properties.AddRange(names.Last().field.Split('.').Select(name => (IProperty) new Property(name)).ToArray());
      
      return new PropertyPath(properties.ToArray());
   }
   

   public object? GetValue(object obj)
   {
      var current = obj;
      foreach (var property in Properties)
      {
         current = property.GetValue(current);
         if (current == null)
            break;
      }

      return current;
   }

   public IProperty[] Properties { get; }
   
   public static (QueryArguments args, string leftOperand, string rightOperand) SimplifyPath(string leftOperand, string rightOperand)
   {
      var args = new QueryArguments();
      
      var comment1 = new Regex(@"\[([^\.]*)\]");
      var comment2 = new Regex(@"\(([^\.]*)\)");

      var arrayComment = new Regex(@"\[Any\]\(([^\(]*)\)\.?");
      var arrayDefs = arrayComment.Matches(leftOperand).ToArray();
      var arrays = arrayComment.Split(leftOperand);
      args.PathInArrays = new (string, string, string)[arrayDefs.Length];
      for (int i = 0; i < arrayDefs.Length; i++)
      {
         args.PathInArrays[i] = (comment2.Replace(arrays[i*2], ""), arrayDefs[i].Groups[1].Value, comment2.Replace(arrays[i*2+2],""));
      }
      
      leftOperand = comment1.Replace(leftOperand, "");
      
      if (leftOperand.EndsWith(')'))
      {
         var substring = leftOperand[(leftOperand.LastIndexOf('(')+1) .. ^1];
         args.ShouldSpecifyType = true;
         args.SpecificType = substring;
      }
      
      leftOperand = comment2.Replace(leftOperand, "");
      if (leftOperand.EndsWith("#Count"))
      {
         args.ShouldWorkWithArrayLength = true;
         leftOperand = leftOperand[..^"#Count".Length];
      }

      return (args, leftOperand, rightOperand);
   }
}

public interface IProperty
{
   public object? GetValue(object obj);
}

public class Property(string name) : IProperty
{
   public string Name { get; } = name;

   public object? GetValue(object obj)
   {
      var propertyInfo = obj.GetType().GetProperty(Name);
      return propertyInfo == null ? obj.GetType().GetField(Name)!.GetValue(obj) : propertyInfo.GetValue(obj);
   }
}

public class ArrayProperty(string type) : IProperty
{
   public string Type { get; } = type;

   public object? GetValue(object obj)
   {
      var array = (Array) obj;
      return array.Cast<object?>().FirstOrDefault(item => item!.GetType().Name == Type);
   }
}

public class QueryArguments
{
   /// <summary>
   /// Specifies, whether we should work not with an array object, but with its length
   /// </summary>
   public bool ShouldWorkWithArrayLength { get; set; }
   
   public bool ShouldSpecifyType { get; set; }
   public string? SpecificType { get; set; }
   
   public (string path, string type, string field)[] PathInArrays { get; set; }
}