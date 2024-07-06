namespace Swc.Template;

public class AuthorInfo
{
   [Singleline] public string Author { get; set; } = "";
   [Singleline] public string[] AuthorLinks { get; set; } = Array.Empty<string>();
}
