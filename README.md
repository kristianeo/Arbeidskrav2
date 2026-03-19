# SecondHandMarket

A terminal-based second-hand market where users can:
- create a profile or log in
- view their own profile
- browse and buy listings
- leave reviews 
- create their own listings

## Table of Contents

- [Version Control](#version-control)
- [Technologies Used](#technologies-used)
- [Additional Imports](#additional-imports)
- [Usage](#usage)
- [Navigation Guide](#navigation-guide)
- [Process](#process)
- [Sources](#sources)

## Version control

Kristiane Olsen [GitHub](https://github.com/kristianeo/Arbeidskrav2)

## Technologies Used

- **.NET 10.0 framework**: Code written in C#
- **Microsoft.Data.Sqlite**: Database without the need for server setup

## Additional Imports
- **Microsoft.AspNetCore.Cryptography.KeyDerivation**: Used to encrypt passwords
- **System.Data.SQLite**: Write SQL queries in C#

## Usage

### Running the Application
The application is ready to use. **If** there are any issues with the database,
it can be set up from scratch like this, before running the other code in Main(): 

```bash
DBInteractor interactor = new DBInteractor();

interactor.CreateDatabase();
interactor.CreateUsersTable();
interactor.CreateListingsTable();
interactor.CreateReviewTable();
```
I had to edit the run configuration to not run via bin/debug, for the database to appear in the project, and not in the bin.

## Navigation Guide
**Create profile or log in**: Create a new profile or log in to an existing one before entering the market.

**Main Menu**:
- Create listing: 
  - The user can create their own listing for the market.
- Browse: 
  - Allows the user to browse listings available for purchase. 
  - View individual listings for more information, and purchase them.
- Search: 
  - By category: The user selects desired category
  - Free search: Words in title or description.
- My profile:
    - Shows: Average reviews score, active listings, sold listings and reviews left from other users.
    - User can choose to view their active listings and edit or remove them. 
- Log out:
  - Sets the user as inactive in the database and exits the program.

## Process
In the beginning, I used Craig's example from class, [TicketProcessing](https://gitlab.com/muskatel/ticketsystem/-/tree/master/TicketProcessing?ref_type=heads), 
to create the outline. It implements encapsulation, which limits users direct access to the data, and instead
controls it with public methods. I used this outline to also make ListingFilters, and made a generic method to invoke these.
Here you can see some use of LINQ-expressions, but I found this hard to use with the database and SQL-queries
later on.

I soon implemented a database, as data from the above example would reset every time the program exits. 
I have made generic methods for retrieval of data from the database where this has been reasonable. 

I have chosen not to use inheritance (except for DbInteractor which inherits startup code for the DB from Init)
as I have not found a use for it in this assignment. The DbInteractor contains code used to access the database.

The Listings and User classes only contains data, which is accessed through ListingsCollection and UserCollection 
respectfully. I chose to still use some of the original code, for example creating a Listings instance
before putting it in the database, even though this is not strictly necessary.

MainMenu.UserInterface uses code from DbInteractor, ListingCollection and UserCollection to bring the
code together and set up for a smooth user experience in Main().

I did not use any AI in this assignment.

## Sources
- [Get console secure password](https://gist.github.com/huobazi/1039424)
- [Create SQLite database](https://stackoverflow.com/questions/15292880/create-sqlite-database-and-table)
- [Password hashing](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-10.0)
- [Reading from SQLite database](https://www.instructables.com/Reading-and-Writing-Data-Into-SQLite-Database-Usin/)
- [Craig's TicketProcessing example](https://gitlab.com/muskatel/ticketsystem/-/tree/master/TicketProcessing?ref_type=heads)
