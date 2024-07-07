using System.Collections.ObjectModel;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using DnsClient;
using MongoDB.Bson;
using MongoDB.Driver;
using Swc.Core.Serialization.Json;
using Swc.Template;
using Swc.WpfClient.Controls;

namespace Swc.WpfClient.Windows;

public partial class DatabaseObserverWindow : Window
{
   public ObservableCollection<SwcObject> Objects { get; } = new();
   public ObservableCollection<FilterDefinition> Filters { get; } = new();

   public MainWindow MainWindow { get; }
   public MongoDbHelper MongoDb => MainWindow.MongoDb;

   public DatabaseObserverWindow(MainWindow mainWindow)
   {
      InitializeComponent();
      MainWindow = mainWindow;
      Owner = mainWindow;
   }

   private void Refresh(object sender, ExecutedRoutedEventArgs? e = null)
   {
      Refresh();
   }

   public void Refresh()
   {
      Objects.Clear();

      try
      {
         FilterQuery query = new();
         foreach (var filter in Filters)
         {
            filter.Filter?.Apply(query, filter.Property!, filter.Argument);
         }

         List<JsonPipelineStageDefinition<BsonDocument, BsonDocument>> stages = [];
         
         stages.AddRange(query.Filters.Select(filterDefinition => 
            new JsonPipelineStageDefinition<BsonDocument, BsonDocument>($"{{$match:{filterDefinition}}}")));
         stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$set:{\"priority\":0}}"));
         stages.AddRange(query.Sorts);
         stages.Add(new JsonPipelineStageDefinition<BsonDocument, BsonDocument>("{$sort:{\"priority\":1}}"));

         var cursor =
            MongoDb.Objects.Aggregate(new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stages));
         foreach (var swcObject in cursor.ToEnumerable())
         {
            Objects.Add(MongoDb.FromBson(swcObject));
         }
      }
      catch (Exception exc)
      {
         MessageBox.Show(this, $"Invalid filters:\n{exc.Message}", "Error", MessageBoxButton.OK);
      }
   }

   private void ExitWindow(object sender, ExecutedRoutedEventArgs e)
   {
      Hide();
   }

   private void Selected(object sender, SelectionChangedEventArgs e)
   {
      if (e.AddedItems.Count > 0)
      {
         MainWindow.Inspector.Creation = (SwcObject) e.AddedItems[0]!;
         Hide();
      }
   }

   private void AddFilter(object sender, ExecutedRoutedEventArgs e)
   {
      Filters.Add(new FilterDefinition());
   }

   private void DeleteSelectedFilters(object sender, ExecutedRoutedEventArgs e)
   {
      while (FilterList.SelectedItems.Count > 0)
      {
         Filters.Remove((FilterDefinition) FilterList.SelectedItems[0]!);
      }
   }
}