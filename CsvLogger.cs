using System;
using System.IO;

/*
This class handles CSV file logging.
*/
public static class CsvLogger
{
    /*
    The activity.csv file will be published in the current directory.
    */
    private static readonly string FilePath =
        Path.Combine(
            Directory.GetCurrentDirectory(),
            "activity.csv"
        );

    /*
    Creates the CSV file and header
    if it does not already exist.
    */
    public static void Initialize()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(
                FilePath,
                "StartTime,EndTime,DurationSeconds,Application,WindowTitle,ProcessId"
                + Environment.NewLine
            );
        }
    }

    /*
    Writes one activity record to the CSV file.
    */
    public static void Write(ActivityRecord activity)
    {
        /*
        Escape values so that commas and
        quotation marks do not break the CSV format.
        */
        string application =
            Escape(activity.Application);

        string windowTitle =
            Escape(activity.WindowTitle);

        /*
        Create one CSV row.
        */
        string line =
            $"{activity.StartTime:O}," +
            $"{activity.EndTime:O}," +
            $"{activity.DurationSeconds:F0}," +
            $"{application}," +
            $"{windowTitle}," +
            $"{activity.ProcessId}" +
            Environment.NewLine;
        // Add the row to the CSV file.
        File.AppendAllText(FilePath, line);
    }

    /*
    Escapes quotation marks for valid CSV formatting.
    */
    private static string Escape(string value)
    {
        value = value.Replace("\"", "\"\"");

        return $"\"{value}\"";
    }
}