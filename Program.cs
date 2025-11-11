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
    System.Console.WriteLine("Welcome to the Hotel California\n");
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
        System.Console.WriteLine($"Welcome {receptionist.Username}\nMain Menu\n" +
            "[1] Occupied rooms\n" +
            "[2] Vacant rooms\n" +
            "[3] Change room status\n" +
            "[4] Check out or in a guest\n" +
            "[5] Create or cancel a booking\n" +
            "[6] List bookings\n" +
            "[9] Exit program"
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
                Room? room = Room.PickRoom();
                if (room == null) break;
                RoomStatus? newStatus = Room.PickStatus();
                room.Status = newStatus ?? room.Status; // revert to previous room status if it is not set
                Room.SaveToFile(roomSaveFile);
                break;
            case 4: // check in/out guest
                while (true)
                {
                    Console.Clear();
                    System.Console.WriteLine("Check In/Out Menu\n\n[cancel] discard changes\n[1] Check in a guest\n[2] Check out a guest");
                    string selection = Console.ReadLine() ?? "";
                    switch (selection)
                    {
                        case "1":
                            Booking.PickBookingForCheckin();
                            break;
                        case "2":
                            Booking.CheckOut();
                            break;
                        case "cancel":
                            break;
                        default:
                            continue;
                    }
                    break;
                }
                break;
            case 5: // create or cancel booking
                while (true)
                {
                    Console.Clear();
                    System.Console.WriteLine("Bookings Menu\n\n[cancel] discard changes\n[1] Create booking\n[2] Cancel booking");
                    string selection = Console.ReadLine() ?? "";
                    switch (selection)
                    {
                        case "1": // Create booking
                            Guest? guest = Guest.PickGuest();
                            if (guest == null) break; // user canceled or no guests
                            Room? bookRoom = Room.PickRoom(RoomStatus.Vacant);
                            if (bookRoom == null) break;
                            Booking.Create(guest, bookRoom, receptionist);
                            Booking.SaveToFile(bookingSaveFile);
                            Room.SaveToFile(roomSaveFile);
                            break;
                        case "2": // Cancel booking
                            // if (guest == null) break;
                            Booking? bookingToCancel = Booking.PickBooking(BookingStatus.Booked);
                            if (bookingToCancel == null) break; // user canceled or no bookings
                            bookingToCancel.Cancel();
                            break;
                        default:
                            continue;
                    }
                    break;
                }
                break;
            case 6: // list bookings
                Booking.ListBookings();
                break;
            case 9: // exit
                return;
            default:
                continue;
        }
    }
}