namespace Swc.Core.Attributes.Text;

[AttributeUsage(AttributeTargets.Property)]
public class MultilineAttribute : MultipleValidationAttribute
{
   protected override bool IsSingleObjectValid(object? value)
   {
      return true;
   }
}
