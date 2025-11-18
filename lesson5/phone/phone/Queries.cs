using System;
using System.Linq;
using System.Xml.Linq;

namespace phone
{
    public class Queries
    {
        public static void PrintCallersToNumber(XDocument doc, string destinationNumber)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            var callers = doc.Descendants("Call")
                .Where(c => (string?)c.Element("Destination") == destinationNumber)
                .Select(c => c.Ancestors("Phone").FirstOrDefault()?.Attribute("number")?.Value)
                .Where(n => n != null)
                .Distinct();

            foreach (var num in callers)
                Console.WriteLine(num);
        }

        public static void PrintLongCalls(XDocument doc, int minDurationSeconds = 60)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            var longCalls = doc.Descendants("Call")
                .Where(c =>
                {
                    var s = c.Element("Duration")?.Value;
                    return int.TryParse(s, out var d) && d > minDurationSeconds;
                });

            foreach (var call in longCalls)
            {
                var phoneNum = call.Ancestors("Phone").FirstOrDefault()?.Attribute("number")?.Value ?? "<unknown>";
                var duration = call.Element("Duration")?.Value ?? "<unknown>";
                Console.WriteLine("Phone: " + phoneNum + ", Duration: " + duration);
            }
        }

        public static void PrintDestinationsForPhone(XDocument doc, string phoneNumber)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            var destinations = doc.Descendants("Phone")
                .Where(p => (string?)p.Attribute("number") == phoneNumber)
                .Descendants("Call")
                .Select(c => (string?)c.Element("Destination"))
                .Where(d => d != null);

            foreach (var d in destinations)
                Console.WriteLine(d);
        }
    }
}