using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Swc.Template;

namespace Swc.WpfClient.Controls;

public class FilterOperation
{
   public FilterOperation(string verb, bool requiresSecondOperand, QueryAction action)
   {
      Verb = verb;
      RequiresSecondOperand = requiresSecondOperand;
      Action = action;
   }

   public string Verb { get; }
   public bool RequiresSecondOperand { get; }

   public delegate void QueryAction(FilterQuery query, string leftOperand, string rightOperand);

   public QueryAction Action { get; }


   public override string ToString()
   {
      return Verb;
   }

   public void Apply(FilterQuery query, string leftOperand, string rightOperand)
   {
      var comment1 = new Regex(@"\[([^\.]*)\]");
      var comment2 = new Regex(@"\(([^\.]*)\)");
      leftOperand = comment1.Replace(leftOperand, "");
      
      leftOperand = comment2.Replace(leftOperand, "");
      if (leftOperand.EndsWith("#Count"))
      {
         // TODO
      }
      
      Action(query, leftOperand, rightOperand);
   }
}

public class FilterQuery
{
   public List<FilterDefinition<BsonDocument>> Filters { get; } = new();
   public List<SortDefinition<BsonDocument>> Sorts { get; } = new();
}
