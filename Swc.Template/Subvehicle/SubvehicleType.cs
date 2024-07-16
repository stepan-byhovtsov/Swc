namespace Swc.Template.Subvehicle;

public abstract class SubvehicleType
{
   public class Missile : SubvehicleType
   {
      public VehicleConnection[] VehicleConnection { get; set; } = [];
   }
   
   public class FuelTank : SubvehicleType
   {
      
   }
}
