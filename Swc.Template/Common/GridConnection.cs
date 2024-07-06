namespace Swc.Template.Common;

public abstract class GridConnection
{
   public class HardpointConnection : GridConnection
   {
      [Unit(Unit.Blocks)] [Int] public Vector3 PositionOfHardpoint { get; set; }
   }
}
