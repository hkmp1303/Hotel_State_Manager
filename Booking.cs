namespace App;

class Booking
{
    public DateTime Start;
    public DateTime End;
    public string Guest;
    public int Room;
    public string Receptionist;

    // Booking constructor
    Booking(long start, long end, string guest, int room, string receptionist)
    {
        Start = DateTimeOffset.FromUnixTimeSeconds(start).UtcDateTime; // convert long to DateTime
        End = DateTimeOffset.FromUnixTimeSeconds(end).UtcDateTime; // convert long to DateTime
        Guest = guest;
        Room = room;
        Receptionist = receptionist;
    }

    // Booking dictionaries, indexed by start or end time
    public static Dictionary<long, Booking> BookingsByStart = new();
    public static Dictionary<long, Booking> BookingsByEnd = new();

    // load from file
    public static void LoadFromFile(string filename)
    {
        // read data from file into string array
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            string[] parts = line.Split('|'); // separate data for loading
            if (parts.Length < 5) continue; // discard invalid data
            // add booking to dictionary
            long start = long.Parse(parts[0]), end = long.Parse(parts[1]);
            int room = int.Parse(parts[3]);
            Booking booking = new Booking(start, end, parts[2], room, parts[4]);
            BookingsByStart.Add(start, booking);
            BookingsByEnd.Add(end, booking);
        }
    }

    // save to file
    public static void SaveToFile(string filename)
    {
        string[] lines = new string[BookingsByStart.Count]; // array of bookings
        int i = 0;
        foreach (var booking in BookingsByStart)
        {
            lines[i++] = booking.Value.Serialize();
        }
        File.WriteAllLines(filename, lines);
    }

    // prepare data for saving
    private String Serialize()
    {
        long start = ((DateTimeOffset)Start).ToUnixTimeSeconds();
        long end = ((DateTimeOffset)End).ToUnixTimeSeconds();
        return $"{start}¤{end}¤{Guest}¤{Room}¤{Receptionist}";
    }
}

enum BookingStatus
{
    Booked,
    CheckedIn,
    CheckedOut,
    Canceled
}