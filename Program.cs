using System.Diagnostics;
using App;

// Load data from files
string roomSaveFile = "files/rooms.csv",
    guestSaveFile = "files/guest.csv",
    bookingSaveFile = "files/bookings.csv",
    receptionstsSaveFile = "files/receptionists.csv";
Room.LoadFromFile(roomSaveFile);
Guest.LoadFromFile(guestSaveFile);
Booking.LoadFromFile(bookingSaveFile);
Receptionist.LoadFromFile(receptionstsSaveFile);

bool running = true;

while (running)
{
    Console.Clear();
    System.Console.WriteLine("Welcome to the Hotel California");
    System.Console.Write("Please login\nUsername: ");
    Receptionist? receptionist;
    if (!Receptionist.ReceptionistList.TryGetValue(Console.ReadLine() ?? "", out receptionist))
    {
        System.Console.WriteLine("Please enter a vaild username. Press enter to continue");
        System.Console.ReadLine();
        continue;
    }
    System.Console.Write("Password: ");
    if (!receptionist.TryLogin(receptionist.Username, Console.ReadLine() ?? ""))
    {
        System.Console.WriteLine("The username and password do not match. Press enter to reenter your login information.");
        System.Console.ReadLine();
        continue;
    }
    while (true)
    {
        Console.Clear();
        System.Console.WriteLine($"Welcome{receptionist.Username}\nMain Menu\n" +
            "[1] Occupied rooms\n" +
            "[2] Vacant rooms list\n" +
            "[3] Change room status\n" +
            "[4] Check out or in a guest\n" +
            "[5] Create booking\n" +
            "[6] Exit program"
        );

        int choice = 0;
        if (!int.TryParse(Console.ReadLine(), out choice)) continue;
        switch (choice)
        {
            case 1: // list occupied rooms
                Room.ListRooms(RoomStatus.Occupied);
                System.Console.WriteLine("Press enter to continue");
                Console.ReadLine();
                break;
            case 2: // list vacant rooms
                Room.ListRooms(RoomStatus.Vacant);
                System.Console.WriteLine("Press enter to continue");
                Console.ReadLine();
                break;
            case 3: // change room status
                Room room = Room.PickRoom();
                room.Status = Room.PickStatus();
                Room.SaveToFile(roomSaveFile);
                break;
            case 4: // check in/out guest
                Booking.PickBookingForCheckin();
                break;
            case 5: // create booking
                Guest guest = Guest.PickGuest();
                Room bookRoom = Room.PickRoom(RoomStatus.Vacant);
                Booking.Create(guest, bookRoom, receptionist);
                Booking.SaveToFile(bookingSaveFile);
                break;
            case 6: // exit
                running = false;
                break;
            default:
                continue;
        }
    }
}