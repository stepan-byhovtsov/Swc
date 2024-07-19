using MongoDB.Bson;
using MongoDB.Driver;
using Swc.MongoMigrationTemplate.Tools;

namespace Swc.MongoMigrationTemplate.Migrations;

public sealed class Migration_0_1_to_0_2 : Migration
{
   public override string FromVersion => "0.1";
   public override string ToVersion => "0.2";

   protected override void ApplyInternal(BsonDocument old)
   {
      var type = old["Type"];
      if (type is null)
         return;
      if (type["@Type"].AsString != "Vehicle")
         return;
      var survivability = type["Survivability"];
      if (survivability is null)
         return;
      var criticalSpots = (BsonArray) survivability["CriticalSpots"];
      var spotsToRemove = new List<BsonValue>();
      foreach (var criticalSpot in criticalSpots)
      {
         if (criticalSpot is not null)
         {
            var criticalSpotType = criticalSpot["CriticalSpotType"];
            if (criticalSpotType is null)
            {
               spotsToRemove.Add(criticalSpot);
               continue;
            }
            criticalSpot.AsBsonDocument.Add("@Type", criticalSpotType["@Type"]);
            criticalSpot.AsBsonDocument.Remove("CriticalSpotType");
         }
      }

      foreach (var spot in spotsToRemove)
      {
         criticalSpots.Remove(spot);
      }
   }
}