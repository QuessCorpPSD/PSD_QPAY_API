using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QPay.BAL.IRepository.Extensions
{
    public static class XmlHelper
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

    public static class XmlHelper2
    {
        public static string SerializeObjectToXml<T>(T value)
        {
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,   // remove <?xml version="1.0" ...?>
                Indent = false,
                Encoding = new UTF8Encoding(false)
            };

            var xmlSerializer = new XmlSerializer(typeof(T));

            // Remove namespaces
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");   // VERY IMPORTANT: no namespace

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);

            xmlSerializer.Serialize(xmlWriter, value, ns);

            return stringWriter.ToString();
        }
    }
}
