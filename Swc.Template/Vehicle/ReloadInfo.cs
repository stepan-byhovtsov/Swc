namespace Swc.Template.Vehicle;

public abstract class ReloadInfo
{
   public abstract int TotalCount { get; }
   
   public class Manual : ReloadInfo
   {
      [Unit(Unit.Seconds)] public float ApproximateReloadTime { get; set; }
      public int AmmunitionStock { get; set; }
      
      [DependsOn(nameof(AmmunitionStock))]
      public override int TotalCount => AmmunitionStock;
   }

   public class SemiAuto : ReloadInfo
   {
      public int FirstStageAmmunition { get; set; }
      public int AmmunitionStock { get; set; }
      
      [Unit(Unit.Seconds)] public float ReloadTimeFromFirstStage { get; set; }
      [Unit(Unit.Seconds)] public float ApproximateReloadTimeFromStock { get; set; }
      
      public Bool CanReloadWhileFiring { get; set; } = new Bool.False();
      
      [DependsOn(nameof(AmmunitionStock))]
      [DependsOn(nameof(FirstStageAmmunition))]
      public override int TotalCount => AmmunitionStock + FirstStageAmmunition;
   }

   public class FullAuto : ReloadInfo
   {
      public int AmmunitionStock { get; set; }
      [Unit(Unit.Seconds)] public float ReloadTime { get; set; }
    
      [DependsOn(nameof(AmmunitionStock))]  
      public override int TotalCount => AmmunitionStock;
   }
}