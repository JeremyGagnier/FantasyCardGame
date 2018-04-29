using System;
using System.IO;

public class FileManager
{
    private static string APP_DIRECTORY = "CardGame/";
    private static string _appDataPath = null;
    public static string appDataPath
    {
        get
        {
            if (_appDataPath == null)
            {
                _appDataPath =
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + APP_DIRECTORY;
            }
            return _appDataPath;
        }
    }
}
