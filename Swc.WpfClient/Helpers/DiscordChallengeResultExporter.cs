using Swc.Template;

namespace Swc.WpfClient;

public class DiscordChallengeResultExporter
{
   public static string ExportToDiscordMessage(DiscordChallengeResultParameters parameters)
   {
      var message = $"""
                     # {parameters.CategoryName}
                     :first_place: **{parameters.Objects[0].obj.Name}** by @{parameters.Objects[0].obj.AuthorInfos[0].AuthorLinks.DiscordId}: **{parameters.Objects[0].value}** {parameters.ValueUnit}
                     :second_place: **{parameters.Objects[1].obj.Name}** by @{parameters.Objects[1].obj.AuthorInfos[0].AuthorLinks.DiscordId}: **{parameters.Objects[1].value}** {parameters.ValueUnit}
                     :third_place: **{parameters.Objects[2].obj.Name}** by @{parameters.Objects[2].obj.AuthorInfos[0].AuthorLinks.DiscordId}: **{parameters.Objects[2].value}** {parameters.ValueUnit}
                     """;

      return message;
   }
}

public class DiscordChallengeResultParameters((SwcObject, string)[] objects)
{
   public (SwcObject obj, string value)[] Objects { get; set; } = objects;
   public string ValueUnit { get; set; } = "";
   public string CategoryName { get; set; } = "";
}