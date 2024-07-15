using System.Reflection;
using System.Text.RegularExpressions;

namespace Swc.Core.Helpers;

public class PropertyPath
{
   public PropertyPath(string[] properties)
   {
      Properties = properties;
   }

   public static PropertyPath FromVisualPath(string str)
   {
      var (_, realPath, _) = SimplifyPath(str, "");

      var names = realPath.Split('.');

      return new PropertyPath(names);
   }
   

   public object? GetValue(object obj)
   {
      var current = obj;
      foreach (var property in Properties)
      {
         var propertyInfo = current.GetType().GetProperty(property);
         current = propertyInfo == null ? current.GetType().GetField(property)!.GetValue(current) : propertyInfo.GetValue(current);
         if (current == null)
            break;
      }

      return current;
   }

   public string[] Properties { get; }
   
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