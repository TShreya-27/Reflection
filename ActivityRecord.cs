/*
This class represents one activity record.
It stores the information collected from the currently active Windows window.
*/
using System;

// This class represents one completed user activity session.
public class ActivityRecord
{
    // Time when the activity started.
    public DateTime StartTime { get; set; }

    // Time when the activity ended.
    public DateTime EndTime { get; set; }

    // Total duration of the activity in seconds.
    public double DurationSeconds { get; set; }

    // Name of the active application.
    public string Application { get; set; } = "";

    // Title of the active window.
    public string WindowTitle { get; set; } = "";

    //  Windows Process ID.
    public int ProcessId { get; set; }
}