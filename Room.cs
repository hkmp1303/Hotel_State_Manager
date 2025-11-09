namespace App;

class Room
{
    public int RoomNumber;
    public RoomStatus Status;

    // room constructor
    Room(int roomNumber, RoomStatus status)
    {
        RoomNumber = roomNumber;
        Status = status;
    }

    // dictionary of room numbers
    public static Dictionary<int, Room> RoomList = new();

    // load method
    public static void LoadFromFile(string filename)
    {
        // read data from file into string array lines
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)              // loop through array lines
        {
            string[] parts = line.Split('¤');       // separate data for loading
            if (parts.Length < 2) continue;         // Skip invalid lines
            int roomnumber = int.Parse(parts[0]);   // initialize variable, cast data from string to int for loading
            // add roomnumber to dictionary
            RoomList.Add(roomnumber, new Room(roomnumber, (RoomStatus)Enum.Parse(typeof(RoomStatus), parts[1])));
        }
    }

    // save room number to file
    public static void SaveToFile(string filename)
    {
        string[] lines = new string[RoomList.Count];  // array of room numbers
        int i = 0; // initalize variable i
        foreach (var room in RoomList) // loop through RoomList dictionary
        {
            lines[i++] = $"{room.Key}¤{room.Value.Status}"; // prepare data for saving, separate by ¤
        }
        File.WriteAllLines(filename, lines); // write data to save file
    }
}
enum RoomStatus
{
    Unavailible,
    Reserved,
    Occupied,
    Vacant
}