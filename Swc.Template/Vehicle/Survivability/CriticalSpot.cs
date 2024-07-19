namespace Swc.Template.Vehicle.Survivability;

public abstract class CriticalSpot
{
   public class Batteries : CriticalSpot { }
   public class Controllers : CriticalSpot { }
   public class Mobility : CriticalSpot { }
   public class Buoyancy : CriticalSpot { }
   public class Ammo : CriticalSpot { }
   public class Weapon : CriticalSpot { }
   public class Pilot : CriticalSpot { }
   public class Navigation : CriticalSpot { }
   
   [Unit(Unit.Blocks)] [Int] public Vector3 Bounds { get; set; }
   [Unit(Unit.Blocks)] public Vector3 Center { get; set; }
   
   public int ArmorLayersCountFront { get; set; }
   public int ArmorLayersCountLeft { get; set; }
   public int ArmorLayersCountRight { get; set; }
   public int ArmorLayersCountBack { get; set; }
   public int ArmorLayersCountTop { get; set; }
   public int ArmorLayersCountBottom { get; set; }

   [Unit(Unit.Seconds)] public float RepairTime { get; set; }

   public Bool IsDifferentGrid { get; set; } = new Bool.False();
}