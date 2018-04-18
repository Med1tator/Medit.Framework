using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Medit.Framework.Apis
{
    public class CustomSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (XmlNode childNode in section.ChildNodes)
            {
                string key = string.Empty, value = string.Empty;
                if (childNode.Attributes["key"] != null)
                {
                    key = childNode.Attributes["key"].Value;

                    if (childNode.Attributes["value"] != null)
                    {
                        value = childNode.Attributes["value"].Value;
                    }
                    else
                    {
                        value = string.Empty;
                    }
                    result.Add(key, value);
                }
            }
            return result;
        }
    }
}
