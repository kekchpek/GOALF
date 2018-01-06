using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFixer : MonoBehaviour {

    public enum XY { X, Y }
    public float sizeDeltaX;
    public float sizeDeltaY;
    public float anchoredPosX;
    public float anchoredPosY;
    public XY XorY;
    private RectTransform rt;
    public Camera cam;

    void Awake()
    {
        if (rt == null)
            rt = GetComponent<RectTransform>();
        Lock();
    }

    public void CalulateSize()
    {
        rt = GetComponent<RectTransform>();
        if (XorY == XY.X)
        {
            sizeDeltaX = rt.sizeDelta.x / cam.pixelWidth;
            sizeDeltaY = rt.sizeDelta.y / cam.pixelWidth;
            anchoredPosX = rt.anchoredPosition.x / cam.pixelWidth;
            anchoredPosY = rt.anchoredPosition.y / cam.pixelWidth;
        }
        if(XorY == XY.Y)
        {
            sizeDeltaY = rt.sizeDelta.y / cam.pixelHeight;
            sizeDeltaX = rt.sizeDelta.x / cam.pixelHeight;
            anchoredPosX = rt.anchoredPosition.x / cam.pixelHeight;
            anchoredPosY = rt.anchoredPosition.y / cam.pixelHeight;
        }
    }

    public void Lock()
    {
        if (rt == null)
            rt = GetComponent<RectTransform>();
        if (XorY == XY.X)
        {
            rt.anchoredPosition = new Vector2(cam.pixelWidth * anchoredPosX, cam.pixelWidth * anchoredPosY);
            rt.sizeDelta = new Vector2(cam.pixelWidth * sizeDeltaX, cam.pixelWidth * sizeDeltaY);
        }
        if (XorY == XY.Y)
        {
            rt.anchoredPosition = new Vector2(cam.pixelHeight * anchoredPosX, cam.pixelHeight * anchoredPosY);
            rt.sizeDelta = new Vector2(cam.pixelHeight * sizeDeltaX, cam.pixelHeight * sizeDeltaY);
        }
    }
	
}
