using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Объект, который реагирует на нажатие
/// </summary>
public class InteractableObject : MonoBehaviour
{
    public bool enable;
    public bool interactable;
    public bool interacted;
    public bool preInteracted;
    private bool preInteractedFlag;
    public delegate void ButtonAction();
    public ButtonAction onButtonUp;
    public ButtonAction onButtonDown;
    public ButtonAction onButtonDownEnd;



    /// <summary>
    /// Нажатие
    /// </summary>
    public virtual void Interact()
    {
        if (interactable)
            interacted = true;
        onButtonUp();
    }

    /// <summary>
    /// Наведение
    /// </summary>
    public virtual void PreInteract()
    {
        preInteracted = true;
        StartCoroutine(ButtonUp());
    }

    IEnumerator ButtonUp()
    { 
        yield return null;
        preInteracted = false;
    }

    public virtual void Update()
    {
        if(preInteractedFlag && !preInteracted)
        {
            onButtonDownEnd();
        }
        if(!preInteractedFlag && preInteracted)
        {
            onButtonDown();
        }
        preInteractedFlag = preInteracted;
    }

}