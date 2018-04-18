using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Medit.Framework.Serializer
{
    /// <summary>
    /// JSON序列化器
    /// </summary>
    public class JSONSerializer
    {
        public static string Serialize<T>(T t) where T : new()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(t);
        }

        public static T Deserialize<T>(string json) where T : new()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(json);
        }
    }
}
