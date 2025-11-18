using System;
using System.Linq;
using System.Xml.Linq;

namespace phone
{
    public static class AddCall
    {
        // Adds a call entry to the specified phone number in the XDocument.
        public static bool AddNewCall(XDocument doc, string phoneNumber, int durationSeconds, string destination)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            var phone = doc.Descendants("Phone")
                           .FirstOrDefault(p => (string)p.Attribute("number") == phoneNumber);

            if (phone == null)
            {
                Console.WriteLine("Phone not found.");
                return false;
            }

            var callsElement = phone.Element("Calls");
            if (callsElement == null)
            {
                callsElement = new XElement("Calls");
                phone.Add(callsElement);
            }

            var callElement = new XElement("Call",
                new XElement("Start", DateTime.Now.ToString("s")),
                new XElement("Duration", durationSeconds),
                new XElement("Destination", destination)
            );

            callsElement.Add(callElement);
            doc.Save("PelephoneData.xml");
            Console.WriteLine("Call added!");
            return true;
        }
    }
}