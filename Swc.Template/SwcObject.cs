namespace Swc.Template;

/*
 * Currently supported types:
 * string,
 * float,
 * int,
 * Vector2,
 * Vector3,
 * (bool),
 * arrays,
 * objects,
 * "enums"
 */

public class SwcObject
{
   [NonEditable]
   [MongoDB.Bson.Serialization.Attributes.BsonId]
   public MongoDB.Bson.ObjectId? Id { get; set; }

   [Singleline] public string Name { get; set; } = "";
   [Multiline] public string Description { get; set; } = "";
   
   public AuthorInfo[] AuthorInfos { get; set; } = [];
   public CreationType? Type { get; set; }

   [NonEditable] public string File { get; set; } = "";
   public string TemplateVersion { get; } = "0.2.1";
   
   // ReSharper disable once InconsistentNaming
   [NonEditable] [NonSerializable] public float priority { get; set; }

}
