using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

/// <summary>
/// Главный контроллер, который управляет сменой экранов, инпутом и сохранением рекордов
/// </summary>
public class MainController : MonoBehaviour {

    public static MainController controller;//статическая ссылка на этот контроллер
    public GameObject audioSources;//объект со всеми аудиосоурсами в игре
    public AudioSource loopAudio;//аудиосоурс с фоновой музыкой

    protected object[] argsForScreen;
    public Text log;
    public ScreenController currentScreen, nextScreen;//текущий экран  и экран на который нужно перейдти
    public Image fadeImage;//картинка для затемнения экрана
    public int bestPoints;//регкорд
    public Camera currentCamera;//текущая активная камера
    public bool inputTime;//работает ли инпут
    public ScreenController[] controllers;//массив всех контроллеров в игре
    public int firstScreen;
    InterstitialAd ad;//межстраничная реклама
    public int adTimer;//раз в сколько игр показывается реклама
    int adTime;//счётчик определяющий сколько игр осталось до показа рекламы
    string adHandle = "ca-app-pub-5377701829054453/9318814921";//хэндл рекламы
    private bool changeScreenFlag;
    float sTime, sTimer, sTimer2;//таймеры для перехода от одного экрана к другому
    public bool soundOn { get { return audioSources.active; } set { audioSources.SetActive(value); } }//включён ли звук
 
    void OnApplicationFocus(bool hasFocus)//при сворачивании приложения ставит паузу если идёт игра
    {
        if (!hasFocus)
        {
            currentScreen.GameNotFocus();
        }
    }

    void OnApplicationPause(bool pauseStatus)//при сворачивании приложения ставит паузу если идёт игра
    {
        if (pauseStatus)
        {
            currentScreen.GameNotFocus();
        }
    }

    public AudioSource GenerateAudioSource()
    {
        return audioSources.AddComponent<AudioSource>();
    }


