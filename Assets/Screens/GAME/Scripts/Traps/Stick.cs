
using UnityEngine;

public class Stick : Trap {

    bool active = true;//активна ли ловушка
    public Rigidbody2D rb;//риджитбоди ловушки
    public SpriteRenderer sr;
    public float angleSpeed;//скорость с которой вращается ловушка при активации
   

    protected override void EffectFunction(Collider2D other)
    {
        if (active)
        {
            base.EffectFunction(other);
            rb.angularVelocity = angleSpeed;
            active = false;
            Color c = sr.color;
            c.a = 1;
            sr.color = c;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (rb.transform.eulerAngles.z >= 180)//если провернулась на 180 градусов
        {
            //останавливаем и возвращием в исходное состояние
            active = true;
            rb.angularVelocity = 0;
            Color c = sr.color;
            c.a = 0.3f;
            sr.color = c;
            rb.transform.eulerAngles = Vector3.zero;
        }
    }

}
