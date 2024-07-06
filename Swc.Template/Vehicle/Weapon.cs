namespace Swc.Template.Vehicle;

public abstract class Weapon
{
   public class LaunchPad : Weapon
   {
      public GridConnection? Connection { get; set; }
      public Direction Direction { get; set; } = new Direction.Forward();
      [Unit(Unit.Blocks)] [Int] public Vector3 MaxWeaponSize { get; set; }
   }
   
   public class Gun : Weapon
   {
      
   }
}
