using System.Collections.Generic;
using UnityEngine;

public class MyAnimator : MonoBehaviour {
    [SerializeField]
    public List<string> animNames;
    public List<MyAnimation> anims;
    private Dictionary<string, MyAnimation> animations;
    public string  currentAnimation;
    public float currentTime;
    public bool loop;
    public bool playAnimation;
    private SpriteRenderer spriteRenderer;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTime = 0;
        animations = new Dictionary<string, MyAnimation>();
        for(int i = 0; i < animNames.Count; i++)
        {
            animations.Add(animNames[i], anims[i]);
        }
	}

    public void PlayAnimation(string aName, float time, bool l)
    {

        if (animations.ContainsKey(aName))
        {
            loop = l;
            currentAnimation = aName;
            currentTime = 0f;
            animations[currentAnimation].playTime = time;
            playAnimation = true;
        }
    }

    public void FixAnimation(float t, float t2, string aName)
    {
        if (animations.ContainsKey(aName))
        {
            playAnimation = false;
            loop = false;
            currentAnimation = aName;
            animations[currentAnimation].playTime = t2;
            spriteRenderer.sprite = animations[currentAnimation].GetCurrentSprite(t);
        }
    }

	void Update () {
        if (playAnimation)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= animations[currentAnimation].playTime)
            {
                if (loop)
                {
                    currentTime -= animations[currentAnimation].playTime;
                }
                else
                {
                    playAnimation = false;
                }
            }
            spriteRenderer.sprite = animations[currentAnimation].GetCurrentSprite(currentTime);
        }
	}

}
