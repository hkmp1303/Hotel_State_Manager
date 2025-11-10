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
    public static Dictionary<string, Booking> BookingsByStart = new();
    public static Dictionary<string, Booking> BookingsByEnd = new();

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
            string[] parts = line.Split('¤'); // separate data for loading
            if (parts.Length < 5) continue; // discard invalid data
            // add booking to dictionary
            long start = long.Parse(parts[0]), end = long.Parse(parts[1]);
            int room = int.Parse(parts[3]);
            Booking booking = new Booking(start, end, parts[2], room, parts[4], parts[5]);
            BookingsByStart.Add($"{start}-{room}", booking);
            BookingsByEnd.Add($"{end}-{room}", booking);
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
        return $"{start}¤{end}¤{Guest}¤{Room}¤{Receptionist}¤{Status}";
    }

    public static Booking? Create(Guest guest, Room room, Receptionist receptionist)
    {
        DateTime start, end;
        if (room.Status != RoomStatus.Vacant)
        {
            System.Console.WriteLine("This room is all ready booked. Press enter to continue");
            System.Console.ReadLine();
            return null;
        }
        System.Console.WriteLine("Select booking start (YYYY-MM-DD HH:mm): "); // entering start DATETIME
        while (true)
            if (DateTime.TryParse(Console.ReadLine() ?? "", out start)) break;
        System.Console.WriteLine("Select booking end (YYYY-MM-DD HH:mm): "); // entering end DATETIME
        while (true)
            if (DateTime.TryParse(Console.ReadLine() ?? "", out end)) break;
        Booking booking = new Booking(start, end, guest.ToString(), room.RoomNumber, receptionist.Username);
        room.Status = RoomStatus.Reserved;
        long startLong = ((DateTimeOffset)booking.End).ToUnixTimeSeconds();
        long endLong = ((DateTimeOffset)booking.Start).ToUnixTimeSeconds();
        BookingsByStart.Add($"{startLong}-{room}", booking);
        BookingsByEnd.Add($"{endLong}-{room}", booking);
        System.Console.WriteLine("The guest's booking has been created. It can be viewed in: List bookings \nPress enter to continue");
        Console.ReadLine();
        return booking;
    }

    public static void PickBookingForCheckin()
    {
        while (true)
        {
            Console.Clear();
            System.Console.WriteLine("Pick a booking from the list, or write [cancel] to go back to menu.");
            int i = 0;
            List<string> listedStarts = new();
            foreach (var booking in BookingsByStart)
            {
                if (booking.Value.Start < DateTime.UtcNow && booking.Value.Status == BookingStatus.Booked) // check booking start DATETIME
                {
                    System.Console.WriteLine($"[{++i}] {booking.Value.Start} Room {booking.Value.Room} - {booking.Value.Guest}");
                    listedStarts.Add(booking.Key);
                }
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
            if (!int.TryParse(selection, out i)) continue;
            if (!BookingsByStart.TryGetValue(listedStarts[i - 1], out b)) continue;
            b.Status = BookingStatus.CheckedIn;
            Booking.SaveToFile("files/bookings.csv");
            System.Console.WriteLine("The guest has been checked in and the booking status updated.\nPress enter to continue");
            System.Console.ReadLine();
            break;
        }
    }

    // list bookings
    public static void ListBookings()
    {
        Console.Clear();
        System.Console.WriteLine("Booking List ("+BookingsByStart.Count()+")");
        foreach (var booking in BookingsByStart)
        {
            System.Console.WriteLine($"Room {booking.Value.Room} ({booking.Value.Start} - {booking.Value.End})");
        }
        System.Console.WriteLine("Press enter to continue");
        Console.ReadLine();
    }

}

enum BookingStatus
{
    Booked,
    CheckedIn,
    CheckedOut,
    Canceled
}