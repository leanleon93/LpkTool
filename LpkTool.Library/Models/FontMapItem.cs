using System.ComponentModel;
using System.Xml.Serialization;

namespace LpkTool.Library.Models
{
    [Serializable]
    public class FontMapItem
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlAttribute]
        public string File { get; set; }
        [XmlAttribute, DefaultValue(0)]
        public int LeadingCorrection { get; set; }
        [XmlAttribute, DefaultValue(0)]
        public int SizeCorrectionRatio { get; set; }

    }
}