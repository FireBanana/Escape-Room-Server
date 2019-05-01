using System.Collections.Generic;
using System.Windows;

public static class WindowManager
{
    static Dictionary<string, Window> WindowUsageDictionary = new Dictionary<string, Window>();

    public static bool IsWindowOpen(string teamName)
    {
        if (WindowUsageDictionary.ContainsKey(teamName))
        {
            return true;
        }

        return false;
    }

    public static void SetWindowOpenState(string teamName, bool isUsed, Window window)
    {
        if (isUsed && !WindowUsageDictionary.ContainsKey(teamName))
        {
            WindowUsageDictionary.Add(teamName, window);
        }
        else if(!isUsed && WindowUsageDictionary.ContainsKey(teamName))
        {
            WindowUsageDictionary.Remove(teamName);
        }
    }

    public static Window GetWindow(string name)
    {
        if(WindowUsageDictionary.ContainsKey(name))
        {
            return WindowUsageDictionary[name];
        }

        return null;
    }
}

