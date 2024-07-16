namespace Swc.Template.Vehicle;

public abstract class Weapon
{
   public class LaunchPad : Weapon
   {
      public VehicleConnection? Connection { get; set; }
      public Direction Direction { get; set; } = new Direction.Forward();
      [Unit(Unit.Blocks)] [Int] public Vector3 MaxWeaponSize { get; set; }
   }

   public class Cannon : Weapon
   {
      public CannonType? Type { get; set; }
      public ReloadInfo? ReloadInfo { get; set; }
   }
}

public abstract class CannonType
{
   public class MachineGun : CannonType { }
   public class LightAutocannon : CannonType { }
   public class HeavyAutocannon : CannonType { }
   public class RotaryAutocannon : CannonType { }
   public class BattleCannon : CannonType { }
   public class ArtilleryCannon : CannonType { }
   public class BerthaCannon : CannonType { }
}