using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public static class LocalizationManager
{
    public static string GetLocalizedString(string key)
    {
        string text = LocalizationSettings.StringDatabase.GetLocalizedString(key);
        return text;    
    }
}

