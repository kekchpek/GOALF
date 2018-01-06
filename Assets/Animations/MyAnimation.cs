using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MyAnimation : ScriptableObject {
    public Sprite[] frames;
    public float playTime;



#if UNITY_EDITOR
    [MenuItem("Assets/Create/MyAnimation")]
    public static void CreateMyAnim()
    {
        CreateSO.CreateAsset<MyAnimation>();
    }
#endif


    public Sprite GetCurrentSprite(float t)
    {
        int n = Mathf.RoundToInt(t * (frames.Length - 1) / playTime);
        if (n < 0) n = 0;
        if (n > frames.Length - 1) n = frames.Length - 1;
        return frames[n];
    }
}