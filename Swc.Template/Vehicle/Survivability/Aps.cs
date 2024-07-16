namespace Swc.Template.Vehicle.Survivability;

public abstract class Aps
{
   public class SoftKill : Aps
   {
      public GuidanceType? GuidanceType { get; set; }
      public ApsAutomationLevel? ApsAutomationLevel { get; set; }
      
      public ReloadInfo ReloadInfo { get; set; } = new ReloadInfo.FullAuto();
   }

   public class HardKill : Aps
   {
      [Unit(Unit.Percents)] public float Efficiency { get; set; }
      
      public ApsAutomationLevel? ApsAutomationLevel { get; set; }
      
      public ReloadInfo? ReloadInfo { get; set; }
      
      [Unit(Unit.Meters)] public float MinInterceptionDistance { get; set; }
      [Unit(Unit.Meters)] public float MaxInterceptionDistance { get; set; }
      [Unit(Unit.MetersPerSecond)] public float MaxTargetSpeed { get; set; }
      
      [Unit(Unit.Turns)] public float MinXAngle { get; set; }
      [Unit(Unit.Turns)] public float MaxXAngle { get; set; }
      [Unit(Unit.Turns)] public float MinYAngle { get; set; }
      [Unit(Unit.Turns)] public float MaxYAngle { get; set; }
   }
}