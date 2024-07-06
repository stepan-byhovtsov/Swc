using System.Globalization;
using System.Windows.Data;

namespace Swc.WpfClient.Controls;

public class FloatToString : IValueConverter
{
   public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
   {
      return ((float) value).ToString("F2", CultureInfo.InvariantCulture);
   }

   public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
   {
      var str = (string) value;
      str = str.Trim(' ');
      if (str.Length == 0)
         return 0;
      return float.Parse(str, CultureInfo.InvariantCulture);
   }
}

public class IntToString : IValueConverter
{
   public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
   {
      return ((int) value).ToString(CultureInfo.InvariantCulture);
   }

   public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
   {
      var str = (string) value;
      str = str.Trim(' ');
      if (str.Length == 0)
         return 0;
      return int.Parse((string) value);
   }
}
