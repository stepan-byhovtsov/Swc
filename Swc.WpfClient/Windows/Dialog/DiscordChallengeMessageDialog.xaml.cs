using System.Windows;
using System.Windows.Input;

namespace Swc.WpfClient.Windows.Dialog;

public partial class DiscordChallengeMessageDialog : Window
{
   public DiscordChallengeMessageDialog()
   {
      InitializeComponent();
   }

   private void Accepted(object sender, RoutedEventArgs e)
   {
      DialogResult = true;
   }

   public string CategoryName => CategoryNameTextBox.Text;
   public string Unit => UnitTextBox.Text;

   private void Exit(object sender, ExecutedRoutedEventArgs e)
   {
      Close();
   }
}