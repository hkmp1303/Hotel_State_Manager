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
    public static Dictionary<string, Guest> GuestDict = new();

    // load method
    static public void LoadFromFile(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            string[] parts = line.Split('¤');
            if (parts.Length < 2) continue; // skip invaild lines
            // add guest to dictionary
            GuestDict.Add(parts[0] + ' ' + parts[1], new Guest(parts[0], parts[1]));
        }

    }

}