
using UnityEngine;

/// <summary>
/// Кнопка
/// </summary>
public class UIButton : InteractableObject {

    protected float touchX = 0.5f, touchY = 0.5f;//координаты нажания на кнопку

    protected RectTransform rt;//трансформ кнопки

    public bool sound;//есть ли звук у кнопки

    public AudioClip[] sounds;//возможные звуки кнопки

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        float x, y;
        //настраиваем коллайдер
        //-----------------------------------------------------------------------------------------
        if (rt.anchorMax.x == rt.anchorMin.x)
        {
            x = rt.sizeDelta.x;
        }
        else
        {
            x = (rt.anchorMax.x - rt.anchorMin.x) * Screen.width;
        }
        if(rt.anchorMax.y == rt.anchorMin.y)
        {
            y = rt.sizeDelta.y;
        }
        else
        {
            y = (rt.anchorMax.y - rt.anchorMin.y) * Screen.height;
        }
        gameObject.AddComponent<BoxCollider>().size = new Vector3(x, y, 1);
        //------------------------------------------------------------------------------------------
    }

    /// <summary>
    /// Вызывется каждый кадр если кнопка зажата
    /// </summary>
    public override void On()
    {
        touchX = (Input.GetTouch(0).position.x - Screen.width * rt.anchorMin.x) / (Screen.width * (rt.anchorMax.x - rt.anchorMin.x));
        touchY = (Input.GetTouch(0).position.y - Screen.height * rt.anchorMin.y) / (Screen.height * (rt.anchorMax.y - rt.anchorMin.y));
        base.On();
    }

    /// <summary>
    /// Вызывается при нажатии на кнопку
    /// </summary>
    public override void Down()
    {
        if(sound)
        {
            SoundController.PlaySound(sounds[Random.Range(0, sounds.Length)]);
        }
        touchX = (Input.GetTouch(0).position.x - Screen.width * rt.anchorMin.x) / (Screen.width * (rt.anchorMax.x - rt.anchorMin.x));
        touchY = (Input.GetTouch(0).position.y - Screen.height * rt.anchorMin.y) / (Screen.height * (rt.anchorMax.y - rt.anchorMin.y));
        base.Down();
    }

    /// <summary>
    /// Вызывается в момень отжатия кнопки
    /// </summary>
    public override void Up()
    {
        touchX = (Input.GetTouch(0).position.x - Screen.width * rt.anchorMin.x) / (Screen.width * (rt.anchorMax.x - rt.anchorMin.x));
        touchY = (Input.GetTouch(0).position.y - Screen.height * rt.anchorMin.y) / (Screen.height * (rt.anchorMax.y - rt.anchorMin.y));
        base.Up();
    }

}
