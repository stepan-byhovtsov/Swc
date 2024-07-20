using System.Numerics;
using System.Text;
using System.Windows;
using MongoDB.Bson;
using MongoDB.Driver;
using Swc.Core.Helpers;
using Swc.Template;

namespace Swc.WpfClient.Controls;

public class FilterDefinition : DependencyObject
{
   public static readonly DependencyProperty PropertyProperty =
      DependencyProperty.Register(nameof(Property), typeof(Query), typeof(FilterDefinition));

   public static readonly DependencyProperty FilterProperty =
      DependencyProperty.Register(nameof(Filter), typeof(FilterOperation), typeof(FilterDefinition));

   public static readonly DependencyProperty ArgumentProperty =
      DependencyProperty.Register(nameof(Argument), typeof(string), typeof(FilterDefinition));

   public Query[] Properties { get; }

   #region Filters

   private static void ExistsFilter(FilterQuery query, Query left, string _)
   {
      var filter = new StringBuilder();

      filter.Append('{');

      var currentProperty = new StringBuilder();
      var closingBraces = 1;
      foreach (var elem in left.Elements)
      {
         switch (elem)
         {
            case QueryTypeElement type:
               filter.AppendAll('"', currentProperty, ".@Type", '"', ':', '"', type.TypeName, '"', ',');
               break;
            case QueryArrayAnyElement:
               filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
               currentProperty.Clear();
               closingBraces += 2;
               break;
            case QueryArrayTypeElement arrayType:
               filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
               filter.AppendAll('"', "@Type", '"', ':', '"', arrayType.TypeName, '"', ',');
               currentProperty.Clear();
               closingBraces += 2;
               break;
            case QueryPropertyElement element:
               if (currentProperty.Length > 0)
                  currentProperty.Append('.');
               currentProperty.AppendAll(element.PropertyName);
               break;
         }
      }

      for (int i = 0; i < closingBraces; i++) filter.Append('}');

      var @string = filter.ToString();
      query.Filters.Add(@string);
   }

