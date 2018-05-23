using System.IO;
using System.Xml.Serialization;

namespace _Editor.GramBlog
{
    public class XMLOp
    {
        public static void Serialize(object item, string path)
        {
            var serializer = new XmlSerializer(item.GetType());
            var writer = new StreamWriter(path: path);
            serializer.Serialize(stream: writer.BaseStream, o: item);
            writer.Close();
        }

        public static T Deserialize<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new StreamReader(path: path);
            var deserialized = (T) serializer.Deserialize(stream: reader.BaseStream);
            reader.Close();
            return deserialized;
        }
    }
}