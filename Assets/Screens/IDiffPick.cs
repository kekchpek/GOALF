using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Не помню что это такое
/// </summary>
public class IDiffPick : ScreenController {


    public Text errorText;
    public Text[] diffText;
    protected float errorTextTime;

    public int currDiff;
    public int currMode;

    public byte currentUnlockLevel;
    public Image[] lockImages;
    public Image[] underlinedDiffImage;
    public Image[] underlinedModeImage;


    protected bool swapToDiff, swapToMain, swapToCredits, creditsMenu, diffMenu;
    public float swapDistToDiff, swapDistToCredits;
    protected float swapDist;
    public float swapSpeed;

    protected string[] levelsDiff = { "EASY", "NOY EASY", "HARDCORE", "IMPOZZIBRU" };
    protected string[] levelsMode = { "FROM", "ALLWAYS" };

    public RectTransform[] menuItems;

    protected float currentSwapDist;

    protected void Update()
    {
        if (swapToDiff || swapToCredits)
        {
            float d = swapSpeed * Time.deltaTime;
            currentSwapDist += swapSpeed * Time.deltaTime;
            if (currentSwapDist >= swapDist)
            {
                diffMenu = swapToDiff;
                creditsMenu = swapToCredits;
                swapToCredits = swapToDiff = false;
                swapToDiff = false;
                MainController.controller.inputTime = true;
                d -= currentSwapDist - swapDist;
            }
            if (swapToCredits)
                d = -d;
            foreach (RectTransform rt in menuItems)
            {
                Vector2 ap = rt.anchoredPosition;
                Vector2 sd = rt.sizeDelta;
                rt.anchorMin += Vector2.up * d;
                rt.anchorMax += Vector2.up * d;
                rt.anchoredPosition = ap;
                rt.sizeDelta = sd;
            }
        }
        if (swapToMain)
        {
            float d = swapSpeed * Time.deltaTime;
            if (creditsMenu)
                d = -d;
            currentSwapDist += swapSpeed * Time.deltaTime;
            if (currentSwapDist >= swapDist)
            {
                swapToMain = false;
                diffMenu = false;
                creditsMenu = false;
                MainController.controller.inputTime = true;
                d -= currentSwapDist - swapDist;
            }
            foreach (RectTransform rt in menuItems)
            {
                rt.anchorMin -= Vector2.up * d;
                rt.anchorMax -= Vector2.up * d;
                rt.anchoredPosition = Vector2.zero;
            }
        }
        if (errorTextTime > 0)
        {
            errorTextTime -= Time.deltaTime;
            if (errorTextTime <= 0)
            {
                errorTextTime = 0;
                errorText.text = "";
            }
        }
    }



}
