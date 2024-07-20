using System.Text;

namespace Swc.Core.Helpers;

public static class StringBuilderHelper
{
   public static StringBuilder AppendAll(this StringBuilder str, params object[] args)
   {
      foreach (var arg in args)
      {
         str.Append(arg);
      }

      return str;
   }
   
   public static Stack<T> PushAll<T>(this Stack<T> stack, params T[] args)
   {
      foreach (var arg in args)
      {
         stack.Push(arg);
      }

      return stack;
   }
   
   public static char ClosingBrace(this char c)
   {
      return c switch
      {
         '{' => '}',
         '[' => ']',
         _ => c
      };
   }

}