using System;
using System.Runtime.InteropServices;
using System.Text;

/*
This class contains declarations for native Windows API
functions provided by user32.dll.

P/Invoke allows the C# application to call these
Windows functions.
*/
public static class NativeMethods
{
    // Gets the handle of the currently active window.
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr GetForegroundWindow();

    /*
    Gets the title/text of the specified window.

    hWnd:
    Handle of the window.

    lpString:
    Stores the retrieved window title.

    nMaxCount:
    Maximum number of characters to retrieve.
    */
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int GetWindowText(
        IntPtr hWnd,
        StringBuilder lpString,
        int nMaxCount
    );

    /*
    Gets the Process ID associated with the specified window.

    hWnd:
    Handle of the window.

    processId:
    Output variable containing the Process ID.
    */
    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(
        IntPtr hWnd,
        out uint processId
    );
}