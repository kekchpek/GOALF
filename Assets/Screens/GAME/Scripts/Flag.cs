using System.Collections;
using UnityEngine;

public class Flag : MonoBehaviour {

    public GameScreenController controller;
    public MyAnimator anim;
    public Collider2D trigger;
    public bool holyOne;
    private SpriteRenderer sr;

    string[] anims = { "", "birdy", "par", "booger" };

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<BallController>() != null)
        {
            sr = GetComponent<SpriteRenderer>();
            trigger.enabled = false;
            int n = controller.GetFlag();
            if (n > 4)
                n = 4;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, other.gameObject.GetComponent<Rigidbody2D>().velocity.y);
            if (n == 1)
            {
                holyOne = true;
            }
            else
            {
                anim.PlayAnimation(anims[n-1], 0.5f, false);
            }
            StartCoroutine(Destr());
        }
    }

    void Update()
    {
        if(holyOne)
        {
            transform.position += Vector3.up * 1f * Time.deltaTime;
            sr.color = sr.color + new Color(0, 0, 0, -2 * Time.deltaTime);
        }
    }

    IEnumerator Destr()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.parent.gameObject);
    }



}
