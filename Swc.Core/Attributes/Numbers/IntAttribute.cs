using System.Numerics;

namespace Swc.Core.Attributes.Numbers;

[AttributeUsage(AttributeTargets.Property)]
public class IntAttribute : MultipleValidationAttribute
{
   private static bool IsInteger(float f)
   {
      return Math.Abs((int) f - f) < float.Epsilon;
   }

   protected override bool IsSingleObjectValid(object? value)
   {
      if (value is Vector3 vec3)
      {
         if (!(IsInteger(vec3.X) && IsInteger(vec3.Y) && IsInteger(vec3.Z)))
         {
            ErrorMessage = "Vector's coordinates should be integers";
            return false;
         }
      }

      if (value is Vector2 vec2)
      {
         if (!(IsInteger(vec2.X) && IsInteger(vec2.Y)))
         {
            ErrorMessage = "Vector's coordinates should be integers";
            return false;
         }
      }

      return true;
   }
}
