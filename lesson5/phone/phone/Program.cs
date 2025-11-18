using System;
using System.Linq;
using System.Xml.Linq;

namespace PelephoneProject
{
    internal class Program
    {
        static XDocument doc;

        static void Main(string[] args)
        {
            // Load XML
            doc = XDocument.Load("PelephoneData.xml");

            int choice;

            do
            {
                Console.WriteLine("\n=== Pelephone XML System ===");
                Console.WriteLine("1. Show calls to a specific number");
                Console.WriteLine("2. Show long calls (above 60 minutes)");
                Console.WriteLine("3. Show destinations of a phone");
                Console.WriteLine("4. Add a new call");
                Console.WriteLine("5. Remove phone");
                Console.WriteLine("6. Save & Exit");
                Console.Write("Your choice: ");

                choice = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (choice)
                {
                    case 1: QueryCallsToNumber(); break;
                    case 2: QueryLongCalls(); break;
                    case 3: QueryDestinations(); break;
                    case 4: AddCall(); break;
                    case 5: RemovePhone(); break;
                    case 6: SaveAndExit(); break;
                    default: Console.WriteLine("Invalid option."); break;
                }

            } while (choice != 6);
        }


        // ============================================
        // 1. All callers to a specific number
        // ============================================
        static void QueryCallsToNumber()
        {
            Console.Write("Enter destination number: ");
            string dest = Console.ReadLine();

            var callers =
                doc.Descendants("Call")
                   .Where(c => (string)c.Element("Destination") == dest)
                   .Select(c => c.Parent.Parent.Attribute("number").Value)
                   .Distinct();

            Console.WriteLine($"\nPhones that called {dest}:");
            foreach (var num in callers)
                Console.WriteLine(num);
        }


        // ============================================
        // 2. All calls longer than 60 minutes
        // ============================================
        static void QueryLongCalls()
        {
            var longCalls =
                doc.Descendants("Call")
                   .Where(c => (int)c.Element("Duration") > 60);

            Console.WriteLine("\nCalls longer than 60 minutes:");

            foreach (var c in longCalls)
            {
                Console.WriteLine(
                    "From phone: " +
                    c.Parent.Parent.Attribute("number").Value +
                    ", Duration: " + c.Element("Duration").Value
                );
            }
        }


        // ============================================
        // 3. Destinations dialed by a specific phone
        // ============================================
        static void QueryDestinations()
        {
            Console.Write("Enter phone number: ");
            string num = Console.ReadLine();

            var dests =
                doc.Descendants("Phone")
                   .Where(p => (string)p.Attribute("number") == num)
                   .Descendants("Call")
                   .Select(c => (string)c.Element("Destination"));

            Console.WriteLine($"\nDestinations called by {num}:");
            foreach (var d in dests)
                Console.WriteLine(d);
        }


        // ============================================
        // 4. Add a new call
        // ============================================
        static void AddCall()
        {
            Console.Write("Enter phone number to add call to: ");
            string num = Console.ReadLine();

            var phone = doc.Descendants("Phone")
                           .FirstOrDefault(p => (string)p.Attribute("number") == num);

            if (phone == null)
            {
                Console.WriteLine("Phone not found.");
                return;
            }

            Console.Write("Enter destination: ");
            string dest = Console.ReadLine();

            Console.Write("Enter duration (minutes): ");
            int dur = int.Parse(Console.ReadLine());

            phone.Element("Calls").Add(
                new XElement("Call",
                    new XElement("Start", DateTime.Now.ToString("s")),
                    new XElement("Duration", dur),
                    new XElement("Destination", dest)
                )
            );

            Console.WriteLine("Call added!");
        }


        // ============================================
        // 5. Remove a phone
        // ============================================
        static void RemovePhone()
        {
            Console.Write("Enter phone number to remove: ");
            string num = Console.ReadLine();

            var phone =
                doc.Descendants("Phone")
                   .FirstOrDefault(p => (string)p.Attribute("number") == num);

            if (phone != null)
            {
                phone.Remove();
                Console.WriteLine("Phone removed.");
            }
            else
            {
                Console.WriteLine("Phone not found.");
            }
        }


        // ============================================
        // 6. Save and exit
        // ============================================
        static void SaveAndExit()
        {
            doc.Save("PelephoneData.xml");
            Console.WriteLine("Saved. Goodbye!");
        }
    }
}
