using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Controls;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Swc.Core.Serialization.Json;
using Swc.Template;

namespace Swc.WpfClient;

public class MongoDbHelper
{
   private IMongoClient Client { get; }
   private IMongoDatabase Database { get; }
   public IMongoCollection<BsonDocument> Objects { get; }
   private IGridFSBucket<ObjectId> GridFs { get; }
   
   public MongoDbHelper()
   {
      Client = ConnectToDb();

      Database = Client.GetDatabase("Swc");
      
      GridFs = new GridFSBucket<ObjectId>(Database);

      Objects = Database.GetCollection<BsonDocument>("Objects");
   }

   public async Task SaveToDb(SwcObject obj)
   {
      obj.Id ??= ObjectId.GenerateNewId();
      await Objects.ReplaceOneAsync(json => json["_id"] == obj.Id, ToBson(obj), new ReplaceOptions {IsUpsert = true});
   }

   public BsonDocument ToBson(SwcObject obj)
   {
      var doc = BsonDocument.Parse(SwcJsonSerializer.SerializeToJsonNode<SwcObject>(obj).ToJsonString());
      doc["_id"] = obj.Id;
      doc.Remove("File");
      doc.Remove("Id");
      return doc;
   }
   
   public SwcObject FromBson(BsonDocument bson)
   {
      bson["Id"] = bson["_id"].ToString();
      bson.Remove("_id");
      return SwcJsonSerializer.Deserialize<SwcObject>(JsonNode.Parse(bson.ToJson()));
   }

   public async Task UploadFile(SwcObject obj)
   {
      if ((await GridFs.FindAsync(new ExpressionFilterDefinition<GridFSFileInfo<ObjectId>>(c => c.Id == obj.Id))).ToList().Count > 0)
      {
         await GridFs.DeleteAsync(obj.Id!.Value);
      }
      await GridFs.UploadFromBytesAsync(obj.Id!.Value, obj.Name, Encoding.UTF8.GetBytes(obj.File));
      await SaveToDb(obj);
   }
   
   public async Task DownloadFile(SwcObject obj)
   {
      var bytes = await GridFs.DownloadAsBytesAsync(obj.Id!.Value);
      if (bytes is null)
      {
         throw new InvalidOperationException("Object's file isn't presented in db");
      }
      obj.File = Encoding.UTF8.GetString(bytes);
   }

   private IMongoClient ConnectToDb()
   {
      var settings = Properties.Settings.Default;
      return new MongoClient($"mongodb://{settings.DatabaseUser}:{settings.DatabasePwd}@{settings.DatabaseIp}:27017/Swc");
   }
}
