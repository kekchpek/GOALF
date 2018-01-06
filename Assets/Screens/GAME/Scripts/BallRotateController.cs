using UnityEngine;

public class BallRotateController : UIButton {

    public BallController ball;
    public RectTransform rotateSprite;
    public float rotateForce;

    protected void SetRotateForce()
    {
        rotateForce = ball.rotateForce = -(touchX - 0.5f) * 720 * 2.5f;
        if (Mathf.Abs(touchX - 0.5f) <= 0.05f)
            rotateForce = ball.rotateForce = 0;

    }

    public override void Update()
    {
        base.Update();
        rotateSprite.eulerAngles += new Vector3(0, 0, rotateForce * Time.deltaTime);
        rotateSprite.anchorMin = rotateSprite.anchorMax = new Vector2(touchX, 0.5f);
        if(rotateForce == 0)
        {
            rotateSprite.anchorMin = rotateSprite.anchorMax = new Vector2(0.5f, 0.5f);
            rotateSprite.eulerAngles = Vector3.zero;
        }
        rotateSprite.anchoredPosition = Vector3.zero;
    }

    public override void Down()
    {
        base.Down();
        SetRotateForce();
    }

    public override void On()
    {
        base.On();
        SetRotateForce();
    }

    public override void Up()
    {
        base.Up();
        SetRotateForce();
    }

}
