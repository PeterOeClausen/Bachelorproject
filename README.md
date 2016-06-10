# Bachelorproject

To run the program, download the .zip or clone the repository.

Run the .sln file found in folder: "Bachelorproject\DCRGraph Case Study - Subsequent".

Be sure you are running Visual Studio 2015 (may work with later versions as well, but not previous).
Be sure you have installed all updates and frameworks necessary to run the application. Namely Universal Windows Platform application framework, ASP .NET Web API 4.x, and some SQL server tools for Visual Studio.

When the solution is opened. Right-click the "Solution 'DCRGraph Case Study' (2 projects)" solution file in the Solution Explorer.
Click "Set StartUp Projects".
Click "Multiple startup projects:" and select DROM Client (Start) and WebAPI (Start).
Press OK.

You might need to Build and Rebuild the solution.

To empty the database, open the Package Manager Console.
Be sure the Default project: (WebAPI) is selected.
Write: "update-database" and press Enter.
The database should now be emptied.

You can check the database file (.mdf), it's tables and contents by opening the Server Explorer.
Under "Data Connections", a database named "Database (WebAPI)" should be visible, click it.
-OBS: If the database is not there, click "Tools > Connect to database > Microsoft SQL Server Database File" and OK, then browse for the .mdf file (should be in folder DCRGraph Case Study - Subsequent\WebAPI\App_Data), and click ok.
Click on Tables, right click a table and click "Show Table Data" to see the columns and contents of the table.

If you have the correct settings (multiple startup projects) and sufficient updates installed, you should be able to click "Start" and see the DROM Client application starting. Note that a browser tab will open with "Server Error in '/' Application.", ignore this, this is because we have no views on our API.

You should now be able to play around with the DROM Client application.

If you have questions, you may send them to our emails: PeterOeClausen@gmail.com and Johan@Arleth.dk.
