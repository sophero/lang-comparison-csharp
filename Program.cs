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
                else if (cmd.Equals("a"))
                {
                    addContact();
                }
                cmd = Console.ReadLine();
            }
        }

        private void addContact()
        {
            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter phone number:");
            string phone = Console.ReadLine();
            Console.WriteLine("Enter email address:");
            string email = Console.ReadLine();
            Contact newContact = new Contact(name, phone, email);
            // add new contact to contacts list and update csv
            contacts.Add(newContact);
            writeContactsCsv();
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

        private void writeContactsCsv()
        {
            // delete existing file... this gets around the "file cannot be accessed because currently in use" error
            if (File.Exists(dataFileName))
            {
                File.Delete(dataFileName);
            }
            using (var writer = new StreamWriter(dataFileName))
            {
                // write column headers
                string headers = "Name,Phone,Email";
                writer.WriteLine(headers);
                // write contacts
                foreach (Contact c in contacts)
                {
                    string newLine = $"{c.Name},{c.Phone},{c.Email}";
                    writer.WriteLine(newLine);
                }
            }
            
        }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

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