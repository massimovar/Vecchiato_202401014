#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using System.IO;
#endregion

public class LogManager : BaseNetLogic
{
    private PeriodicTask periodicTask;

    public override void Start()
    {
        periodicTask = new PeriodicTask(ReadLogFile, 1000, LogicObject);
        //periodicTask.Start();
    }

    public override void Stop()
    {
        periodicTask.Dispose();
    }

    [ExportMethod]
    public void ReadLogFile()
    {
        int lastRows = 10;
        var logPath = new ResourceUri(LogicObject.GetVariable("LogPath").Value).Uri;
        var logContent = LogicObject.GetVariable("LogContent");
        logContent.Value = string.Empty;

        // Read all lines from the file into a List<string>
        string[] lines = File.ReadAllLines(logPath);

        // Check if there are less than 10 rows in the file
        if (lines.Length <= lastRows)
        {
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                logContent.Value += "\n" + lines[i];
            }
        }
         
        else
        {
            // Print the last lastRows rows
            for (int i = lines.Length - 1; i >= lines.Length - lastRows; i--)
            {
                logContent.Value += "\n" +  lines[i];
            }
        }

    }
}
