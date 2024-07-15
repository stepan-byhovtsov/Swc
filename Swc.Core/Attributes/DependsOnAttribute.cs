namespace Swc.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class DependsOnAttribute(string memberName) : Attribute
{
   public string MemberName { get; } = memberName;
}