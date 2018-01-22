using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : Trap {

    bool active = true;//активна ли ловушка
    public Rigidbody2D piston;//риджитбоди снаряда
    public float speed;//скорость с которой выстреливает пистон

    protected override void EffectFunction(Collider2D other)
    {
        if (active)
        {
            base.EffectFunction(other);
            Color c = piston.GetComponent<SpriteRenderer>().color;
            c.a = 1;
            piston.GetComponent<SpriteRenderer>().color = c;
            piston.velocity = Vector3.right * speed * Mathf.Sign(transform.lossyScale.x);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (piston.transform.localPosition.x > 0)//если пистон достаточно вылетел останавливаем его
        {
            piston.transform.localPosition = Vector3.zero;
            piston.velocity = Vector3.zero;
        }
    }

}
