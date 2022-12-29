using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LpkTool.Library.Models
{
    [Serializable]
    [XmlRoot("FontMap")]
    public class FontMap
    {
        public FontMap()
        {
            Items = new List<FontMapItem>();
        }

        [XmlElement("Item")]
        public List<FontMapItem> Items { get; set; }

        public static FontMap FromXml(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FontMap));
            FontMap result = (FontMap)xmlSerializer.Deserialize(new StringReader(xml));
            return result;
        }

        public string ToXml()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer xsSubmit = new XmlSerializer(typeof(FontMap));
            var encoding = new UTF8Encoding(false);
            var subReq = this;
            var xml = "";

            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms, encoding))
            {
                xsSubmit.Serialize(sw, subReq, ns);
                xml = Encoding.UTF8.GetString(ms.ToArray());
            }

            return xml;
        }

    }
}
