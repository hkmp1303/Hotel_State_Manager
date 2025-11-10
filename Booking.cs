namespace App;

class Booking
{
    public DateTime Start;
    public DateTime End;
    public string Guest;
    public int Room;
    public string Receptionist;
    public BookingStatus Status;

    // Booking dictionaries, indexed by start or end time
    public static Dictionary<long, Booking> BookingsByStart = new();
    public static Dictionary<long, Booking> BookingsByEnd = new();

    // Booking constructor
    Booking(long start, long end, string guest, int room, string receptionist, string bookingStatus)
    {
        Start = DateTimeOffset.FromUnixTimeSeconds(start).UtcDateTime; // convert long to DateTime
        End = DateTimeOffset.FromUnixTimeSeconds(end).UtcDateTime; // convert long to DateTime
        Guest = guest;
        Room = room;
        Receptionist = receptionist;
        Status = Enum.Parse<BookingStatus>(bookingStatus); // trusted b/c read from savefile
    }

    Booking(DateTime start, DateTime end, string guest, int room, string receptionist)
    {
        Start = start; // convert long to DateTime
        End = end; // convert long to DateTime
        Guest = guest;
        Room = room;
        Receptionist = receptionist;
        Status = BookingStatus.Booked;
    }

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
            Booking booking = new Booking(start, end, parts[2], room, parts[4], parts[5]);
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

    public static Booking Create(Guest guest, Room room, Receptionist receptionist)
    {
        DateTime start, end;
        System.Console.WriteLine("Select booking start (YYYY-MM-DD HH:mm): ");
        while (true)
            if (DateTime.TryParse(Console.ReadLine() ?? "", out start)) break;
        System.Console.WriteLine("Select booking end (YYYY-MM-DD HH:mm): ");
        while (true)
            if (DateTime.TryParse(Console.ReadLine() ?? "", out end)) break;
        Booking booking = new Booking(start, end, guest.ToString(), room.RoomNumber, receptionist.Username);
        BookingsByEnd.Add(((DateTimeOffset)booking.End).ToUnixTimeSeconds(), booking);
        BookingsByStart.Add(((DateTimeOffset)booking.Start).ToUnixTimeSeconds(), booking);
        return booking;
    }

    public static void PickBookingForCheckin()
    {
        while (true)
        {
            Console.Clear();
            System.Console.WriteLine("Pick a booking from the list, or write [cancel] to go back to menu.");
            int i = 0;
            List<long> listedStarts = new();
            foreach (var booking in BookingsByStart)
            {
                if (booking.Value.Start < DateTime.UtcNow) // have booking started ?
                    System.Console.WriteLine($"[{++i}] {booking.Value.Start} Room {booking.Value.Room} - {booking.Value.Guest}");
            }
            if (listedStarts.Count == 0)
            {
                System.Console.WriteLine("No bookings have started yet, press enter to go back to the previous menu.");
                Console.ReadLine();
                break;
            }
            Booking? b;
            string selection = Console.ReadLine() ?? "";
            if (selection.Trim() == "") continue;
            if (selection == "cancel") break;
            if (int.TryParse(selection, out i) && BookingsByStart.TryGetValue(listedStarts[i-1], out b))
            {
                b.Status = BookingStatus.CheckedIn;
                Booking.SaveToFile("files/bookings.csv");
            }
        }
    }

}

enum BookingStatus
{
    Booked,
    CheckedIn,
    CheckedOut,
    Canceled
}