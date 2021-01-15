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
        private Contact currentContact;
        private string prompt = "Enter d to display contacts, a to add, s to search, r to remove. Enter q to quit";

        public ContactsTextUI(string filename)
        {
            this.dataFileName = filename;
        }

        public void RunUI()
        {
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
                else if (cmd.Equals("r"))
                {
                    runRemoveContact();
                }
                else if (cmd.Equals("s"))
                {
                    runSearchContacts();
                }
                Console.WriteLine(prompt);
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
            Console.WriteLine("Contact added.");
        }

        private void displayContacts()
        {
            Console.WriteLine($"Printing {contacts.Count} contacts:");
            foreach (Contact c in contacts)
            {
                Console.WriteLine(c.ToString());
            }
        }

        private Contact searchContacts(string search)
        {
            // going to return first match (case-insensitive)
            foreach (Contact c in contacts)
            {
                if (c.Name.ToLower().Contains(search.ToLower()))
                {
                    return c;
                }
            }
            // if no contact found
            return null;
        }

        private void runSearchContacts()
        {
            Console.WriteLine("Enter name to search:");
            string search = Console.ReadLine();
            Contact result;
            result = searchContacts(search);
            if (result == null)
            {
                Console.WriteLine("No contact found with name " + search);
            } else
            {
                Console.WriteLine("First match with name containing " + search + ":");
                Console.WriteLine(result.ToString());
            }
        }

        private void removeContact(Contact contact)
        {
            Console.WriteLine("Are you sure you wish to remove " + contact.Name + " from your contacts? y/n");
            string cmd = Console.ReadLine();
            if (cmd.ToLower().Equals("y"))
            {
                contacts.Remove(contact);
                writeContactsCsv();
                Console.WriteLine("Contact removed.");
            }
        }

        private void runRemoveContact()
        {
            Console.WriteLine("Enter name of contact to delete:");
            string search = Console.ReadLine();
            Contact result;
            result = searchContacts(search);
            if (result == null)
            {
                Console.WriteLine("No contact found with name " + search);
            }
            else
            {
                removeContact(result);
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