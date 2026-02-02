using System.Xml.Serialization;

namespace QPay.BAL.IRepository
{
    public class XmlHelper
    {
        public static string SerializeObjectToXml<T>(T obj, string rootName = "Main")
        {
            if (obj == null) return string.Empty;

            var xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
    }
}
