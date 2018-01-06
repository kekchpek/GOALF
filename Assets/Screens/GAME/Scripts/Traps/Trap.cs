using UnityEngine;
using UnityEngine.UI;

abstract public class Trap : MonoBehaviour
{

    public float rotateSpeed;
    public bool fading;
    protected Transform ball;
    protected Image[] imgs;

    protected virtual void Start()
    {
        imgs = GetComponentsInChildren<Image>();
        ball = ((GoalfMainController)MainController.controller).ball.transform;
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            rotateSpeed *= -1;
        }
    }

    protected virtual void EffectFunction()
    {
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        EffectFunction();
    }

    float alpha;
    Color c;
    protected virtual void Update()
    {
        transform.Rotate(0, 0, rotateSpeed);
        if (fading)
        {
            alpha = Vector3.Distance(transform.position, ball.transform.position) * (-1.56f) + 1.66f;
            if (alpha > 1) alpha = 1;
            if (alpha < 0.1f) alpha = 0.1f;
            foreach (Image img in imgs)
            {
                c = img.color;
                c.a = alpha;
                img.color = c;
            }
        }
    }
}