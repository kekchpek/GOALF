using System.Collections.Generic;
using UnityEngine;

public class MyAnimator : MonoBehaviour {

    [SerializeField]
    public List<string> animNames;//названия анимаций
    public List<MyAnimation> anims;//анимации
    private Dictionary<string, MyAnimation> animations;//словарь(название, анимация)
    public string  currentAnimation;//воспроизводимая анимация
    public float currentTime;//какой сейчас момент времени
    public bool loop;//зациклена ли анимация
    public bool playAnimation;//воспроизводися ли анимация
    private SpriteRenderer spriteRenderer;//спрайт рендерер

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentTime = 0;
        animations = new Dictionary<string, MyAnimation>();//создаём словарь из двух списков так как в редакторе юнити словари не отображаются
        for(int i = 0; i < animNames.Count; i++)
        {
            animations.Add(animNames[i], anims[i]);
        }
	}

    /// <summary>
    /// Воспроизвести анимацию
    /// </summary>
    /// <param name="aName">название анимации</param>
    /// <param name="time">время за которое она воспроизведётся</param>
    /// <param name="l">зациклена ли анимация</param>
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

    /// <summary>
    /// Показ определённого кадра анимации на времени t если максимальное время проигрыша анимации t2
    /// </summary>
    /// <param name="t"></param>
    /// <param name="t2"></param>
    /// <param name="aName">Название анимации</param>
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
        //играем анимацию если она включена
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
            spriteRenderer.sprite = animations[currentAnimation].GetCurrentSprite(currentTime);//получаем текущий кадр
        }
	}

}
