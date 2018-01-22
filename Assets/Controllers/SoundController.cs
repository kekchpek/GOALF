using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundController : MonoBehaviour {
    
    public int audioLayersCount = 10;//максимальное количество воспроизводимых одновременно звуков
    //два списка для словаря заданных в редакторе звуков
    public List<string> soundsName;
    public List<AudioClip> soundsClip;
    //собственно сам словарь
    static Dictionary<string, AudioClip> sounds;

    static Dictionary<string, AudioSource> loopAudios;

    static Queue<AudioSource> freeAudioSources;//очередь свободных источников звука
    static GameObject gObj;//объект-контейнер для источников звука
    static SoundController instance;//ссылка на самого себя

    void Awake()
    {
        //инициализация начальных переменных
        instance = this;
        freeAudioSources = new Queue<AudioSource>();
        gObj = new GameObject();
        UnityEngine.Object.DontDestroyOnLoad(gObj);
        gObj.name = "audioSourceContainer";
        sounds = new Dictionary<string, AudioClip>();
        loopAudios = new Dictionary<string, AudioSource>();
        for(int i = 0; i<soundsName.Count; ++i)//составление словаря из двух списков
        {
            
            sounds.Add(soundsName[i], soundsClip[i]);
            soundsName.RemoveAt(i);
            soundsClip.RemoveAt(i);
        }
        for(int i = 0; i<audioLayersCount; ++i)//составление очереди свободных источников звука
        {
            freeAudioSources.Enqueue(gObj.AddComponent<AudioSource>());
        }
    }

    /// <summary>
    /// Добавляет клип к словарю известных звуков
    /// </summary>
    /// <param name="clip">клип</param>
    /// <param name="name">название клипа</param>
    public static void AddSound(AudioClip clip, string name)
    {
        instance.OAddSound(clip, name);
    }

    public void OAddSound(AudioClip clip, string name)
    {
        sounds.Add(name, clip);
    }


    /// <summary>
    /// Проигрывает указанный по названию клип если он есть в словаре известных
    /// </summary>
    /// <param name="s">название клипа</param>
    /// <param name="volume">громкость [0, 1]</param>
    /// <param name="loop">закилен ли звук</param>
    /// <param name="key">Ключ присваемый зацикленному звуку. Чтобы потом выключить зацикленный звук нужно обратиться к нему по этому ключу</param>
    public static void PlaySound(string s, float volume = 0.5f, bool loop = false, string key = "")
    {
        instance.OPlaySound(s, volume, loop, key);
    }

    public void OPlaySound(string s, float volume = 0.5f, bool loop = false, string key = "")
    {
        if(sounds.ContainsKey(s))
        {
            PlaySound(sounds[s], volume, loop, key);
        }
    }

    /// <summary>
    /// Проигрывает заданый клип
    /// </summary>
    /// <param name="clip">Клип</param>
    /// <param name="volume">громкость [0, 1]</param>
    /// <param name="loop">закилен ли звук</param>
    /// <param name="key">Ключ присваемый зацикленному звуку. Чтобы потом выключить зацикленный звук нужно обратиться к нему по этому ключу</param>
    public static void PlaySound(AudioClip clip, float volume = 0.5f, bool loop = false, string key = "")
    {
        instance.OPlaySound(clip, volume, loop, key);
    }

    public void OPlaySound(AudioClip clip, float volume = 0.5f, bool loop = false, string key = "")
    {
        if(freeAudioSources.Count>0)
        {
            if(loop)
            {
                if (loopAudios.ContainsKey(key))
                    return;
            }
            AudioSource aSor = freeAudioSources.Dequeue();//достаём источник звука из очереди
            aSor.clip = clip;
            aSor.loop = loop;
            aSor.Play();
            if(!loop)
            {
                StartCoroutine(Free(clip.length * 1, aSor));//освобождаем источник звука после того как клип проиграется
            }
            else
            {
                loopAudios.Add(key, aSor);
            }
        }
    }


    /// <summary>
    /// Остановить зацикленный звук идентифицируемый ключём key
    /// </summary>
    /// <param name="key"></param>
    public static void StopSound(string key)
    {
        instance.OStopSound(key);
    }

    public void OStopSound(string key)
    {
        if(loopAudios.ContainsKey(key))
        {
            AudioSource aSor = loopAudios[key];
            aSor.Stop();
            freeAudioSources.Enqueue(aSor);
        }
    }

    /// <summary>
    /// Добавляет источник звука в лчередь свободных
    /// </summary>
    /// <param name="t"></param>
    /// <param name="aSor"></param>
    /// <returns></returns>
    static IEnumerator Free(float t, AudioSource aSor)
    {
        yield return new WaitForSeconds(t);
        freeAudioSources.Enqueue(aSor);
    }


}
