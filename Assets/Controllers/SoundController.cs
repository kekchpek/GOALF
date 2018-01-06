using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundController : MonoBehaviour {
    
    public int audioLayersCount = 10;
    public List<string> soundsName;
    public List<AudioClip> soundsClip;
    static Dictionary<string, AudioClip> sounds;
    static Dictionary<string, AudioSource> loopAudios;
    static Queue<AudioSource> freeAudioSources;
    static GameObject gObj;
    static SoundController instance;

    void Awake()
    {
        instance = this;
        freeAudioSources = new Queue<AudioSource>();
        gObj = new GameObject();
        UnityEngine.Object.DontDestroyOnLoad(gObj);
        gObj.name = "audioSourceContainer";
        sounds = new Dictionary<string, AudioClip>();
        loopAudios = new Dictionary<string, AudioSource>();
        for(int i = 0; i<soundsName.Count; ++i)
        {
            
            sounds.Add(soundsName[i], soundsClip[i]);
            soundsName.RemoveAt(i);
            soundsClip.RemoveAt(i);
        }
        for(int i = 0; i<audioLayersCount; ++i)
        {
            freeAudioSources.Enqueue(gObj.AddComponent<AudioSource>());
        }
    }

    public static void AddSound(AudioClip clip, string name)
    {
        instance.OAddSound(clip, name);
    }

    public void OAddSound(AudioClip clip, string name)
    {
        sounds.Add(name, clip);
    }

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
            AudioSource aSor = freeAudioSources.Dequeue();
            aSor.clip = clip;
            aSor.loop = loop;
            aSor.Play();
            if(!loop)
            {
                StartCoroutine(Free(clip.length * 1, aSor));
            }
            else
            {
                loopAudios.Add(key, aSor);
            }
        }
    }

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

    static IEnumerator Free(float t, AudioSource aSor)
    {
        yield return new WaitForSeconds(t);
        freeAudioSources.Enqueue(aSor);
    }


}
