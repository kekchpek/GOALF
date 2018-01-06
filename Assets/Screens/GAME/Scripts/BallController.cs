using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour {

    public Rigidbody2D rb;//риджитбади кубика
    public Vector2 startPoint, endPoint;//точка откуда происходит "натягивание" и где сейчас палец(пиксели)
    public Transform triangle;//треугольный прицел
    public MyAnimator goalfer;//аниматор гольфера
    public MyAnimator fly;//аниматор огня под ногами
    public BallInputController bic;//контроллер удара кубика
    public GameScreenController controller;//контроллер экрана игры
    public float rotateForce;//сила вращения кубика при ударе
    private bool stopFlag;//бит сигнализирующий об остановке кубика
    private bool activeFlag;//кубик остановился и готов быть удареным
    private bool hitFlag;//кубик бьют, но ещё не ударили
    public AudioClip[] sounds;

    void OnCollisionEnter2D(Collision2D other)
    {
        SoundController.PlaySound(sounds[Random.Range(0, sounds.Length)]);
    }

    public void Hit()//начали бить по кубику
    {
        if (activeFlag)
        {
            if (startPoint == Vector2.zero || startPoint == endPoint || startPoint.y < endPoint.y) return;
            goalfer.PlayAnimation("Hit", 0.2f, false);
            bic.interactable = false;
            hitFlag = true;
            controller.Hit();
            StartCoroutine(HitNow());
        }
    }

    IEnumerator HitNow()
    {
        yield return new WaitForSeconds(0.1f);//ударили!!!
        SoundController.PlaySound("hit");
        Vector2 f = 15 * (startPoint - endPoint) / (0.17f * (Screen.height + Screen.width));
        if(f.magnitude >15)
        {
            f = f.normalized * 15;
        }
        rb.velocity = f;
        hitFlag = false;
        startPoint = Vector2.zero;
        yield return new WaitForSeconds(0.2f);//гольфер исчез
        rb.angularVelocity = rotateForce;
        fly.gameObject.SetActive(false);
        goalfer.PlayAnimation("Fade", 0.4f, false);
    }

    IEnumerator SetInteractable(bool b)
    {
        yield return new WaitForSeconds(0.3f);
        bic.interactable = true;
    }

    void Update()
    {
        if (!controller.pauseFlag)
        {
            if (rb.velocity.magnitude > 0.01f)
            {
                activeFlag = false;
                stopFlag = false;
                controller.SetMove(true);
                bic.interactable = false;
            }
            else
            {
                if (stopFlag)
                {
                    if (!activeFlag && !hitFlag)
                    {
                        controller.SetLoseZone();
                        controller.SetMove(false);
                        goalfer.transform.parent.transform.position = transform.position + Vector3.left * -0.5f + Vector3.down * 0.1f;
                        goalfer.transform.parent.localScale = new Vector3(-0.5f, 0.5f, 1);
                        SoundController.PlaySound("spawn");
                        goalfer.PlayAnimation("Unfade", 0.3f, false);
                        fly.gameObject.SetActive(false);
                        StartCoroutine(SetInteractable(true));
                        activeFlag = true;
                    }
                }
                else
                    stopFlag = true;
            }
            float k = 0;
            if (startPoint != Vector2.zero)
            {
                if (endPoint.y < startPoint.y)
                {
                    k = 5f * (startPoint - endPoint).magnitude / (0.17f * (Screen.height + Screen.width));
                    if (k > 5f)
                        k = 5f;
                }
            }
            if (k > 0)
            {
                SoundController.PlaySound("aim", 0.5f, true, "aim");
                float x, y;
                x = startPoint.x - endPoint.x;
                y = startPoint.y - endPoint.y;
                if ((triangle.eulerAngles = new Vector3(0, 0, -Mathf.Atan2(x, y) * 180 / Mathf.PI)).z > 0)
                {
                    goalfer.transform.parent.transform.position = transform.position + Vector3.left * -0.7f + Vector3.down * 0.1f;
                    goalfer.transform.parent.localScale = new Vector3(-0.5f, 0.5f, 1);
                }
                else
                {
                    goalfer.transform.parent.transform.position = transform.position + Vector3.left * 0.7f + Vector3.down * 0.1f;
                    goalfer.transform.parent.localScale = new Vector3(0.5f, 0.5f, 1);
                }
            }
            else
            {
                SoundController.StopSound("aim");
            }
            if (bic.interactable)
                goalfer.FixAnimation(k, 5f, "Prepare");
            triangle.localScale = new Vector2(triangle.transform.localScale.x, triangle.transform.localScale.x * k);
        }
    }


}
