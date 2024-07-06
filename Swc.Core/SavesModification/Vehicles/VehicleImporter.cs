using System.Xml;

namespace Swc.Core.SavesModification.Vehicles;

public class VehicleImporter
{
   public VehicleSave ImportFromString(string text)
   {
      var document = new XmlDocument();
      document.Load(new StringReader(text));
      return new VehicleSave(document.DocumentElement!);
   }

   public string ExportToString(VehicleSave save)
   {
      var stringWriter = new StringWriter();
      var xmlWriter = new XmlTextWriter(stringWriter);
      save.Element.WriteTo(xmlWriter);
      return stringWriter.ToString();
   }
}
