using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameScreenController : MainMenuController {

    public Transform ballRaspawn;
    public BallController ball;
    public float camSpeed;
    public GameObject gameCanvas;
    bool moveFlag;
    public float currentHeight;
    public float ballHeight;
    public float minX, maxX;
    public int currentLevel;
    private bool canSetFlag;
    public int currentMode;
    public GameObject loseZone;
    public GameObject bottom;
    private bool loseFlag;
    public GameObject[] easyPlatform;
    public GameObject[] notEasyPlatform;
    public GameObject[] hardcorePlatform;
    public GameObject[] impozzibruPlatform;
    public List<GameObject> platforms;
    public GameObject flag;
    private int hitCount;
    private int points;
    private Vector2 ballSpeed;
    public Text skillText;
    private float skillTimer;
    private int currentLevelPoints;
    public int[] completePoints;
    public GameObject pauseObj;
    public bool pauseFlag;
    private Vector3 camPos;
    public float loseJoltTime;
    private bool endJoltFlag, endFadeFlag, endContentFlag;
    float currentLoseJoltTime;
    public Image endBackground;
    public Text[] endText;
    public Text[] pointsText;
    public RectTransform[] endMenu;
    public float endFadeTime;
    private float currentEndFadeTime;
    public float endContentTime;
    private float currentEndContentTime;
    public GameObject loseMenu;

    protected bool right;

    public override void OnEscapeButtonPressed()
    {
        if (loseFlag)
        {
            base.OnEscapeButtonPressed();
            if(!diffMenu && !creditsMenu && !swapToCredits && !swapToDiff && !swapToMain)
            {
                GoToMenu();
            }
        }
        else
        {
            if(!endContentFlag && !endFadeFlag && !endJoltFlag)
            {
                Pause();
            }
        }
    }

    public void GoToMenu()
    {
        object[] args = new object[3];
        args[0] = currDiff;
        args[1] = currMode;
        args[2] = currentUnlockLevel;
        SoundController.PlaySound("play");
        MainController.controller.GoToScreen(MainController.controller.controllers[1], 0.5f, 0.5f, Color.white, args);
    }

    public override void Init(object[] args)
    {
        base.Init(args);
        Unpause();
        int record = Records.GetRecord();
        platforms = new List<GameObject>();
        loseFlag = false;
        bottom.SetActive(true);
        loseMenu.SetActive(false);
        gameCanvas.SetActive(true);
        loseZone.transform.position = Vector3.down * 100;
        camPos = cam.transform.position;
        ball.transform.position = ballRaspawn.transform.position;
        currentHeight = 0;
        AddPoints(-points);
        for (int i = 1; i <= 10; i++)
        {
            GenerateNext();
        }
        foreach (Text t in recordText)
        {
            t.text = "BEST : " + record.ToString();
            t.color = Color.white;
        }
        currentLevel = ((MainMenuController)MainController.controller.controllers[1]).currDiff;
    }

    public override void Final()
    {
        base.Final();
        foreach(GameObject g in platforms)
        {
            Destroy(g);
        }
    }

    public void Lose()
    {
        if (loseFlag) return;
        if (Records.SetRecord(points))
        {
            foreach (Text t in recordText)
            {
                t.color = Color.yellow;
                t.text = "NEW BEST : " + points.ToString();
            }
        }
        SoundController.PlaySound("gameover");
        camPos = cam.transform.position;
        gameCanvas.SetActive(false);
        endJoltFlag = true;
        loseFlag = true;
        MainController.controller.SetInputTime(false);
        currentLoseJoltTime = loseJoltTime;
        loseMenu.SetActive(true);
        foreach (RectTransform rt in endMenu)
        {
            rt.anchorMin -= Vector2.up * 0.4f;
            rt.anchorMax -= Vector2.up * 0.4f;
            rt.anchoredPosition = Vector2.zero;
        }
        float r, g, b, a;
        r = endBackground.color.r;
        g = endBackground.color.g;
        b = endBackground.color.b;
        a = 0;
        endBackground.color = new Color(r, g, b, 0);
        foreach (Text t in endText)
        {
            r = t.color.r;
            g = t.color.g;
            b = t.color.b;
            a = 0;
            t.color = new Color(r, g, b, a);
        }

    }

    public void SetLoseZone()
    {
        if (cam.ScreenToWorldPoint(new Vector3(0, -cam.pixelHeight * 0.3f, 0)).y > loseZone.transform.position.y)
            loseZone.transform.position = cam.ScreenToWorldPoint(new Vector3(0, -cam.pixelHeight * 0.3f, 0));
    }

    public void Hit()
    {
        hitCount++;
    }

    public void GenerateNext()
    {
        GameObject created = null;
        if (currentLevel == 0)
        {
            created = Instantiate(easyPlatform[Random.Range(0, easyPlatform.Length)], Vector3.zero, Quaternion.identity, transform);
        }
        if (currentLevel == 1)
        {
            created = Instantiate(notEasyPlatform[Random.Range(0, notEasyPlatform.Length)], Vector3.zero, Quaternion.identity, transform);
        }
        if (currentLevel == 2)
        {
            created = Instantiate(hardcorePlatform[Random.Range(0, hardcorePlatform.Length)], Vector3.zero, Quaternion.identity, transform);
        }
        if (currentLevel == 3)
        {
            created = Instantiate(impozzibruPlatform[Random.Range(0, impozzibruPlatform.Length)], Vector3.zero, Quaternion.identity, transform);
        }
        if (right)
        {
            created.transform.localPosition = new Vector3(Random.Range((minX+maxX)/2f, maxX), currentHeight, 0);
        }
        else
        {
            created.transform.localPosition = new Vector3(Random.Range(minX, (minX + maxX) / 2f), currentHeight, 0);
        }
        right = !right;
        platforms.Add(created);
        currentHeight += 4f;
        GameObject _flag = null;
        if (canSetFlag)
        {
            _flag = Instantiate(flag);
            if (Random.Range(0, 2) > 0)
            {
                canSetFlag = false;
            }
            else
            {
                canSetFlag = true;
            }
        }
        else
            canSetFlag = true;
        Transform t = created.transform.GetChild(Random.Range(0, created.transform.childCount));
        if (_flag != null)
        {
            platforms.Add(_flag);
            _flag.transform.position = t.position;
            if (Random.Range(0, 2) == 0)
            {
                _flag.transform.localScale = new Vector3(-_flag.transform.localScale.x, _flag.transform.localScale.y, 1);
            }
            _flag.transform.parent = t;
            _flag.GetComponentInChildren<Flag>().controller = this;
        }
    }

    public int GetFlag()
    {
        int ret = hitCount;
        if (hitCount == 1)
        {
            SoundController.PlaySound("holy");
            AddPoints(5);
            skillText.text = "HOLLY ONE! +5";
            skillText.color = new Color(0, 0.8f, 0);
        }
        if (hitCount == 2)
        {
            SoundController.PlaySound("birdy");
            AddPoints(3);
            skillText.text = "BIRDY +3";
            skillText.color = new Color(1, 0.6f, 0);
        }
        if (hitCount == 3)
        {
            SoundController.PlaySound("par");
            AddPoints(1);
            skillText.text = "PAR +1";
            skillText.color = new Color(0, 0.5f, 1);
        }
        if (hitCount > 3)
        {
            SoundController.PlaySound("booger");
            skillText.text = "BOOGER +0";
            skillText.color = new Color(1, 0, 0);
        }
        skillTimer = 3f;
        hitCount = 0;
        return ret;
    }

    private void AddPoints(int p)
    {
        points += p;
        currentLevelPoints += p;
        foreach (Text t in pointsText)
        {
            t.text = points.ToString();
        }
        if (currentLevelPoints > completePoints[currentLevel])
        {
            currentLevelPoints = 0;
            SetDiff(currentLevel + 1);
        }

    }

    override public void Update()
    {
        base.Update();
        if (skillTimer > 0)
        {
            skillTimer -= Time.deltaTime;
            if (skillTimer < 0)
            {
                skillTimer = 0;
                skillText.text = "";
            }
        }
        if (!loseFlag)
        {
            float by, cy;
            by = ball.transform.position.y;
            cy = cam.transform.position.y;
            if (moveFlag)
            {
                if (by > cy)
                {
                    cam.transform.position += Vector3.up * (by - cy);
                }
                if (cy - by > 0.3f * (cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y - cam.ScreenToWorldPoint(Vector3.zero).y))
                {
                    cam.transform.position += -cy * Vector3.up + by * Vector3.up + Vector3.up * 0.3f * (cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y - cam.ScreenToWorldPoint(Vector3.zero).y);
                }
            }
            else
            {
                if (Mathf.Abs(cy - by - 0.3f * (cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y - cam.ScreenToWorldPoint(Vector3.zero).y)) <= camSpeed * Time.deltaTime)
                {
                    cam.transform.position += -Vector3.up * cy + Vector3.up * (by + 0.3f * (cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y - cam.ScreenToWorldPoint(Vector3.zero).y));
                }
                else
                {
                    if (cy - by < 0.3f * (cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y - cam.ScreenToWorldPoint(Vector3.zero).y))
                    {
                        cam.transform.position += Vector3.up * camSpeed * Time.deltaTime;
                    }
                    if (cy - by > 0.3f * (cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y - cam.ScreenToWorldPoint(Vector3.zero).y))
                    {
                        cam.transform.position += Vector3.down * camSpeed * Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            if (endJoltFlag)
            {
                if (currentLoseJoltTime > 0)
                {
                    currentLoseJoltTime -= Time.deltaTime;
                    cam.transform.position = camPos + Vector3.up * Random.Range(-0.5f, 0.5f) + Vector3.right * Random.Range(-0.5f, 0.5f);
                    if(currentLoseJoltTime < 0)
                    {
                        cam.transform.position = camPos;
                        endJoltFlag = false;
                        endFadeFlag = true;
                        currentEndFadeTime = endFadeTime;
                        currentLoseJoltTime = 0;
                    }
                }
            }
            if (endFadeFlag)
            {
                if (currentEndFadeTime > 0)
                {
                    currentEndFadeTime -= Time.deltaTime;
                    if (currentEndFadeTime <= 0)
                    {
                        currentEndContentTime = endContentTime;
                        currentEndFadeTime = 0;
                        endFadeFlag = false;
                        endContentFlag = true;
                    }
                    float r, g, b, a;
                    r = endBackground.color.r;
                    g = endBackground.color.g;
                    b = endBackground.color.b;
                    a = 0.6f - currentEndFadeTime / endFadeTime * 0.6f;
                    endBackground.color = new Color(r, g, b, a);
                }
            }
            if (endContentFlag)
            {
                if (currentEndContentTime > 0)
                {
                    currentEndContentTime -= Time.deltaTime;
                    float dt = Time.deltaTime;
                    if (currentEndContentTime <= 0)
                    {
                        dt += currentEndContentTime;
                        currentEndContentTime = 0;
                        endContentFlag = false;
                        MainController.controller.SetInputTime(true);
                    }
                    foreach (Text t in endText)
                    {
                        float r, g, b, a;
                        r = t.color.r;
                        g = t.color.g;
                        b = t.color.b;
                        a = 1 - currentEndContentTime / endContentTime;
                        t.color = new Color(r, g, b, a);
                    }
                    foreach(RectTransform rt in endMenu)
                    {
                        rt.anchorMin += Vector2.up * 0.4f / endContentTime * dt;
                        rt.anchorMax += Vector2.up * 0.4f / endContentTime * dt;
                        rt.anchoredPosition = Vector2.zero;
                    }
                }
            }
        }
    }
     
    public void Pause()
    {
        if (!pauseFlag)
        {
            gameCanvas.SetActive(false);
            ballSpeed = ball.rb.velocity;
            ball.rb.velocity = Vector3.zero;
            ball.rb.isKinematic = true;
            pauseObj.SetActive(true);
            pauseFlag = true;
        }
    }

    public void Unpause()
    {
        if (pauseFlag)
        {
            ball.rb.isKinematic = false;
            ball.rb.velocity = ballSpeed;
            pauseObj.SetActive(false);
            gameCanvas.SetActive(true);
            pauseFlag = false;
        }
    }

    public void SetMove(bool b)
    {
        if(moveFlag && !b)
        {
            float y = ball.transform.position.y;
            while(currentHeight - y < 45)
            {
                GenerateNext();
            }
            ballHeight = y;
        }
        moveFlag = b;
    }

}
