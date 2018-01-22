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
    /// Отжатиe
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
    /// Нажатие
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

    /// <summary>
    /// Отмена нажатия
    /// </summary>
    public virtual void Cancel()
    {
        if (!interactable || funcObj == null) return;
        MethodInfo mi = funcObj.GetType().GetMethod(onButtonPress);
        if (mi != null)
        {
            mi.Invoke(funcObj, null);
        }
    }

    /// <summary>
    /// Нажатие и задержание
    /// </summary>
    public virtual void On()
    {
        if (!interactable || funcObj == null) return;
        MethodInfo mi = funcObj.GetType().GetMethod(onButtonPress);
        if (mi != null)
        {
            mi.Invoke(funcObj, null);
        }
    }


    /// <summary>
    /// Нажимаем на кнопку
    /// </summary>
    public void Press()
    {
        pressedNow = true;
    }



    public virtual void Update()
    {
        if (pressedNow && !pressed)//вервый кадр нажатия
        {
            Down();
        }
        if (!pressedNow && pressed)//если во втором кадре она отжата то отменяем нажатие
            Cancel();
        if (pressedNow && pressed)//кнопка задержана
            On();
        pressed = pressedNow;
        pressedNow = false;
    }

}