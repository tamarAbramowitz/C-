using System;
using System.Linq;
using System.Xml.Linq;

namespace phone
{
    public class RemovePhone
    {
        public void ProcessPhoneData(XDocument doc)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            // callers who called 0501111111
            var callers = doc.Descendants("Call")
                .Where(c => (string?)c.Element("Destination") == "0501111111")
                .Select(c => c.Ancestors("Phone").FirstOrDefault()?.Attribute("number")?.Value)
                .Where(n => n != null)
                .Distinct()
                .ToList();

            foreach (var num in callers)
                Console.WriteLine(num);

            string myPhone = "0501111111";

            // calls longer than 60 seconds
            var longCalls = doc.Descendants("Call")
                .Where(c =>
                {
                    var v = c.Element("Duration")?.Value;
                    return int.TryParse(v, out var d) && d > 60;
                })
                .ToList();

            foreach (var call in longCalls)
            {
                var phoneNum = call.Ancestors("Phone").FirstOrDefault()?.Attribute("number")?.Value ?? "<unknown>";
                var duration = call.Element("Duration")?.Value ?? "<unknown>";
                Console.WriteLine("Phone: " + phoneNum + ", Duration: " + duration);
            }

            // destinations for myPhone
            var destinations = doc.Descendants("Phone")
                .Where(p => (string?)p.Attribute("number") == myPhone)
                .Descendants("Call")
                .Select(c => (string?)c.Element("Destination"))
                .Where(s => s != null)
                .ToList();

            foreach (var d in destinations)
                Console.WriteLine(d);
        }

        // optional helper to remove a phone element safely
        public bool RemovePhoneByNumber(XDocument doc, string phoneNumber)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));
            if (phoneNumber == null) throw new ArgumentNullException(nameof(phoneNumber));

            var phones = doc.Descendants("Phone")
                .Where(p => (string?)p.Attribute("number") == phoneNumber)
                .ToList();

            if (!phones.Any()) return false;

            foreach (var p in phones)
                p.Remove();

            doc.Save("PelephoneData.xml");
            Console.WriteLine("Phone(s) removed: " + phoneNumber);
            return true;
        }
    }
}