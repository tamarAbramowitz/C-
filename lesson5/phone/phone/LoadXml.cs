using System;
using System.Xml.Linq;

namespace phone
{
    public class LoadXml
    {
        public XDocument Doc { get; }

        // Loads the XML when an instance is created.
        public LoadXml(string path = "PelephoneData.xml")
        {
            Doc = XDocument.Load(path);
            Console.WriteLine("XML Loaded!");
        }

        // Convenience static loader if callers prefer a single call.
        public static XDocument Load(string path = "PelephoneData.xml")
        {
            var doc = XDocument.Load(path);
            Console.WriteLine("XML Loaded!");
            return doc;
        }
    }
}