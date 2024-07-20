using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MongoDB.Bson;
using MongoDB.Driver;
using Swc.MongoMigrationTemplate.Tools;
using Swc.Template;
using Swc.WpfClient.Controls;
using Swc.WpfClient.Windows.Dialog;

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

#if !DEBUG
      try
      {
#endif
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
            Objects.Add(MongoDb.FromBson(MigrationTool.Migrate(swcObject!, new SwcObject().TemplateVersion)!));
         }
         
#if !DEBUG
      }
      catch (Exception exc)
      {
        MessageBox.Show(this, $"Invalid filters:\n{exc.Message}", "Error", MessageBoxButton.OK);
      }
#endif
   }

   private void ExitWindow(object sender, ExecutedRoutedEventArgs e)
   {
      Hide();
   }

   private void Selected(object sender, SelectionChangedEventArgs e)
   {
      if (e.AddedItems.Count > 0)
      {
         MainWindow.Inspector.Object = (SwcObject) e.AddedItems[0]!;
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

   private void ExportDiscordChallengeMessage(object sender, ExecutedRoutedEventArgs e)
   {
      DiscordChallengeMessageDialog dialog = new();
      if (dialog.ShowDialog() == true)
      {
         var propertyPaths = Filters.Where(c => c.SelectedFilter == 7).Select(c => c.Property).ToArray();
      
         string GenerateValue(SwcObject obj)
         {
            StringBuilder str = new();
            foreach (var path in propertyPaths)
            {
               str.Append(path!.GetValue(obj));
               str.Append('x');
            }
      
            if (str.Length > 0)
               str.Remove(str.Length - 1, 1);
      
            return str.ToString();
         }
         
         var objects = new (SwcObject, string)[Objects.Count];
         for (var i = 0; i < objects.Length; i++)
         {
            objects[i] = (Objects[i], GenerateValue(Objects[i]));
         }
         
         var message = DiscordChallengeResultExporter.ExportToDiscordMessage(
            new DiscordChallengeResultParameters(objects)
            {
               ValueUnit = dialog.Unit,
               CategoryName = dialog.CategoryName
            });
         Clipboard.SetText(message);
      }
   }
}