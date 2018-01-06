using UnityEngine;

public class BallInputController : UIButton {

    public BallController ball;

    public override void Down()
    {
        base.Down();
        if (!interactable) return;
        ball.startPoint = ball.endPoint = Input.GetTouch(0).position;
    }

    public override void Cancel()
    {
        base.Cancel();
        if (!interactable) return;
        ball.startPoint = Vector2.zero;
    }

    public override void Up()
    {
        base.Up();
        if (!interactable) return;
        ball.Hit();
    }

    public override void On()
    {
        base.On();
        if (!interactable) return;
        ball.endPoint = Input.GetTouch(0).position;
    }
}
