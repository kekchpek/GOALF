using UnityEngine;
using UnityEngine.UI;

public class Trap : MonoBehaviour
{

    public float rotateSpeed;
    public bool fading;
    public Transform ball;
    protected SpriteRenderer[] imgs;
    public AudioClip clip;

    protected virtual void Start()
    {
        imgs = GetComponentsInChildren<SpriteRenderer>();//получем спрайты для динамической прозрачности
        if (ball == null)
            ball = ((GoalfMainController)MainController.controller).ball.transform;//получаем шар для проверки расстояния до ловушки
        if (UnityEngine.Random.Range(0, 2) == 0)//задаём вращение в случайную сторону
        {
            rotateSpeed *= -1;
        }
    }

    /// <summary>
    /// Действия при активации ловушки
    /// </summary>
    /// <param name="other"></param>
    protected virtual void EffectFunction(Collider2D other)
    {
        SoundController.PlaySound(clip);//проигрываем звук
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")//если объект коллизии наш мячик
            EffectFunction(other);
    }

    float alpha;
    Color c;
    protected virtual void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        if (fading)//динамическая прозрачность
        {
            //чем дальше мячик тем сильнее прозрачность
            alpha = 1.5f - Vector3.Distance(transform.position, ball.transform.position);
            if (alpha > 1) alpha = 1;
            if (alpha < 0.1f) alpha = 0.1f;
            foreach (SpriteRenderer img in imgs)
            {
                c = img.color;
                c.a = alpha;
                img.color = c;
            }
        }
    }
}