   public FilterOperation[] Filters { get; } =
   [
      new("exists", false, ExistsFilter),
      new("is greater than", true, (query, left, right) =>
      {
         var filter = new StringBuilder();

         filter.Append('{');

         var currentProperty = new StringBuilder();
         var closingBraces = 1;

         bool handled = false;

         foreach (var elem in left.Elements)
         {
            switch (elem)
            {
               case QueryTypeElement type:
                  filter.AppendAll('"', currentProperty, ".@Type", '"', ':', '"', type.TypeName, '"', ',');
                  break;
               case QueryArrayLengthElement:
                  filter.AppendAll('"', currentProperty, '.', right, '"', ':', "{$exists:true}");
                  currentProperty.Clear();
                  handled = true;
                  break;
               case QueryArrayAnyElement:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryArrayTypeElement arrayType:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  filter.AppendAll('"', "@Type", '"', ':', '"', arrayType.TypeName, '"', ',');
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryPropertyElement element:
                  if (currentProperty.Length > 0)
                     currentProperty.Append('.');
                  currentProperty.Append(element.PropertyName);
                  break;
            }

            if (handled)
               break;
         }

         if (!handled)
         {
            filter.AppendAll('"', currentProperty, '"', ':', "{$gt:", right, '}', ',');
         }

         for (int i = 0; i < closingBraces; i++) filter.Append('}');

         var @string = filter.ToString();
         query.Filters.Add(@string);
      }),
      new("is less than", true, (query, left, right) =>
      {
         var filter = new StringBuilder();

         filter.Append('{');

         var currentProperty = new StringBuilder();
         var closingBraces = 1;

         bool handled = false;

         foreach (var elem in left.Elements)
         {
            switch (elem)
            {
               case QueryTypeElement type:
                  filter.AppendAll('"', currentProperty, ".@Type", '"', ':', '"', type.TypeName, '"', ',');
                  break;
               case QueryArrayLengthElement:
                  filter.AppendAll('"', currentProperty, '.', int.Parse(right) - 1, '"', ':', "{$exists:false}");
                  currentProperty.Clear();
                  handled = true;
                  break;
               case QueryArrayAnyElement:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryArrayTypeElement arrayType:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  filter.AppendAll('"', "@Type", '"', ':', '"', arrayType.TypeName, '"', ',');
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryPropertyElement element:
                  if (currentProperty.Length > 0)
                     currentProperty.Append('.');
                  currentProperty.Append(element.PropertyName);
                  break;
            }

            if (handled)
               break;
         }

         if (!handled)
         {
            filter.AppendAll('"', currentProperty, '"', ':', "{$lt:", right, '}', ',');
         }

         for (int i = 0; i < closingBraces; i++) filter.Append('}');

         var @string = filter.ToString();
         query.Filters.Add(@string);
      }),
      new("is equals to", true, (query, left, right) =>
      {
         var filter = new StringBuilder();

         filter.Append('{');

         var currentProperty = new StringBuilder();
         var closingBraces = 1;

         bool handled = false;

         foreach (var elem in left.Elements)
         {
            switch (elem)
            {
               case QueryTypeElement type:
                  filter.AppendAll('"', currentProperty, ".@Type", '"', ':', '"', type.TypeName, '"', ',');
                  break;
               case QueryArrayLengthElement:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$size:", right, '}');
                  currentProperty.Clear();
                  handled = true;
                  break;
               case QueryArrayAnyElement:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryArrayTypeElement arrayType:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  filter.AppendAll('"', "@Type", '"', ':', '"', arrayType.TypeName, '"', ',');
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryPropertyElement element:
                  if (currentProperty.Length > 0)
                     currentProperty.Append('.');
                  currentProperty.Append(element.PropertyName);
                  break;
            }

            if (handled)
               break;
         }

         if (!handled)
         {
            filter.AppendAll('"', currentProperty, '"', ':', "{$eq:", right, '}', ',');
         }

         for (int i = 0; i < closingBraces; i++) filter.Append('}');

         var @string = filter.ToString();
         query.Filters.Add(@string);
      }),
      new("is greater or equals to", true, (query, left, right) =>
      {
         var filter = new StringBuilder();

         filter.Append('{');

         var currentProperty = new StringBuilder();
         var closingBraces = 1;

         bool handled = false;

         foreach (var elem in left.Elements)
         {
            switch (elem)
            {
               case QueryTypeElement type:
                  filter.AppendAll('"', currentProperty, ".@Type", '"', ':', '"', type.TypeName, '"', ',');
                  break;
               case QueryArrayLengthElement:
                  filter.AppendAll('"', currentProperty, '.', int.Parse(right) - 1, '"', ':', "{$exists:true}");
                  currentProperty.Clear();
                  handled = true;
                  break;
               case QueryArrayAnyElement:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryArrayTypeElement arrayType:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  filter.AppendAll('"', "@Type", '"', ':', '"', arrayType.TypeName, '"', ',');
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryPropertyElement element:
                  if (currentProperty.Length > 0)
                     currentProperty.Append('.');
                  currentProperty.Append(element.PropertyName);
                  break;
            }

            if (handled)
               break;
         }

         if (!handled)
         {
            filter.AppendAll('"', currentProperty, '"', ':', "{$gte:", right, '}', ',');
         }

         for (int i = 0; i < closingBraces; i++) filter.Append('}');

         var @string = filter.ToString();
         query.Filters.Add(@string);
      }),
      new("is less or equals to", true, (query, left, right) =>
      {
         var filter = new StringBuilder();

         filter.Append('{');

         var currentProperty = new StringBuilder();
         var closingBraces = 1;

         bool handled = false;

         foreach (var elem in left.Elements)
         {
            switch (elem)
            {
               case QueryTypeElement type:
                  filter.AppendAll('"', currentProperty, ".@Type", '"', ':', '"', type.TypeName, '"', ',');
                  break;
               case QueryArrayLengthElement:
                  filter.AppendAll('"', currentProperty, '.', int.Parse(right), '"', ':', "{$exists:false}");
                  currentProperty.Clear();
                  handled = true;
                  break;
               case QueryArrayAnyElement:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryArrayTypeElement arrayType:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  filter.AppendAll('"', "@Type", '"', ':', '"', arrayType.TypeName, '"', ',');
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryPropertyElement element:
                  if (currentProperty.Length > 0)
                     currentProperty.Append('.');
                  currentProperty.Append(element.PropertyName);
                  break;
            }

            if (handled)
               break;
         }

         if (!handled)
         {
            filter.AppendAll('"', currentProperty, '"', ':', "{$lte:", right, '}', ',');
         }

         for (int i = 0; i < closingBraces; i++) filter.Append('}');

         var @string = filter.ToString();
         query.Filters.Add(@string);
      }),
      new("is not equals to", true, (query, left, right) =>
      {
         var filter = new StringBuilder();

         filter.Append('{');

         var currentProperty = new StringBuilder();
         var closingBraces = 1;

         bool handled = false;

         foreach (var elem in left.Elements)
         {
            switch (elem)
            {
               case QueryTypeElement type:
                  filter.AppendAll('"', currentProperty, ".@Type", '"', ':', '"', type.TypeName, '"', ',');
                  break;
               case QueryArrayLengthElement:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$not:{$size:", right, "}}");
                  currentProperty.Clear();
                  handled = true;
                  break;
               case QueryArrayAnyElement:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryArrayTypeElement arrayType:
                  filter.AppendAll('"', currentProperty, '"', ':', "{$elemMatch:{");
                  filter.AppendAll('"', "@Type", '"', ':', '"', arrayType.TypeName, '"', ',');
                  currentProperty.Clear();
                  closingBraces += 2;
                  break;
               case QueryPropertyElement element:
                  if (currentProperty.Length > 0)
                     currentProperty.Append('.');
                  currentProperty.Append(element.PropertyName);
                  break;
            }

            if (handled)
               break;
         }

         if (!handled)
         {
            filter.AppendAll('"', currentProperty, '"', ':', "{$ne:", right, '}', ',');
         }

         for (int i = 0; i < closingBraces; i++) filter.Append('}');

         var @string = filter.ToString();
         query.Filters.Add(@string);
      }),
      new("sort with coefficient", true, (query, left, right) =>
      {
         ExistsFilter(query, left, right);

         var filter = new StringBuilder();

         var currentProperty = new StringBuilder();
         var braces = new Stack<char>();

         bool handled = false;

         filter.AppendAll("{$set:{\"priority\":{$sum:[\"$priority\",{$multiply:[", right, ",");
         braces.PushAll('{', '{', '{', '[', '{', '[');

         StringBuilder obj = new();

         for (var i = 0; i < left.Elements.Count; i++)
         {
            var elem = left.Elements[i];
            switch (elem)
            {
               case QueryPropertyElement propElem:
                  if (i == 0)
                  {
                     obj.AppendAll("'$", propElem.PropertyName, "'");
                  }
                  else
                  {
                     obj = new StringBuilder().AppendAll("{$getField:{input:", obj, ",field:'", propElem.PropertyName,
                        "'}}");
                  }

                  break;
               case QueryArrayTypeElement typeElem:
                  obj = new StringBuilder().AppendAll("{$arrayElemAt:[{$filter:{input:", obj,
                     ",as:'e',cond:{$eq:['$$e.@Type','", typeElem.TypeName, "']}}},0]}");
                  break;
               case QueryArrayLengthElement:
                  obj = new StringBuilder().AppendAll("{$size:", obj, "}");
                  break;
            }
         }

         filter.Append(obj);

         while (braces.Count > 0) filter.Append(braces.Pop().ClosingBrace());

         var @string = filter.ToString();
         query.Sorts.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(@string));
      })
   ];

   #endregion

   private int _selectedProperty = -1;
   private int _selectedFilter = -1;

   public FilterDefinition()
   {
      var rootType = typeof(SwcObject);
      var list = new List<Query>();
      AddAllPropertiesOf(rootType, list, new Query());
      Properties = list.ToArray();
   }

   private void AddAllPropertiesOf(Type type, List<Query> list, Query query)
   {
      foreach (var property in type.GetShownProperties())
      {
         var propertyType = property.PropertyType;
         AddProperty(property.Name, propertyType, list, query);
      }
   }

   private void AddProperty(string propertyName, Type propertyType, List<Query> list, Query query)
   {
      if (propertyType.IsAbstract)
      {
         var newQuery = query.BranchAdd(new QueryPropertyElement(propertyName));
         AddAllPropertiesOf(propertyType, list, newQuery);
         foreach (var nestedType in propertyType.GetNestedTypes())
         {
            list.Add(newQuery.BranchAdd(new QueryTypeElement(nestedType.Name)));
            AddAllPropertiesOf(nestedType, list, newQuery.BranchAdd(new QueryTypeElement(nestedType.Name)));
         }

         return;
      }

      if (propertyType.IsArray)
      {
         var arrayQuery = query.BranchAdd(new QueryPropertyElement(propertyName));
         list.Add(arrayQuery.BranchAdd(new QueryArrayLengthElement()));

         var elementType = propertyType.GetElementType()!;
         if (elementType.IsAbstract)
         {
            foreach (var nestedType in elementType.GetNestedTypes())
            {
               var newQuery = arrayQuery.BranchAdd(new QueryArrayTypeElement(nestedType.Name));
               list.Add(newQuery);
               AddAllPropertiesOf(nestedType, list, newQuery);
            }
         }

         var firstQuery = arrayQuery.BranchAdd(new QueryArrayIndexElement(0));
         list.Add(firstQuery);
         AddAllPropertiesOf(elementType, list, firstQuery);

         var anyQuery = arrayQuery.BranchAdd(new QueryArrayAnyElement());
         list.Add(anyQuery);
         AddAllPropertiesOf(elementType, list, anyQuery);

         var allQuery = arrayQuery.BranchAdd(new QueryArrayAllElement());
         list.Add(allQuery);
         AddAllPropertiesOf(elementType, list, allQuery);

         return;
      }

      if (propertyType == typeof(Vector2))
      {
         list.Add(query.BranchAdd(new QueryPropertyElement(propertyName)).Add(new QueryPropertyElement("X")));
         list.Add(query.BranchAdd(new QueryPropertyElement(propertyName)).Add(new QueryPropertyElement("Y")));
         return;
      }

      if (propertyType == typeof(Vector3))
      {
         list.Add(query.BranchAdd(new QueryPropertyElement(propertyName)).Add(new QueryPropertyElement("X")));
         list.Add(query.BranchAdd(new QueryPropertyElement(propertyName)).Add(new QueryPropertyElement("Y")));
         list.Add(query.BranchAdd(new QueryPropertyElement(propertyName)).Add(new QueryPropertyElement("Z")));
         return;
      }

      if (ObjectPresentation.IsSimpleType(propertyType))
      {
         list.Add(query.BranchAdd(new QueryPropertyElement(propertyName)));
         return;
      }

      AddAllPropertiesOf(propertyType, list, query.BranchAdd(new QueryPropertyElement(propertyName)));
   }

   public int SelectedProperty
   {
      get => _selectedProperty;
      set
      {
         if (SelectedProperty != value)
         {
            Property = value == -1 ? null : Properties[value];
         }

         _selectedProperty = value;
      }
   }

   public int SelectedFilter
   {
      get => _selectedFilter;
      set
      {
         if (_selectedFilter != value)
         {
            Filter = value == -1 ? null : Filters[value];
         }

         _selectedFilter = value;
      }
   }

   public Query? Property
   {
      get => (Query) GetValue(PropertyProperty);
      set => SetValue(PropertyProperty, value);
   }

   public FilterOperation? Filter
   {
      get => (FilterOperation) GetValue(FilterProperty);
      set => SetValue(FilterProperty, value);
   }

   public string Argument
   {
      get => (string) GetValue(ArgumentProperty);
      set => SetValue(ArgumentProperty, value);
   }
}