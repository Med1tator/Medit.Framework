using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medit.Framework.Serializer
{
    /// <summary>
    /// XML序列化器
    /// </summary>
    public class XMLSerializer
    {
        public static string Serialize<T>(T t) where T : new()
        {
            string result = string.Empty;
            Type type = typeof(T);
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(type);
                xml.Serialize(Stream, t);
                Stream.Position = 0;
                using (StreamReader sr = new StreamReader(Stream))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        public static T Deserialize<T>(string xml) where T : new()
        {
            Type type = typeof(T);
            T result = new T();
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                result = (T)xmlSerializer.Deserialize(sr);
            }
            return result;
        }

        public static T Deserialize<T>(Stream stream) where T : new()
        {
            Type type = typeof(T);
            T result = new T();
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            result = (T)xmlSerializer.Deserialize(stream);
            return result;
        }
    }
}
