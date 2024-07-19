using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Swc.MongoMigrationTemplate.Tools;

public abstract class Migration
{
   public abstract string FromVersion { get; }
   public abstract string ToVersion { get; }
   protected abstract void ApplyInternal(BsonDocument old);

   public void Apply(BsonDocument old)
   {
      Debug.Assert(old["TemplateVersion"].AsString == FromVersion);
      ApplyInternal(old);
      old["TemplateVersion"] = ToVersion;
   }
}