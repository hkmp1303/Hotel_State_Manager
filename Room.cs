using System.Runtime.Intrinsics.Arm;

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

    // List rooms with optional filter
    public static void ListRooms(RoomStatus? filter = null)
    {
        Console.Clear();
        System.Console.WriteLine("Room List\n");
        foreach (var room in RoomList)
        {
            if (filter == null || filter == room.Value.Status)
                System.Console.WriteLine($"[{room.Key}] ({room.Value.Status})"); // print room status
        }
    }

    // Pick room from list
    public static Room? PickRoom(RoomStatus? filter = null)
    {
        while (true)
        {
            ListRooms(filter);
            Room? room;
            int roomNumber = 0;
            System.Console.WriteLine("Pick a room number from the list\n[cancel] discard changes");
            string selection = Console.ReadLine()?? "";
            if (selection == "cancel") return null;
            if (!int.TryParse(selection, out roomNumber))
            {
                System.Console.WriteLine("Could not find room number, try again. Press enter to continue");
                Console.ReadLine();
                continue;
            }
            if (RoomList.TryGetValue(roomNumber, out room))
                return room;
        }
    }

    // shows and operates status picker, null means user canceled selection
    public static RoomStatus? PickStatus()
    {
        while (true)
        {
            Console.Clear();
            System.Console.WriteLine("Select the room status you wish to enter \n\n[cancel] abort status change\n");
            List<RoomStatus> types = new(Enum.GetValues<RoomStatus>());
            int i = 0;
            for (i = 0; i < types.Count(); i++)
            {
                System.Console.WriteLine($"[{i + 1}] {types[i]}");
            }
            string selection = Console.ReadLine() ?? "";
            if (selection == "cancel") return null;
            if (int.TryParse(selection, out i) && i > 0 && types.Count() >= i) // check if selected room status exists
                return types[i - 1];
        }
    }
}
enum RoomStatus
{
    Unavailable,
    Reserved,
    Occupied,
    Vacant
}