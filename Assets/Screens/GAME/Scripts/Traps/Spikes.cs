using System.Collections;
using UnityEngine;

public class Spikes : Trap {

    public Rigidbody2D rb;//риджитбоди пули
    public float speed;//скорость с которой выстрелит пуля
    bool active = true;//активна ли ловушка

    protected override void EffectFunction(Collider2D other)
    {
        if (active)
        {
            base.EffectFunction(other);
            active = false;
            rb.simulated = true;
            rb.velocity = Vector3.right * speed * Mathf.Sign(transform.localScale.x);//стреляем
            StartCoroutine(Remove());
        }
    }


    IEnumerator Remove()//удаляем пулю
    {
        yield return new WaitForSeconds(1000);//через 1000 сек она уже будет далеко внизу
        Destroy(gameObject);
    }
}
