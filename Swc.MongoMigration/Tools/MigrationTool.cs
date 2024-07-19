using System.Reflection;
using MongoDB.Bson;

namespace Swc.MongoMigrationTemplate.Tools;

public class MigrationTool
{
   private static readonly Dictionary<string, Migration> Migrations = new();

   static MigrationTool()
   {
      foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsSubclassOf(typeof(Migration))))
      {
         var migration = (Migration) type.GetConstructor([])!.Invoke([]);
         Migrations[migration.FromVersion] = migration;
      }
   }
   
   public static BsonDocument Migrate(BsonDocument document, string targetVersion)
   {
      while (document["TemplateVersion"].AsString != targetVersion)
      {
         var currentVersion = document["TemplateVersion"].AsString!;
         if (Migrations.TryGetValue(currentVersion, out var migration))
         {
            migration.Apply(document);
         }
         else
         {
            return document;
         }
      }

      return document;
   }
}