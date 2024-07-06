using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace Swc.WpfClient.Controls;

public class ToStringConverter : IValueConverter
{
   public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
   {
      if (value == null)
         return "[null]";

      if (value is string or float or int)
         return Clip(value.ToString()!);

      if (value is Vector3 vec3)
         return$"({vec3.X}; {vec3.Y}; {vec3.Z})";
      
      if (value is Vector2 vec2)
         return$"({vec2.X}; {vec2.Y})";
      
      return value.GetType().Name;
   }

   private const int ClipSize = 30;

   private static string Clip(string s)
   {
      s = s.ReplaceLineEndings("//");
      
      if (s.Length > ClipSize)
         return string.Concat(s.AsSpan(0, ClipSize), " ...");
      
      return s;
   }

   public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
   {
      throw new InvalidOperationException();
   }
}
