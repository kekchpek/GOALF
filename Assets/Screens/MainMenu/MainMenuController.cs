using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : ScreenController
{

    protected string[] levelsDiff = { "EASY", "NOY EASY", "HARDCORE", "IMPOZZIBRU" };
    protected string[] levelsMode = { "FROM", "ALLWAYS" };

    public RectTransform[] menuItems;
    protected bool swapToDiff, swapToMain, swapToCredits, creditsMenu, diffMenu;
    public float swapDistToDiff, swapDistToCredits;
    protected float swapDist;
    public float swapSpeed;

    public int currentUnlockLevel;
    public Image[] lockImages;
    public Image[] underlinedDiffImage;
    public Image[] underlinedModeImage;
    public Text errorText;
    public Text[] diffText;
    public Text[] recordText;
    protected float errorTextTime;

    public int currDiff;
    public int currMode;

    protected float currentSwapDist;


    public void StartGame()
    {
        object[] args = new object[3];
        args[0] = currDiff;
        args[1] = currMode;
        args[2] = currentUnlockLevel;
        SoundController.PlaySound("play");
        MainController.controller.GoToScreen(MainController.controller.controllers[2], 0.5f, 0.5f, Color.white, args);
    }

    public virtual void Update()
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
                MainController.controller.inputTime = true;
                d -= currentSwapDist - swapDist;
            }
            if (swapToCredits || creditsMenu)
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
            currentSwapDist += swapSpeed * Time.deltaTime;
            if (currentSwapDist >= swapDist)
            {
                int k = 1;
                if (creditsMenu)
                    k = -1;
                swapToMain = false;
                diffMenu = false;
                creditsMenu = false;
                MainController.controller.inputTime = true;
                d -= currentSwapDist - swapDist;
                d *= k;
            }
            if (creditsMenu)
                d = -d;
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

    public void GoToMarket()
    {
        Application.OpenURL("market://details?id=com.DARTEKZ");
    }

    public void Rate()
    {
        Application.OpenURL("market://details?id=com.DARTEKZ");
    }

    public override void Final()
    {
        base.Final();
        if(diffMenu)
        {
            foreach (RectTransform rt in menuItems)
            {
                rt.anchorMin -= 0.6f * Vector2.up;
                rt.anchorMax -= Vector2.up * 0.6f;
                rt.anchoredPosition = Vector2.zero;
            }
        }
        if(creditsMenu)
        {
            foreach (RectTransform rt in menuItems)
            {
                rt.anchorMin += Vector2.up * 0.85f;
                rt.anchorMax += Vector2.up * 0.85f;
                rt.anchoredPosition = Vector2.zero;
            }
        }
        diffMenu = false;
        creditsMenu = false;
    }

    public override void Init(object[] args)
    {
        int _diff = (int)args[0];
        int _mode = (int)args[1];
        int _unlock = (int)args[2];
        while (currentUnlockLevel<_unlock)
        {
            UnlockNext();
        }
        int record = Records.GetRecord();
        foreach (Text t in recordText)
        {
            t.text = record.ToString();
            t.color = Color.black;
        }
        SetDiff(_diff);
        SetMode(_mode);
        base.Init();
        errorText.text = "";
    }

    public void DiffMenu()
    {
        swapDist = swapDistToDiff;
        currentSwapDist = 0;
        if (!diffMenu)
            swapToDiff = true;
        else
            swapToMain = true;
        MainController.controller.inputTime = false;
    }

    public void CreditsMenu()
    {
        swapDist = swapDistToCredits;
        currentSwapDist = 0;
        if (!creditsMenu)
            swapToCredits = true;
        else
            swapToMain = true;
        MainController.controller.inputTime = false;
    }

    public override void OnEscapeButtonPressed()
    {
        base.OnEscapeButtonPressed();
        if(creditsMenu)
        {
            CreditsMenu();
        }
        if(diffMenu)
        {
            DiffMenu();
        }
    }

    public void UnlockNext()
    {
        if (currentUnlockLevel < 4)
        {
            currentUnlockLevel++;
            lockImages[currentUnlockLevel].gameObject.SetActive(false);
        }
    }

    public void SetDiff(int n)
    {
        if(n<=currentUnlockLevel)
        {
            underlinedDiffImage[currDiff].gameObject.SetActive(false);
            currDiff = n;
            underlinedDiffImage[currDiff].gameObject.SetActive(true);
            foreach (Text t in diffText)
            {
                t.text = "difficulty:\n" + levelsMode[currMode] + " " + levelsDiff[currDiff];
            }
        }
        else
        {
            SoundController.PlaySound("locked");
            errorTextTime = 5;
            errorText.text = "locked! reach " + levelsDiff[n] + " to unlock";
        }
    }

    public void SetMode(int n)
    {
        underlinedModeImage[currMode].gameObject.SetActive(false);
        currMode = n;
        underlinedModeImage[currMode].gameObject.SetActive(true);
        foreach (Text t in diffText)
        {
            t.text = "difficulty:\n" + levelsMode[currMode] + " " + levelsDiff[currDiff];
        }
    }
}
