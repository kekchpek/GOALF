using System.Collections.Generic;
using UnityEngine;

public class MyAnimator : MonoBehaviour {
    [SerializeField]
    public List<string> animNames;
    public List<MyAnimation> anims;
    private Dictionary<string, MyAnimation> animations;
    public string  currentAnimation;
    private float currentTime;
    private SpriteRenderer spriteRenderer;

    public delegate void VoidDelegate();
    VoidDelegate OnAnimationPlayed;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTime = 0;
        animations = new Dictionary<string, MyAnimation>();
        for(int i = 0; i < animNames.Count; i++)
        {
            animations.Add(animNames[i], anims[i]);
        }
	}
	

	void Update () {
        if (currentAnimation != "")
        {
            if (!animations.ContainsKey(currentAnimation))
            {
                currentAnimation = "";
            }
            else
            {
                currentTime += Time.deltaTime;
                if (currentTime >= animations[currentAnimation].playTime)
                {
                    AnimationPlayed();
                    currentTime -= animations[currentAnimation].playTime;
                }
                spriteRenderer.sprite = animations[currentAnimation].GetCurrentSprite(currentTime);
            }
        }
	}

    void AnimationPlayed()
    {
        if(OnAnimationPlayed != null)
            OnAnimationPlayed();
    }

}
