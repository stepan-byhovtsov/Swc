namespace Swc.Core.Attributes.Text;

[AttributeUsage(AttributeTargets.Property)]
public class SinglelineAttribute : MultipleValidationAttribute
{
   protected override bool IsSingleObjectValid(object? value)
   {
      if (value is string str)
      {
         if (str.Contains('\n'))
         {
            ErrorMessage = "The string should contain single line";
            return false;
         }

         return true;
      }
      
      return true;
   }
}
