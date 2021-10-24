# Room Reservation System
System for reserving rooms built in C# with .NET 5.0 and Entity Framework. 

See live version [here](https://guillaumefontorbe.com/RoomReservationDemo/) 

## Features
* Role-based authorization
* CRUD operations
* Administration of rooms and users 
## Development
Developed using:
* Microsoft Visual Studio Community 2019
* C#9
* .NET 5.0
* Entity Framework
### Dependencies
* FullCalendar ([link](https://fullcalendar.io/))
### Configuration
#### Initial setup
1. Clone the repository on your machine
```git
git clone https://github.com/gfontorbe/RoomReservation.git
```
2. Create a MSSQL database
3. Update `appsettings.json` to include your connection credentials to the database
```json 
    "ConnectionStrings": {
    "DefaultConnection": "Your database connection string"
    } 
```
4. Apply database migration
```bash
dotnet ef database update
```

#### Initial Seeding of Database
You have to seed the database with at least 1 user with admin role in order to access the functionality of the app.
##### Users
Edit the method `SeedAdminAsync` in `RoomReservation/Data/ApplicationDbInitializer.cs` with changing the `UserName`,`Email`, `FirstName`, `LastName` and `"Password"`

```C#
private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
		{
			var adminUser = new ApplicationUser
			{
				UserName = "Admin UserName",
				Email = "Admin Email",
				EmailConfirmed = true,
				FirstName = "Admin FirstName",
				LastName = "Admin LastName"
			};

			if (userManager.Users.All(u => u.Id != adminUser.Id))
			{
				var user = await userManager.FindByEmailAsync(adminUser.Email);
				if (user == null)
				{
					await userManager.CreateAsync(adminUser, "Password");
					await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
					await userManager.AddToRoleAsync(adminUser, Roles.Basic.ToString());
				}
			}
		}
```

If you do not wish to have a user with basic role in the seeding remove `await SeedBasicUserAsync(userManager);` from the `Initialize` method.

##### Rooms
You can disable the default room seeding by removing `await SeedRoomsAsync(context);` from the `Initialize` method.

#### Routing
If your app is not launched from the root address of your server, you will need to edit several files.
1. Edit `appsettings.json`
```json
"Routing": {
    "Root": "Root path of the app"
  }
```
2. Edit the `deleteEvent()` function in `Views/Calendar/ViewCalendar.cshtml` 
```javascript
await $.post("{Insert the root of your app here}/Reservations/Delete", { id: eventId });
```

3. Depending on how the routing is configured on the server, you might need to adjust configuration files for nginx/apache
#### Build
Publish the app using
```bash
dotnet publish -c Release
```