    /// <summary>
    /// Функция обрабатывающая инпут
    /// </summary>
    void InputController()
    {
        if (inputTime)
        {
#if UNITY_EDITOR//ввод мышкой
            if (Input.GetMouseButtonUp(0))
            {
               /* Ray r = currentCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, 100000))
                {
                    if (hit.collider.GetComponent<InteractableObject>() != null)
                    {
                        hit.collider.GetComponent<InteractableObject>().Up();
                    }
                }*/
            }
#endif
            if (Input.touchCount > 0)
            {
                Ray r = currentCamera.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, Mathf.Infinity))
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        hit.collider.GetComponent<InteractableObject>().Up();
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        hit.collider.GetComponent<InteractableObject>().Press();
                    }
                }
            }
        }
        if (nextScreen == null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentScreen.OnEscapeButtonPressed();
            }
        }
    }

    public void Log(string s)
    {
        log.text = s + "\n" + log.text;
    }

    /// <summary>
    /// Инициализация игры
    /// </summary>
    protected virtual void Start () {
        foreach (ScreenController c in controllers)//инициализация всех контроллеров
            c.GameLoadInitialization();
        if (controller == null)
            controller = this;
        GoToScreen(controllers[firstScreen], 0.2f, 0.4f, Color.black);//сразу переходим к экрану с интро
        adTime = adTimer;//обнуляем счётчик игр через которые появится реклама
        if (PlayerPrefs.GetInt("sound", 1) == 1)//включаем/выключаем звук
            soundOn = true;
        else
            soundOn = false;
    }

    /// <summary>
    /// Включение/отключение инпута
    /// </summary>
    public void SetInputTime(bool b)
    {
        StartCoroutine(SetInputTimeC(b));//инпут включается только в следующем кадре
    }

    IEnumerator SetInputTimeC(bool b)
    {
        yield return null;
        inputTime = b;
    }

    /// <summary>
    /// Переход к указанному экрану с помощью затемнения экрана за t секунд
    /// </summary>
    /// <param name="controller"> контролеер экрана, который хотим включить</param>
    /// <param name="t">время, за которое проиходит фэйд</param>
    /// <param name="t2">время, за которое проиходит анфэйд</param>
    public void GoToScreen(ScreenController controller, float t, float t2, object[] args)
    {
        argsForScreen = args;
        if (nextScreen == null)//если в данный момент не происходит свап
        {
            fadeImage.color = new Color(0, 0, 0, 0);
            nextScreen = controller;
            sTimer = t;
            sTimer2 = t2;
            inputTime = false;
            changeScreenFlag = false;
        }
    }
    
    public void GoToScreen(ScreenController controller, float t, float t2, Color color, object[] args)
    {
        argsForScreen = args;
        if (nextScreen == null)//если в данный момент не происходит свап
        {
            changeScreenFlag = false;
            color = new Color(color.r, color.g, color.b, 0);
            fadeImage.color = color;
            nextScreen = controller;
            sTimer = t;
            sTimer2 = t2;
            inputTime = false;
        }
    }

    public void GoToScreen(ScreenController controller, float t, object[] args)
    {
        argsForScreen = args;
        if (t == 0)//мнгновенный переход к экрану
        {
            nextScreen = controller;
            currentScreen.Final();
            currentScreen.gameObject.SetActive(false);
            nextScreen.Init();
            nextScreen.gameObject.SetActive(true);
            currentScreen = nextScreen;
            nextScreen = null;
            currentCamera = currentScreen.cam;
            return;
        }
        GoToScreen(controller, t / 2f, t / 2f);
    }

    public void GoToScreen(ScreenController controller, float t, Color color, object[] args)
    {
        argsForScreen = args;
        if (t == 0)//мнгновенный переход к экрану
        {
            nextScreen = controller;
            currentScreen.Final();
            currentScreen.gameObject.SetActive(false);
            nextScreen.Init();
            nextScreen.gameObject.SetActive(true);
            currentScreen = nextScreen;
            nextScreen = null;
            currentCamera = currentScreen.cam;
            return;
        }
        GoToScreen(controller, t / 2f, t / 2f, color);
    }

    public void GoToScreen(ScreenController controller, float t, float t2, Color color)
    {
        GoToScreen(controller, t, t2, color, null);
    }

    public void GoToScreen(ScreenController controller, float t, float t2)
    {
        GoToScreen(controller, t, t2, null);
    }

    public void GoToScreen(ScreenController controller, float t, Color color)
    {
        GoToScreen(controller, t, color, null);
    }

    public void GoToScreen(ScreenController controller, float t)
    {
        GoToScreen(controller, t, null);
    }

    void Update()
    {
        //инпут
        InputController();
        if (nextScreen != null)//переход к следующему экрану
        {
            sTime += Time.deltaTime;
            float r, g, b, a;//вычисляем нужный цвер
            r = fadeImage.color.r;
            g = fadeImage.color.g;
            b = fadeImage.color.b;
            if (!changeScreenFlag)//фэйд или анфэйд происходит в данный момент
                a = sTime / sTimer;
            else
                a = 1 - (sTime / sTimer2);
            fadeImage.color = new Color(r, g, b, a);
            if (!changeScreenFlag)//переход от одного экрана к друггому
            {
                if (sTime > sTimer)//в момент когда экран полностью затемнён
                {
                    sTime -= sTimer;
                    if (currentScreen != null)
                    {
                        currentScreen.Final();
                        currentScreen.gameObject.SetActive(false);
                    }
                    if (argsForScreen != null)
                    {
                        nextScreen.Init(argsForScreen);
                        argsForScreen = null;
                    }
                    else
                    {
                        nextScreen.Init();
                    }
                    nextScreen.gameObject.SetActive(true);
                    currentScreen = nextScreen;
                    changeScreenFlag = true;
                    currentCamera = currentScreen.cam;

                }
            }
            else
            {
                if (sTime > sTimer2)//заканчиваем переход
                {
                    sTimer = 0;
                    sTime = 0;
                    nextScreen = null;
                    inputTime = true;
                }
            }
        }
    }
}
