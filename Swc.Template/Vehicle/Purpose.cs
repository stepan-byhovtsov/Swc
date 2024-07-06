namespace Swc.Template.Vehicle;

public abstract class Purpose
{
   public class PassengerTransportation : Purpose
   {
      [Unit(Unit.Persons)] public int PassengerCount { get; set; }
      public Bool HasHeater { get; set; } = new Bool.False();
   }

   public class ContainerTransportation : Purpose
   {
      [Unit(Unit.Containers)] public int ContainersCount { get; set; }
   }

   public class CargoTransportation : Purpose
   {
      public CargoBay[] CargoBays { get; set; } = Array.Empty<CargoBay>();   
   }

   public class Resque : Purpose
   {
      
   }

   public class Military : Purpose
   {
      public Weapon[] Weapons { get; set; } = Array.Empty<Weapon>();
   }
}
