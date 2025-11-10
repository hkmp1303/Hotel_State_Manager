using Microsoft.VisualBasic;

namespace App;

class Guest
{
    public string FirstName;
    public string LastName;

    // Guest constructor
    Guest(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    // Dictionary of guests
    public static List<Guest> Guests = new();

    // load method
    static public void LoadFromFile(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            string[] parts = line.Split('¤');
            if (parts.Length < 2) continue; // skip invaild lines
            // add guest to dictionary
            Guests.Add(new Guest(parts[0], parts[1]));
        }

    }

    static public void SaveToFile(string filename)
    {
        string[] lines = new string[Guests.Count()];
        int i = 0;
        foreach (var guest in Guests)
        {
            lines[i++] = guest.FirstName+'¤'+guest.LastName;
        }
        File.WriteAllLines(filename, lines);
    }

    public override string ToString()
    {
        return (FirstName + ' ' + LastName).Trim();
    }

    public static Guest? PickGuest()
    {
        while (true)
        {
            Console.Clear();
            System.Console.WriteLine("Select a guest from the list or select [0] to create a new guest\n\n[cancel] discontinue selection");
            int i = 1;
            foreach (var guest in Guests)
            {
                System.Console.WriteLine($"[{i++}] {guest}");
            }
            string selection = Console.ReadLine() ?? "";
            if (selection == "cancel") return null;
            if (!int.TryParse(selection, out i)) continue;
            Guest g;
            if (i > 0 && Guests.Count() >= i)
            {
                g = Guests[i - 1];
                return g;
            }
            else if (i == 0)
            {
                System.Console.Write("Enter firstname: ");
                string firstName = Console.ReadLine() ?? "";
                System.Console.Write("Enter lastname: ");
                string lastName = Console.ReadLine() ?? "";
                g = new Guest(firstName, lastName);
                Guests.Add(g);
                SaveToFile("files/guest.csv");
                return g;
            }
        }

    }
}