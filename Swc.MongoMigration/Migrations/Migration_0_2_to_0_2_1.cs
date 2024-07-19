using MongoDB.Bson;
using MongoDB.Driver;
using Swc.MongoMigrationTemplate.Tools;

namespace Swc.MongoMigrationTemplate.Migrations;

public sealed class Migration_0_2_to_0_2_1 : Migration
{
   public override string FromVersion => "0.2";
   public override string ToVersion => "0.2.1";

   protected override void ApplyInternal(BsonDocument old)
   {
      old.Add("AuthorInfos", new BsonArray() { old["AuthorInfo" ]});
      old.Remove("AuthorInfo");
   }
}