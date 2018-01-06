using System.Collections;
using System.Reflection;
using UnityEngine;


/// <summary>
/// Объект, который реагирует на нажатие
/// </summary>
public class InteractableObject : MonoBehaviour
{
    public bool interactable;
    public Object funcObj;
    protected bool pressed, pressedNow;
    public string onButtonUp;
    public string onButtonDown;
    public string onButtonPress;
    public string onButtonCancel;



    /// <summary>
    /// Нажатие
    /// </summary>
    public virtual void Up()
    {
        pressed = false;
        if (!interactable || funcObj == null) return;
        MethodInfo mi = funcObj.GetType().GetMethod(onButtonUp);
        
        if(mi!=null)
        {
            mi.Invoke(funcObj, null);
        }
    }

    /// <summary>
    /// Наведение
    /// </summary>
    public virtual void Down()
    {
        if (!interactable || funcObj == null) return;
        MethodInfo mi = funcObj.GetType().GetMethod(onButtonDown);
        Debug.Log("1");
        if (mi != null)
        {
            Debug.Log("2");
            mi.Invoke(funcObj, null);
        }
    }

    public virtual void Cancel()
    {
        if (!interactable || funcObj == null) return;
        MethodInfo mi = funcObj.GetType().GetMethod(onButtonPress);
        if (mi != null)
        {
            mi.Invoke(funcObj, null);
        }
    }

    public virtual void On()
    {
        if (!interactable || funcObj == null) return;
        MethodInfo mi = funcObj.GetType().GetMethod(onButtonPress);
        if (mi != null)
        {
            mi.Invoke(funcObj, null);
        }
    }

    public void Press()
    {
        pressedNow = true;
    }


    public virtual void Update()
    {
        if (pressedNow && !pressed)
        {
            Down();
        }
        if (!pressedNow && pressed)
            Cancel();
        if (pressedNow && pressed)
            On();
        pressed = pressedNow;
        pressedNow = false;
    }

}