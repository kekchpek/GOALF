using UnityEngine;
using System.Collections;

public class Bomb : Trap {

    public Rigidbody2D[] bullets;//риджитбоди снарядов
    bool active = true;//активна ли ловушка

    protected override void EffectFunction(Collider2D other)
    {
        if (active)
        {
            base.EffectFunction(other);
            Vector3 force;//вектор силы
            fading = false;//убираем динамическую прозрачность
            foreach (Rigidbody2D t in bullets)//раскидываем пули в разные стороны
            {
                t.simulated = true;
                force = (t.transform.position - transform.position).normalized * 5;
                t.velocity = force;
            }
            StartCoroutine(Remove());
        }
    }

    IEnumerator Remove()//удаляем ловушку
    {
        yield return new WaitForSeconds(1000);//через 1000 секунд точно осколки уже будут далеко внизу
        Destroy(gameObject);
    }

}
