using System.Xml;
using Swc.Core.Helpers;

namespace Swc.Core.SavesModification.Vehicles;

public class VehicleSave
{
   public XmlElement Element { get; private set; }
   
   public int BodiesId
   {
      get => int.Parse(Element.Attributes["bodies_id"]!.Value);
      set => Element.Attributes["bodies_id"]!.Value = value.ToString();
   }

   public BodySave[] BodySaves
   {
      get => Element["bodies"]!.ChildNodes!.Select(c => new BodySave((XmlElement) c)).ToArray();
      set
      {
         int bodiesId = value.MaxBy(c => c.Id)!.Id;
         BodiesId = bodiesId;
         
         Element["bodies"]!.RemoveAll();
         foreach (var body in value)
         {
            Element["bodies"]!.AppendChild(Element.OwnerDocument.ImportNode(body.Element, true));
         }
      }
   }
   
   public VehicleSave(XmlElement element)
   {
      Element = element;
   }
}
