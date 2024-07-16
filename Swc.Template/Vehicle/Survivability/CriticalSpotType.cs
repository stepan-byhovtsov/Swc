namespace Swc.Template.Vehicle.Survivability;

public abstract class CriticalSpotType
{
   public class Batteries : CriticalSpotType { }
   public class Controllers : CriticalSpotType { }
   public class Mobility : CriticalSpotType { }
   public class Buoyancy : CriticalSpotType { }
   public class Ammo : CriticalSpotType { }
   public class Weapon : CriticalSpotType { }
   public class Pilot : CriticalSpotType { }
   public class Navigation : CriticalSpotType { }
}