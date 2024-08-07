namespace Swc.Template.Common;

public abstract class FuelType
{
   [Unit(Unit.Litres)] 
   public float Amount { get; set; }
   
   [Unit(Unit.LitresPerSecond)] 
   public float Consumption { get; set; }
   
   [DependsOn(nameof(Amount))] 
   [DependsOn(nameof(Consumption))]
   [Unit(Unit.Minutes)] 
   public float TimeUntilFuelRunOut => Amount / Consumption / 60f;
   
   
   public Bool CanBeRefilled { get; set; } = new Bool.False();
   
   
   public class Diesel : FuelType
   {
      
   }

   public class JetFuel : FuelType
   {
      
   }

   public class Coal : FuelType
   {
      
   }

   public class NuclearRods : FuelType
   {
      
   }

   public class Water : FuelType
   {
      
   }

   public class Hydrogen : FuelType
   {
      
   }

   public class Oxygen : FuelType
   {
      
   }
}
