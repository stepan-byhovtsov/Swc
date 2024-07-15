namespace Swc.Template;

public class AuthorInfo
{
   [Singleline] public string Author { get; set; } = "";
   [Singleline] public AuthorLinks AuthorLinks { get; set; } = new();
}
