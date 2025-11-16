# Hotel_State_Manager
A console based program written in C# (.NET 9) where users manage hotel bookings, guest check-in/out and room availability.

### System requirements
- [X] login
- [X] list all occupied rooms
- [X] list all vacant rooms
- [X] book a guest in a vacant room
- [X] check out a guest from a booked room
- [X] mark a room as temporarily unavailable
- [X] save data automatically as changes occur
- [X] utilize enums for room status

### Objectives

The program was created for learning purposes. Specific learning objectives include developing an understanding for programming concepts, git utilization and file system management.

### User background

The program is intended as a complement to an existing system. Feature functions are focused on the present as opposed to future bookings and will be utilized by a receptionist to carry out the tasks as listed in System Requirements.

## Quick guide
Follow on screen console prompts throughout the system. Menus display key characters for user selection to navigate the program.

At program start you can login as "Eve" using the password: pass3

There already exist data which can be manipulated by a user to explore how the system works. There is one Main Menu from which just a few submenus branch off from. The user can exit a menu by typing [cancel] or exit the program from the main menu by selecting menu item 9.

Users can list occupied or vacant rooms as well as bookings. Users can change a room's status, check in/out guests as well as create new bookings or cancel existing ones.

## Design Structure

### Data
The project uses CSV files for persistent data storage. Changes are saved as they occur. Each classes' data is saved by that class. This saving structure allows each class to exist independently of each other, as a separate component. All program data converges in the Booking Class where it is compiled into bookings per user inputs.

### Project structure

**Guest Class**

Users can either create new guests from the guest "picker" method or select existing guests when creating a booking. Currently, this class holds the guest's first and last name.

In the future the Guest Class would be beneficial in facilitating guests' ability to login and view their booking requests although this is not a current system requirement.

**Receptionist Class**

Since the current project is an extension of a larger framework, this class will tie into the existing design structure. The program is designed from the receptionist's perspective as the end user. The login information for these users is stored in this Class.

**Room Class**

The Room Class stores information about a room's status. Potential statuses include: Reserved, Occupied, Vacant or Unavailable. Reserved rooms are associated with a booking in which the guest has not checked in. Occupied rooms have a checked in guest. Vacant rooms are available, unoccupied and unreserved. Unavailable rooms have been removed from rotation for scheduling by the receptionist for maintenance or another reason.

At program start the following rooms have the following status:
- Reserved: 102, 104, 105
- Occupied: 101, 103, 213
- Unavailable: 213
- Vaccant: 106, 107, 108, 109, 201 through 209

**Booking Class**

The information required to fulfill a customer's booking request is stored in the Booking Class. This information includes guest name, room number, booking status, the receptionist responsible for the booking as well as start and end times for the reservation. Room status is retrieved from the Room Class and used to filter available rooms.


**Class UML**

![Project UML](docs/Hotel_State_UML65.png)

Fullview of the UML class diagram at the start of the project can be accessed [here](https://www.yworks.com/yed-live/?file=https://gist.githubusercontent.com/hkmp1303/7178e6128c76b07bfff16a984a47908d/raw/62283e5daa590a48e0cdeb0ad9f22c6967065088/Hotel%20System) from yworks.

Fullview of UML at end of project [here](https://www.yworks.com/yed-live/?file=https://gist.githubusercontent.com/hkmp1303/0b571b249a5ef3986c19612b756b6631/raw/15c05512755769c2415e0b82ebc271eb4ff8cf11/Hotel%20System) from yworks.

## Known Issues

- Bookings
  - Booking timestamps do not consider different timezones. As a result, entries could be erroneously entered in the wrong timezone. Converting user inputs to a standard UTC time would resolve this issue.
  - Automatic triggers for time based events are not available. To implement  such would require continuous time monitoring which is outside of the scope of the project. However, this could be an excellent future objective.
  - The system does not explicitly manage expired bookings, especially where the guest did not check-in. Expired bookings, where the end date and time have lapsed are filtered out from the list bookings method. However, the create bookings method allows for bookings to be created which have past start and end points. The system requirements did not request a specific feature to restrict this possibility so it was not addressed.
  - Bookings are not modifiable. They can only be created or canceled. Modifying bookings was not explicitly requested as a system requirement. This feature was not developed as it is outside of the minimal viable product requirements. However, the feature could be added in the future.
- Room
  - The room selecting method does not consider availability based on time which could result in double bookings for any given room.
