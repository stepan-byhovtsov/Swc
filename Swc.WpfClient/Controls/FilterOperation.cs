using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Swc.Core.Helpers;
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
      (var args, leftOperand, rightOperand) = PropertyPath.SimplifyPath(leftOperand, rightOperand);
      Action(query, leftOperand, rightOperand, args);
   }
}

public class FilterQuery
{
   public List<string> Filters { get; } = new();
   public List<JsonPipelineStageDefinition<BsonDocument, BsonDocument>> Sorts { get; } = new();
}