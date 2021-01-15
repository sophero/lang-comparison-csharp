using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // declare name of contacts list
            string contactsFileName = "contacts.csv";

            // make new um. textUI
            ContactsTextUI UI = new ContactsTextUI(contactsFileName);

            // where does the function to read in the csv go? ahh, fuck this is gonna be a pain in the ass.
            UI.ReadContactsCsv();
            UI.RunUI();
        }

    }

    public class ContactsTextUI
    {
        private string dataFileName;
        private List<Contact> contacts = new List<Contact>();
        private string prompt = "Enter d to display contacts. Enter q to quit";

        public ContactsTextUI(string filename)
        {
            this.dataFileName = filename;
        }

        public void RunUI()
        {
            Console.WriteLine(prompt);
            string cmd = "";
            while (!cmd.Equals("q"))
            {
                if (cmd.Equals("d"))
                {
                    displayContacts();
                }
                cmd = Console.ReadLine();
            }
        }

        private void displayContacts()
        {
            Console.WriteLine($"Printing {contacts.Count} contacts:");
            foreach (Contact c in contacts)
            {
                Console.WriteLine(c.ToString());
            }
        }

        public void ReadContactsCsv()
        {
            using (var reader = new StreamReader(dataFileName))
            {
                int lineNum = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (lineNum > 0)
                    {
                        contacts.Add(new Contact(values[0], values[1], values[2]));
                    }
                    lineNum++;
                }
            }
        }
    }

    public class Contact
    {
        public string Name { get; set; }
        private string Phone { get; set; }
        private string Email { get; set; }

        public Contact(string name, string phone, string email)
        {
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }

        override
        public string ToString()
        {
            return Name + " " + Phone + " " + Email;
        }
    }
}