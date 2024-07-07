using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Swc.Template;

namespace Swc.WpfClient.Controls;

public class FilterOperation(string verb, bool requiresSecondOperand, FilterOperation.QueryAction action)
{
   public string Verb { get; } = verb;
   public bool RequiresSecondOperand { get; } = requiresSecondOperand;

   public delegate void QueryAction(FilterQuery query, string leftOperand, string rightOperand, QueryArguments args);

   public QueryAction Action { get; } = action;


   public override string ToString()
   {
      return Verb;
   }

   public void Apply(FilterQuery query, string leftOperand, string rightOperand)
   {
      var args = new QueryArguments();
      
      var comment1 = new Regex(@"\[([^\.]*)\]");
      var comment2 = new Regex(@"\(([^\.]*)\)");
      leftOperand = comment1.Replace(leftOperand, "");
      
      if (leftOperand.EndsWith(')'))
      {
         var substring = leftOperand[(leftOperand.LastIndexOf('(')+1) .. ^1];
         args.ShouldSpecifyType = true;
         args.SpecificType = substring;
      }
      
      leftOperand = comment2.Replace(leftOperand, "");
      if (leftOperand.EndsWith("#Count"))
      {
         args.ShouldWorkWithArrayLength = true;
         leftOperand = leftOperand[..^"#Count".Length];
      }
      
      Action(query, leftOperand, rightOperand, args);
   }
}

public class FilterQuery
{
   public List<string> Filters { get; } = new();
   public List<JsonPipelineStageDefinition<BsonDocument, BsonDocument>> Sorts { get; } = new();
}

public class QueryArguments
{
   /// <summary>
   /// Specifies, whether we should work not with an array object, but with its length
   /// </summary>
   public bool ShouldWorkWithArrayLength { get; set; }
   
   public bool ShouldSpecifyType { get; set; }
   public string? SpecificType { get; set; }
}