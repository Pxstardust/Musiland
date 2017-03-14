using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public bool loop;
    public string name;
    public AudioClip clip;

    [Range(0f, 10f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    private AudioSource source;
    float currenttime = 0f;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void PlayAtTime(float _time)
    {
        source.time = _time;
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public bool IsPlaying()
    {
        if (source.isPlaying) return true;
        else return false;
    }

    public float GetTime()
    {
        return source.time;
    }

    public void PrintTime()
    {
        Debug.Log(source.time);
    }
}

// ======================================= //
// ======================================= //
// ======================================= //

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    [SerializeField]
    AudioArray[] soundsarray;

    List<Sound> buffer = new List<Sound>();

    // ======================================= //
    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            Debug.LogError("Il existe plus d'un AudioManager");
        }
        else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        instance = this;


    }

    // ======================================= //
    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

        PlaySound("Calm_BGM");
    }

    // ======================================= //
    void Update()
    {
        if (Time.time > 5f)
        {
            //StopSound("Music");
        }
    }

    // ================================= //
    // ========== JOUE UN SON ========== //
    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
        // Pas de son avec ce nom
        Debug.LogWarning("AudioManager : Son manquant ou non incorrect :" + _name);
    }

    // ===================================================== //
    // ========== JOUE UN SON S'IL N'EST PAS JOUE ========== //
    public void PlaySoundIfNoPlaying(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                if (!sounds[i].IsPlaying())
                {
                    sounds[i].Play();
                    return;
                }

            }
        }
    }

    // =============================================== //
    // ========== STOP UN SON S'IL EST JOUE ========== //
    public void StopSoundIfPlaying(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                if (sounds[i].IsPlaying())
                {
                    sounds[i].Stop();
                    return;
                }

            }
        }
    }

    // ============================== //
    // ========== STOP SON ========== //
    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // Pas de son avec ce nom
        Debug.LogWarning("AudioManager : Son manquant ou non incorrect :" + _name);
    }

    // ======================================== //
    // ========== CHECKS SI SON JOUE ========== //
    public bool IsSoundPlaying(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                if (sounds[i].IsPlaying())
                {
                    return true;
                }
                else return false;
            }
        }
        return false;
    }

    public float GetSoundTime(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                return sounds[i].GetTime();

            }
        }
        return 0;

    }

    public void PlaySoundAtTime(string _name, float _time)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].PlayAtTime(_time);

            }
        }
    }

    /* public void PlayRandomFromArray(int _idarray)
     {
         List<Sound> arraysound = new List<Sound>();
         for (int i = 0; i < sounds.Length; i++)
         {
             if (sounds[i].groupid == _idarray)
             {
                 arraysound.Add(sounds[i]);
             }
         }
         if (arraysound.Count > 0)
         {
             arraysound[Random.Range(0, arraysound.Count)].Play(); ;
         }
     }*/

    public void PlayArrayUntilDone(int _idarray)
    {
        AudioArray Theone = null;
        bool shouldrandom = false;

        // ===== RECUP ARRAY ===== //
        for (int i = 0; i < soundsarray.Length; i++)
        {
            if (soundsarray[i].arrayid == _idarray)
            {
                Theone = soundsarray[i];
                shouldrandom = soundsarray[i].israndom;
            }
        }

        if (buffer.Count == 0)
        {


            // ===== Parcours du tableau pour le remplir ===== //
            if (Theone && buffer.Count == 0)
            {
                for (int i = 0; i < Theone.sounds.Length; i++)
                {
                    buffer.Add(Theone.sounds[i]);
                }
            }

        } // Fin remplir buffer

        if (buffer.Count > 0)
        {
            int i = 0;
            if (shouldrandom) i = Random.Range(0, buffer.Count);
            buffer[0].Play();
            buffer.RemoveAt(0);
        }

    }

}