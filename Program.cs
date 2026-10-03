/*
Main entry point of the Activity Tracker PoC.

The program checks the active Windows activity
every second, but only saves a record when
the activity changes.
*/

class Program
{
    static void Main()
    {
        Console.WriteLine(
            "---------------------------------------------------"
        );

        Console.WriteLine(
            "        Activity Tracker - PoC"
        );

        Console.WriteLine(
            "---------------------------------------------------"
        );

        Console.WriteLine();

        /*
        Create the activity collector.
        */
        ActivityCollector collector =
            new ActivityCollector();

        /*
        Create the SQLite database handler.
        */
        Database database =
            new Database();

        /*
        Initialize the database.
        */
        database.Initialize();

        /*
        Initialize the CSV file.
        */
        CsvLogger.Initialize();

        Console.WriteLine(
            "Activity tracking started..."
        );

        Console.WriteLine(
            "Activity is saved only when it changes."
        );

        Console.WriteLine(
            "Press Ctrl+C to stop."
        );

        Console.WriteLine();

        /*
        Print the table header.
        */
        Console.WriteLine(
            "=============================================================================================================="
        );

        Console.WriteLine(
            $"{"Start Time",-18}" +
            $"{"End Time",-18}" +
            $"{"Application",-15}" +
            $"{"Window Title",-40}" +
            $"{"Duration",-12}"
        );

        Console.WriteLine(
            "=============================================================================================================="
        );

        /*
        Stores the previously detected activity.
        */
        ActivityRecord? previousActivity = null;

        /*
        Stores when the current activity started.
        */
        DateTime activityStartTime = DateTime.Now;

        /*
        Continuously monitor the active window.
        */
        while (true)
        {
            try
            {
                
                //Get the currently active activity.
                
                ActivityRecord currentActivity =
                    collector.GetCurrentActivity();

            
                // If this is the first activity, start tracking it.
             
                if (previousActivity == null)
                {
                    previousActivity =
                        currentActivity;

                    activityStartTime =
                        DateTime.Now;
                }
                else
                {
                    // Check whether the activity changed.
                    
                    bool activityChanged =
                        currentActivity.Application != previousActivity.Application || currentActivity.WindowTitle != previousActivity.WindowTitle;
                 
                    // If the activity changed, complete the previous session.
                    
                    if (activityChanged)
                    {
                        
                        // Record when the previous activity ended.
                        
                        DateTime activityEndTime =
                            DateTime.Now;

                        // Calculate the duration.
                        
                        TimeSpan duration =
                            activityEndTime -
                            activityStartTime;
                        
                        // Complete the previous record.
                        
                        previousActivity.StartTime =
                            activityStartTime;

                        previousActivity.EndTime =
                            activityEndTime;

                        previousActivity.DurationSeconds =
                            duration.TotalSeconds;

                        // Save the completed activity to SQLite.
                        
                        database.Insert(
                            previousActivity
                        );

                        // Save the completed activity to CSV.
                        
                        CsvLogger.Write(
                            previousActivity
                        );
                        
                        // Display the completed activity as one row in the terminal.
                        
                        Console.WriteLine(
                            $"{previousActivity.StartTime:HH:mm:ss}   " +
                            $"{previousActivity.EndTime:HH:mm:ss}   " +
                            $"{previousActivity.Application,-15} " +
                            $"{Truncate(
                                previousActivity.WindowTitle,
                                38),-40} " +
                            $"{duration:hh\\:mm\\:ss}"
                        );

                        // The newly detected activity becomes the current activity.
                        previousActivity =
                            currentActivity;
                    
                        // Start timing the new activity.
                       
                        activityStartTime =
                            DateTime.Now;
                    }
                }

                /*
                Wait one second before checking again.

                This is only for detecting activity changes.
                It does not create a database record.
                */
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                // Display errors without terminating the activity tracker.
                Console.WriteLine(
                    $"Error: {ex.Message}"
                );

                Thread.Sleep(1000);
            }
        }
    }

    // Prevent very long window titles from  destroying the table formatting.
    static string Truncate(
        string value,
        int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }

        if (value.Length <= maxLength)
        {
            return value;
        }

        return value.Substring(
            0,
            maxLength - 3
        ) + "...";
    }
}