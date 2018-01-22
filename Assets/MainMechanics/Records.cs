
using UnityEngine;

/// <summary>
/// Класс для записи рекордов
/// </summary>
public static class Records {

    static string key = "record";

    /// <summary>
    /// Получить рекорд
    /// </summary>
    /// <returns>Возвращает рекорд</returns>
    static public int GetRecord()
    {
        return PlayerPrefs.GetInt(key, 0);
    }

    /// <summary>
    /// Попытаться записать рекорд
    /// </summary>
    /// <param name="p">рекорд</param>
    /// <returns>true - если переданный рекорд больше текущего(будет записан)\nfalse - если переданный рекорд меньше или равен текущему(не будет записан)</returns>
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
