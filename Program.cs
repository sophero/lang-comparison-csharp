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

            // instantiate textUI class and call run UI method.
            ContactsTextUI UI = new ContactsTextUI(contactsFileName);
            UI.RunUI();
        }

    }

    public class ContactsTextUI
    {
        private string dataFileName;
        private List<Contact> contacts = new List<Contact>();
        private string prompt = "Enter d to display contacts, a to add, s to search, r to remove, u to update. Enter q to quit";

        public ContactsTextUI(string filename)
        {
            dataFileName = filename;
            readContactsCsv();
        }

        public void RunUI()
        {
            string cmd = "";
            while (!cmd.Equals("q"))
            {
                if (cmd.ToLower().Equals("d"))
                {
                    displayContacts();
                }
                else if (cmd.ToLower().Equals("a"))
                {
                    addContact();
                }
                else if (cmd.ToLower().Equals("r"))
                {
                    runRemoveContact();
                }
                else if (cmd.ToLower().Equals("s"))
                {
                    runSearchContacts();
                }
                else if (cmd.ToLower().Equals("u"))
                {
                    runUpdateContact();
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

        // search by contact name
        private Contact searchContacts(string search)
        {
            foreach (Contact c in contacts)
            {
                if (c.Name.ToLower().Contains(search.ToLower()))
                {
                    Console.WriteLine(c.ToString());
                    Console.WriteLine("Is this the contact you are looking for? y/n");
                    string cmd = Console.ReadLine();
                    if (cmd.ToLower().Equals("y"))
                    {
                        return c;
                    }
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
                Console.WriteLine("Resulting match " + search + ":");
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
                return;
            }
            removeContact(result);
        }

        private void runUpdateContact()
        {
            Console.WriteLine("Enter name of contact to update:");
            string search = Console.ReadLine();
            Contact result;
            result = searchContacts(search);
            // if no contact found, print message and return
            if (result == null)
            {
                Console.WriteLine("No contact found for name " + search);
                return;
            }
            // print current contact info and run update
            Console.WriteLine(result.ToString());

            string name = result.Name;
            string phone = result.Phone;
            string email = result.Email;

            string cmd = "";
            while (!cmd.ToLower().Equals("d"))
            {
                if (cmd.ToLower().Equals("n"))
                {
                    Console.WriteLine("Enter new name:");
                    name = Console.ReadLine();
                }
                else if (cmd.ToLower().Equals("p"))
                {
                    Console.WriteLine("Enter new phone number:");
                    phone = Console.ReadLine();
                }
                else if (cmd.ToLower().Equals("e"))
                {
                    Console.WriteLine("Enter new email address:");
                    email = Console.ReadLine();
                }
                Console.WriteLine("Contact details: ");
                Console.WriteLine(name + " " + phone + " " + email);
                Console.WriteLine("To update Name, Phone or Email, type N, P or E. Type D when done:");
                cmd = Console.ReadLine();
            }
            // update details.. does this work???
            result.Name = name;
            result.Phone = phone;
            result.Email = email;
            writeContactsCsv();
            Console.WriteLine("Contact updated.");
        }

        private void readContactsCsv()
        {
            try
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
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"No file found at: " + dataFileName);
                Console.WriteLine("Contacts list will be empty.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"File could not be opened: {e}");
            }
            
        }

        private void writeContactsCsv()
        {
            // delete existing file... this gets around the "file cannot be accessed because currently in use" error
            if (File.Exists(dataFileName))
            {
                File.Delete(dataFileName);
            }
            try
            {
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
            catch (IOException e)
            {
                Console.WriteLine($"File could not be written to: {e}");
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
            Name = name;
            Phone = phone;
            Email = email;
        }

        override
        public string ToString()
        {
            return Name + " " + Phone + " " + Email;
        }
    }
}