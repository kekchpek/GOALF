using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MyAnimation : ScriptableObject {
    public Sprite[] frames;//кадры
    public float playTime;//время за которое проигрывается анимация



#if UNITY_EDITOR//добавляем в меню создание анимации
    [MenuItem("Assets/Create/MyAnimation")]
    public static void CreateMyAnim()
    {
        CreateSO.CreateAsset<MyAnimation>();
    }
#endif


    public Sprite GetCurrentSprite(float t)//получение кадра на указанном времени
    {
        int n = Mathf.RoundToInt(t * (frames.Length - 1) / playTime);
        if (n < 0) n = 0;
        if (n > frames.Length - 1) n = frames.Length - 1;
        return frames[n];
    }
}