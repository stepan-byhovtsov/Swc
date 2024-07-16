using System.Text;

namespace Swc.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UnitAttribute(Unit unit) : Attribute
{
   public Unit Unit { get; init; } = unit;
   public string FullUnitName => Unit.ToString();
   public string ShortUnitName => FullUnitName.Where(char.IsUpper).Aggregate("", (current, c) => current + c);
}

public enum Unit
{
   Number,
   
   Meters,
   MetersPerSecond,
   
   Litres,
   LitresPerSecond,
   LitresPerMeter,
   
   Kilograms,
   KilogramsPerSecond,
   
   Dollars,
   
   Turns,
   TurnsPerSecond,
   TurnsPerMeter,
   
   Blocks,
   
   Persons,
   Containers,
   
   Seconds,
   Minutes,
   
   Percents
}
