using System;
using System.Runtime.InteropServices;
using System.Threading;

public class SleepMode
{
    [DllImport("powrprof.dll", SetLastError = true)]
    private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

    private const int DelayMinutes = 26;

    public static void Main(string[] args)
    {
        // Subscribe to the system events
        Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

        // Start the timer for initial execution after 27 minutes
        Timer timer = new Timer(ExecuteScript, null, TimeSpan.FromMinutes(DelayMinutes), Timeout.InfiniteTimeSpan);

        // Wait for user input to exit
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();

        // Clean up and unsubscribe from events
        timer.Dispose();
        Microsoft.Win32.SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
    }

    private static void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
    {
        if (e.Mode == Microsoft.Win32.PowerModes.Resume)
        {
            // Start the timer after system wake-up
            Timer timer = new Timer(ExecuteScript, null, TimeSpan.FromMinutes(DelayMinutes), Timeout.InfiniteTimeSpan);
        }
    }

    private static void ExecuteScript(object state)
    {
        // Put the system to sleep
        SetSuspendState(false, true, true);

        // Wait for 5 seconds
        Thread.Sleep(5000);

        // Perform any desired operations after waking up
        Console.WriteLine("System woke up!");

        // Add your additional code here
    }
}