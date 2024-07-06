using System.Xml;

namespace Swc.Core.SavesModification.Vehicles;

public class BodySave
{ 
   public XmlElement Element { get; private set; }

   public int Id
   {
      get => int.Parse(Element.Attributes["unique_id"]!.Value);
      set => Element.Attributes["unique_id"]!.Value = value.ToString();
   }
   
   public BodySave(XmlElement element)
   {
      Element = element;
   }
}
