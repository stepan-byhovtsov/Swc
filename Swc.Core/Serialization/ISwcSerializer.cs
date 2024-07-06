namespace Swc.Core.Serialization;

public interface ISwcSerializer
{
   public void Serialize(object obj, Type type, Stream stream);
   public object? Deserialize(Type type, Stream stream);
}
