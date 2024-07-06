using System.ComponentModel.DataAnnotations;

namespace Swc.Core.Attributes;

public abstract class MultipleValidationAttribute : ValidationAttribute
{
   protected abstract bool IsSingleObjectValid(object? value);
   
   public override bool IsValid(object? value)
   {
      // if (value is Array array)
      //    for (int i = 0; i < array.Length; i++)
      //       if (!IsValid(array.GetValue(i)))
      //          return false;

      return IsSingleObjectValid(value);
   }
}
