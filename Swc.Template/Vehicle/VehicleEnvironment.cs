namespace Swc.Template.Vehicle;

public abstract class VehicleEnvironment
{
   public FuelType[] FuelTypes { get; set; } = Array.Empty<FuelType>();
   
   public class Space : VehicleEnvironment
   {
   }

   public class Air : VehicleEnvironment
   {
      [Unit(Unit.MetersPerSecond)] public float MinSpeed { get; set; }
      [Unit(Unit.MetersPerSecond)]public float MaxSpeed { get; set; }
      [Unit(Unit.Meters)] public int MaxAltitude { get; set; }
      [Unit(Unit.TurnsPerSecond)] public Vector3 TurnSpeeds { get; set; }
   }

   public class Land : VehicleEnvironment
   {
      [Unit(Unit.MetersPerSecond)] public float MaxSpeed { get; set; }
   }

   public class Water : VehicleEnvironment
   {
      [Unit(Unit.MetersPerSecond)] public float MaxSpeed { get; set; }
   }

   public class Underwater : VehicleEnvironment
   {
      [Unit(Unit.MetersPerSecond)] public float MaxSpeed { get; set; }
      [Unit(Unit.Meters)] public float MaxDepth { get; set; }
      [Unit(Unit.Seconds)] public float MaxTimeUnderwater { get; set; }
   }
}
