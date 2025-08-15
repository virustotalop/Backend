# Backend

## Environment variables

`BACKEND_CONNECTION_STRING` - Microsoft SQL Server connection string

## Design Decisions

I separated the database and CRUD layers because of seperation of concerns.

## Challenges

I wasn't really familiar with EF Core or Mvc before this project and I had some problems with
them but they were pretty straight forward once I got used to them after reading documentation, they are fairly similar to spring which I already had experience with so I was already familiar with the concepts but just not how everything was implemented. What I had more issues with is how to structure
the tests and what testing framework to go with. I landed on just using the standard testing library with visual studio, another option I did explore was xunit. There are apparently ways to mock the web server and other ways to construct the web application instance but In the end I think picked the best option for this use case which is to just run the regular web server like I do in the main method. Besides that what made testing challenging was figuring out how to actually get good debug information out of Visual Studio to debug what the issue was. In the end I found where the debugging information I needed.