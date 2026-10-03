using System;
using System.Diagnostics;
using System.Text;

// This class collects information about the currently active Windows window.
public class ActivityCollector
{
    // Gets the currently active activity.
    public ActivityRecord GetCurrentActivity()
    {
        IntPtr windowHandle =
            NativeMethods.GetForegroundWindow();    
        
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime; // For now, we set endTime to startTime. You can update this later as needed.
        double durationSeconds = 0.0;   
        StringBuilder windowTitle =
            new StringBuilder(512);

        NativeMethods.GetWindowText(
            windowHandle,
            windowTitle,
            windowTitle.Capacity
        );

        NativeMethods.GetWindowThreadProcessId(
            windowHandle,
            out uint processId
        );

        string processName = "Unknown";

        try
        {
            using Process process =
                Process.GetProcessById((int)processId);

            processName = process.ProcessName;
        }
        catch
        {
            //The process may have terminated between API calls.
        }

        return new ActivityRecord
        {
            Application = processName,
            WindowTitle = windowTitle.ToString(),
            ProcessId = (int)processId
        };
    }
}