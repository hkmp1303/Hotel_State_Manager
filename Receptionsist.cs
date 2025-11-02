namespace App;

class Receptionsist
{
    public string Username;
    private string _password;
    public bool TryLogin(string username, string password)
    {
        return Username == username && _password == password;
    }

    public static Dictionary<string, Receptionsist> ReceptionistList = new();
}
