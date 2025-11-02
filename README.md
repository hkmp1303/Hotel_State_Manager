# Hotel_State_Manager
A console based program written in C# (.NET 9) where users manage hotel bookings, guest check-in/out and room availibility.

### System requirements
- [ ] login
- [ ] list all occupied rooms
- [ ] list all vacant rooms
- [ ] book a guest in a vacant room
- [ ] check out a guest from a booked room
- [ ] mark a room as temporarily unavailible
- [ ] save data automatically as changes occur
- [ ] utilize enums for room status

### Objectives

The program was created for learning purposes. Specific learning objectives inlcude develping an understanding for programming concepts, git utilization and file system management.

### User background

The program is intended as a complement to an existing system. Feature functions are focused on the present as opposed to furture bookings and will be utilized by a recpetionist to carry out the tasks as listed in System Requirements.

## Quick guide
Follow on screen console prompts throughout the system. Menus display key characters for user selection to navigate the program.

## Design Structure

### Data
The project uses CSV files for persistent data storage as changes occur.

### Project structure

**Guest Class**

In the future the Guest Class would be beneficial in facilitating guests' ability to login and view thier booking requests althought this is not a current system requirement.

**Receptionist Class**

Since the current project is an extention of a larger framework, this class will tie into the existing design structure. The program is designed from the receptionist's perspective as the end user. The login information for these users is stored in this Class.

![Project UML](docs/Hotel_State_UML65.png)

Fullview of the UML class diagram can be accessed [here](https://www.yworks.com/yed-live/?file=https://gist.githubusercontent.com/hkmp1303/7178e6128c76b07bfff16a984a47908d/raw/62283e5daa590a48e0cdeb0ad9f22c6967065088/Hotel%20System) from yworks.
