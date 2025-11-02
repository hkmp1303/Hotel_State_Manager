namespace App;

class Receptionsist
{
    public string Username;
    private string _password;
    public bool TryLogin(string username, string password)
    {
        return Username == username && _password == password;
    }

    // constructor
    Receptionsist(string username, string password)
    {
        Username = username;
        _password = password;
    }

    // dictionary of receptionsits/users
    public static Dictionary<string, Receptionsist> ReceptionistList = new();

    //load method
    static public void LoadFromFile(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            string[] parts = line.Split('¤');
            if (parts.Length < 2) continue; // skip invalid lines
            // Add receptionist to dictionary from file
            ReceptionistList.Add(parts[0], new Receptionsist(parts[0], parts[1]));
        }
    }
}