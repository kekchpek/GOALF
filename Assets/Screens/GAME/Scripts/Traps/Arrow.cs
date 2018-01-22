using UnityEngine;


class Arrow : Trap
{
    /// <summary>
    /// Вызывается при соприкосновении ловушки с посторонним объектом
    /// </summary>
    /// <param name="other">коллайдер постороннего объекта</param>
    protected override void EffectFunction(Collider2D other)
    {
        base.EffectFunction(other);
        other.GetComponent<Rigidbody2D>().velocity = transform.right * 5;
    }
}