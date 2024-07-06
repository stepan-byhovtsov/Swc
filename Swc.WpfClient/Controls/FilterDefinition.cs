using System.Windows;
using MongoDB.Bson;
using MongoDB.Driver;
using Swc.Core.Helpers;
using Swc.Template;

namespace Swc.WpfClient.Controls;

public class FilterDefinition : DependencyObject
{
   public static readonly DependencyProperty PropertyProperty =
      DependencyProperty.Register(nameof(Property), typeof(string), typeof(FilterDefinition));

   public static readonly DependencyProperty FilterProperty =
      DependencyProperty.Register(nameof(Filter), typeof(FilterOperation), typeof(FilterDefinition));
   
   public static readonly DependencyProperty ArgumentProperty =
      DependencyProperty.Register(nameof(Argument), typeof(string), typeof(FilterDefinition));

   public string[] PropertyNames { get; }

   public FilterOperation[] Filters { get; } =
   {
      new("exists", false, (query, left, _) =>
      {
         query.Filters.Add(new JsonFilterDefinition<BsonDocument>($"{{\"{left}\":{{$exists:true}}}}"));
      }),
      new("is greater than", true, (query, left, right) =>
      {
         query.Filters.Add(new JsonFilterDefinition<BsonDocument>($"{{\"{left}\":{{$gt:{right}}}}}"));
      }),
      new("is less than", true, (query, left, right) =>
      {
         query.Filters.Add(new JsonFilterDefinition<BsonDocument>($"{{\"{left}\":{{$lt:{right}}}}}"));
      }),
      new("is equals to", true, (query, left, right) =>
      {
         query.Filters.Add(new JsonFilterDefinition<BsonDocument>($"{{\"{left}\":{{$eq:{right}}}}}"));
      }),
      new("is greater or equals to", true, (query, left, right) =>
      {
         query.Filters.Add(new JsonFilterDefinition<BsonDocument>($"{{\"{left}\":{{$gte:{right}}}}}"));
      }),
      new("is less or equals to", true, (query, left, right) =>
      {
         query.Filters.Add(new JsonFilterDefinition<BsonDocument>($"{{\"{left}\":{{$lte:{right}}}}}"));
      }),
      new("is not equals to", true, (query, left, right) =>
      {
         query.Filters.Add(new JsonFilterDefinition<BsonDocument>($"{{\"{left}\":{{$ne:{right}}}}}"));
      }),
      new("sort with coefficient", true, (query, left, right) =>
      {
         // TODO
      }),
    };

   private int _selectedProperty = -1;
   private int _selectedFilter = -1;

   public FilterDefinition()
   {
      var rootType = typeof(SwcObject);
      var list = new List<string>();
      AddAllPropertiesOf(rootType, list);
      PropertyNames = list.ToArray();
   }

   private void AddAllPropertiesOf(Type type, List<string> list, string prefix = "")
   {
      foreach (var property in type.GetSerializedProperties())
      {
         AddProperty(property.PropertyType, list, prefix + property.Name);
      }
   }

   private void AddProperty(Type type, List<string> list, string prefix)
   {
      string name1 = prefix;
      if (type.IsAbstract)
      {
         foreach (var nestedType in type.GetNestedTypes())
         {
            var name2 = $"{name1}({nestedType.Name})";
            list.Add(name2);
            AddProperty(nestedType, list, name2);
         }

         return;
      }

      if (type.IsArray)
      {
         list.Add($"{name1}#Count");
         var name2 = $"{name1}[Any]";
         var elementType = type.GetElementType()!;
         AddProperty(elementType, list, name2);
         return;
      }

      if (!ObjectPresentation.IsSimpleType(type))
      {
         AddAllPropertiesOf(type, list, name1 + ".");
         return;
      }

      list.Add(name1);
   }

   public int SelectedProperty
   {
      get => _selectedProperty;
      set
      {
         if (SelectedProperty != value)
         { 
            Property = PropertyNames[value];
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
            Filter = Filters[value];
         }

         _selectedFilter = value;
      }
   }

   public string? Property
   {
      get => (string) GetValue(PropertyProperty);
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
