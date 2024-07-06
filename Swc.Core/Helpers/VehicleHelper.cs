using System.Numerics;
using System.Xml;
using Swc.Core.SavesModification.Vehicles;

namespace Swc.Core.Helpers;

public static class VehicleHelper
{
   public static void AddBodiesFrom(this VehicleSave v, VehicleSave other)
   {
      var s1 = v.BodySaves;
      var s2 = other.BodySaves;
      var saves = new BodySave[s1.Length + s2.Length];

      for (int i = 0; i < s1.Length; i++)
      {
         saves[i] = s1[i];
      }

      for (int i = 0; i < s2.Length; i++)
      {
         s2[i].Id = v.BodiesId + i + 1;
         saves[s1.Length + i] = s2[i];
      }

      v.BodySaves = saves;
   }

   private static void UpsertAttribute(this XmlNode node, string attrName, Func<string?, string> conv)
   {
      var attr = node.Attributes![attrName] ?? node.Attributes.Append(node.OwnerDocument!.CreateAttribute(attrName));

      attr.Value = conv.Invoke(attr.Value == "" ? null : attr.Value);
   }

   public static void Move(this VehicleSave v, int x, int y, int z)
   {
      foreach (XmlNode bodyNode in v.Element["bodies"]!.ChildNodes)
      {
         foreach (XmlNode componentNode in bodyNode["components"]!.ChildNodes)
         {
            var transformNode = componentNode["o"]!;
            var positionNode = transformNode["vp"];
            if (positionNode == null)
            {
               positionNode = transformNode.OwnerDocument.CreateElement("vp");
               componentNode.AppendChild(positionNode);
            }
            
            positionNode.UpsertAttribute("x", str => (int.Parse(str ?? "0") + x).ToString());
            positionNode.UpsertAttribute("y", str => (int.Parse(str ?? "0") + y).ToString());
            positionNode.UpsertAttribute("z", str => (int.Parse(str ?? "0") + z).ToString());
         }
      }
   }
}
