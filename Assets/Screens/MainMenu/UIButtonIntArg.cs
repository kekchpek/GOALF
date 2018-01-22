using UnityEngine;
using System.Reflection;

/// <summary>
/// Тот же UIButton только с int аргументом у функции Up
/// </summary>
public class UIButtonIntArg : UIButton {

    public int arg;

    public override void Up()
    {
        touchX = (Input.GetTouch(0).position.x - Screen.width * rt.anchorMin.x) / (Screen.width * (rt.anchorMax.x - rt.anchorMin.x));
        touchY = (Input.GetTouch(0).position.y - Screen.height * rt.anchorMin.y) / (Screen.height * (rt.anchorMax.y - rt.anchorMin.y));
        pressed = false;
        if (!interactable || funcObj == null) return;
        MethodInfo mi = funcObj.GetType().GetMethod(onButtonUp);
        object[] args = new object[1];
        args[0] = arg;
        if (mi != null)
        {
            mi.Invoke(funcObj, args);
        }
    }
}
