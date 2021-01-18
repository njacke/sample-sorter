1. SETUP
- Starup SSMS (or your preferred SQL GUI)
- Connect to localhost server
- Open Setup Query.sql (located in Sample Sorter folder)
- Select all and execute to set up the database and stored procedures

- Open SampleSorter.sln (located in Sample Sorter folder)
- View->Server Explorer->Data Conections->Add Connection (server name: local host, database name: SampleDatabase)
- Replace _connnectionString in Program.cs with your Connection String if needed
- Make sure all required NuGet Packages are installed (System.Data.SqlClient, Dapper, GemBox.Spreadsheet)
- Build solution and run SampleSorter

2. PROGRAM
On startup program will recreate a table with 1000 random string SampleText values and execute 1 - Get List option.
After that you can select ay of the options below.

1 - Get list
- Gets the table from SQL and returns it as an IList<T>
- Prints top 100 lines to console

2 - DB sort by SampleText
- Gets a sorted by SampleText value table from SQL and returns it as an IList<T>
- Prints top 100 rows and sort execution time to console

3 - Bubbble sort by SampleText
- Gets the table from SQL, sorts it with a simple bubble algorith and returns as an IList<T>
- Prints sort execution time and top 100 rows to console

4 - Analysis and report
- Tests both sorting methods on tables of 100, 1.000, 10.000, and 100.000 entries
- Stores execution time values and creates a PerformanceReport.xlsx with a table and a line chart (see PerformanceReportExample.xlsx for exptected output)
- 100.000 entries bubble sort can take a while (20mins on my PC), remove it from _tests if needed