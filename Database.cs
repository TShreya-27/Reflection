using Microsoft.Data.Sqlite;

/*
This class manages the SQLite database.

Each row represents one completed
activity session.
*/
public class Database
{
    /*
    Location of the SQLite database.
    */
    private readonly string connectionString =
        @"Data Source=D:\reflection\activity.db";

    // Creates the database table if it does not already exist.
    public void Initialize()
    {
        using var connection =
            new SqliteConnection(connectionString);

        connection.Open();

        using var command =
            connection.CreateCommand();

        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Activity
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                DurationSeconds REAL NOT NULL,
                Application TEXT,
                WindowTitle TEXT,
                ProcessId INTEGER
            );
            """;

        command.ExecuteNonQuery();
    }

    /*
    Inserts one completed activity session.
    */
    public void Insert(ActivityRecord activity)
    {
        using var connection =
            new SqliteConnection(connectionString);

        connection.Open();

        using var command =
            connection.CreateCommand();

        /*
        SQL statement for inserting one
        completed activity session.
        */
        command.CommandText = """
            INSERT INTO Activity
            (
                StartTime,
                EndTime,
                DurationSeconds,
                Application,
                WindowTitle,
                ProcessId
            )
            VALUES
            (
                $startTime,
                $endTime,
                $durationSeconds,
                $application,
                $windowTitle,
                $processId
            );
            """;

        /*
        Start time.
        */
        command.Parameters.AddWithValue(
            "$startTime",
            activity.StartTime.ToString("O")
        );

        /*
        End time.
        */
        command.Parameters.AddWithValue(
            "$endTime",
            activity.EndTime.ToString("O")
        );

        /*
        Duration of the activity in seconds.
        */
        command.Parameters.AddWithValue(
            "$durationSeconds",
            activity.DurationSeconds
        );

        /*
        Application name.
        */
        command.Parameters.AddWithValue(
            "$application",
            activity.Application
        );

        /*
        Active window title.
        */
        command.Parameters.AddWithValue(
            "$windowTitle",
            activity.WindowTitle
        );

        /*
        Windows Process ID.
        */
        command.Parameters.AddWithValue(
            "$processId",
            activity.ProcessId
        );

        /*
        Execute the INSERT operation.
        */
        command.ExecuteNonQuery();
    }
}