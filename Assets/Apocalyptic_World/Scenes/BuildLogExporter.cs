using System;
using System.IO;
using UnityEngine;

public class BuildLogExporter : MonoBehaviour
{
    // Đường dẫn bạn muốn copy log về
    private string exportPath = @"C:\GameBuild\debug_log.txt";

    void Start()
    {
        // Khi game bắt đầu, đăng ký callback ghi log
        Application.logMessageReceived += HandleLog;
    }

    void OnDestroy()
    {
        // Hủy đăng ký để tránh memory leak
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        try
        {
            string msg = $"[{DateTime.Now:HH:mm:ss}] [{type}] {logString}";
            if (type == LogType.Exception || type == LogType.Error)
                msg += $"\n{stackTrace}";

            File.AppendAllText(exportPath, msg + Environment.NewLine);
        }
        catch (Exception e)
        {
            Debug.LogWarning("LogExporter error: " + e.Message);
        }
    }
}
