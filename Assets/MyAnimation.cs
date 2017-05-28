using UnityEngine;
using System;
using UnityEditor;

public class MyAnimation : ScriptableObject {
    public Sprite[] frames;
    public float playTime;


    [MenuItem("Assets/Create/MyAnimation")]
    public static void CreateMyAnim()
    {
        CreateSO.CreateAsset<MyAnimation>();
    }


    public Sprite GetCurrentSprite(float t)
    {
        return frames[Mathf.RoundToInt(t * (frames.Length-1) / playTime)];
    }
}