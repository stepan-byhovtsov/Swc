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

   public AuthorInfo AuthorInfo { get; set; } = new();
   public CreationType? Type { get; set; }

   [NonEditable] public string File { get; set; } = "";
}
