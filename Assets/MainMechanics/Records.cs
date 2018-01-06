using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Records {

    static string key = "record";

    static public int GetRecord()
    {
        return PlayerPrefs.GetInt(key, 0);
    }
	
    static public bool SetRecord(int p)
    {
        if(PlayerPrefs.GetInt(key, 0)<p)
        {
            PlayerPrefs.SetInt(key, p);
            return true;
        }
        return false;
    }
}
