namespace Swc.Template.Common;

public abstract class VehicleConnection
{
   public class HardpointConnection : VehicleConnection
   {
      [Unit(Unit.Blocks)] [Int] public Vector3 PositionOfHardpoint { get; set; }
   }
}
