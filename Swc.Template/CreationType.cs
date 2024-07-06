using Swc.Template.Subgrid;
using Swc.Template.Vehicle;

namespace Swc.Template;

public abstract class CreationType
{
   public class Vehicle : CreationType
   {
      [Unit(Unit.Dollars)] public float Cost { get; set; }
      [Unit(Unit.Kilograms)] public float Mass { get; set; }
      [Unit(Unit.Blocks)] [Int] public Vector3 Bounds { get; set; }

      public Purpose[] Purposes { get; set; } = Array.Empty<Purpose>();
      public VehicleEnvironment[] Environments { get; set; } = Array.Empty<VehicleEnvironment>();
   }

   public class Microcontroller : CreationType
   {
      
   }

   public class Subsystem : CreationType
   {
      
   }

   public class Subgrid : CreationType
   {
      [Unit(Unit.Dollars)] public float Cost { get; set; }
      [Unit(Unit.Kilograms)] public float Mass { get; set; }
      [Unit(Unit.Blocks)] [Int] public Vector3 Bounds { get; set; }
      
      public GridConnection? Connection { get; set; }
      public SubgridType? Type { get; set; }
   }
}
