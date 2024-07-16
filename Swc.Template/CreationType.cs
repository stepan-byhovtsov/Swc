using Swc.Template.Subvehicle;
using Swc.Template.Vehicle;
using Swc.Template.Vehicle.Survivability;

namespace Swc.Template;

public abstract class CreationType
{
   public class Vehicle : CreationType
   {
      [Unit(Unit.Dollars)] public float Cost { get; set; }
      [Unit(Unit.Kilograms)] public float Mass { get; set; }
      [Unit(Unit.Blocks)] [Int] public Vector3 Bounds { get; set; }

      public Purpose[] Purposes { get; set; } = [];
      public VehicleEnvironment[] Environments { get; set; } = [];
      public FuelType[] FuelTypes { get; set; } = [];

      public Survivability Survivability { get; set; } = new();
   }

   public class Microcontroller : CreationType
   {
      [Unit(Unit.Blocks)] [Int] public Vector2 Size { get; set; }
   }

   public class System : CreationType
   {
      [Unit(Unit.Dollars)] public float Cost { get; set; }
      [Unit(Unit.Kilograms)] public float Mass { get; set; }
   }

   public class Subvehicle : CreationType
   {
      [Unit(Unit.Dollars)] public float Cost { get; set; }
      [Unit(Unit.Kilograms)] public float Mass { get; set; }
      [Unit(Unit.Blocks)] [Int] public Vector3 Bounds { get; set; }
      
      public SubvehicleType? Type { get; set; }
   }
}