using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{

    public bool loop;
    public string name;
    public AudioClip clip;
    public int ArrayHoldingNumber;
    public bool should_loop_in_array;

    public int currentholding;

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

    public bool holdonbaby;


    List<List<Sound>> bufferlist = new List<List<Sound>>();
    

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

        for (int i=0; i < soundsarray.Length; i++)
        {
            for (int j = 0; j < soundsarray[i].sounds.Length; j++)
            {
                GameObject _go = new GameObject("Sound_" + i + "_" + soundsarray[i].sounds[j].name);
                _go.transform.SetParent(this.transform);
                soundsarray[i].sounds[j].SetSource(_go.AddComponent<AudioSource>());
                List<Sound> buffer = new List<Sound>();
                bufferlist.Add(buffer);
            }
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

    // =========================================================//
    // =========================================================//
    // =========================================================//

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


    // =========================================================//
    // =========================================================//
    // =========================================================//

    public void StopThatArray(int _idarray)
    {
        AudioArray Theone = null;

        // ===== RECUP ARRAY ===== //
        for (int i = 0; i < soundsarray.Length; i++)
        {
            if (soundsarray[i].arrayid == _idarray)
            {
                for (int j =0; j < soundsarray[i].sounds.Length; j++)
                {
                    soundsarray[i].sounds[j].Stop();
                }
            }
        }

        holdonbaby = true;


    }

    // =========================================================//
    // =========================================================//
    // =========================================================//

    public void ResetThatArray(int _idarray)
    {
        holdonbaby = false;
        bufferlist[_idarray].Clear();
    }


    // =========================================================//
    // =========================================================//
    // =========================================================//

    public void ImmediatelyPAUD(int _idarray)
    {
        //print(bufferlist[_idarray].Count);
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
        } // ===================== //

        // ========== BUFFER ========== //
        if (bufferlist[_idarray].Count == 0)
        {

            // ===== Parcours du tableau pour le remplir ===== //
            if (Theone && bufferlist[_idarray].Count == 0)
            {
                for (int i = 0; i < Theone.sounds.Length; i++)
                {
                    bufferlist[_idarray].Add(Theone.sounds[i]);
                }
            }

        } // ========================== //

       
        for (int z = 0; z < bufferlist[_idarray].Count; z++)
        {
            //print(id_array+" -- "+bufferlist[_idarray][z].name);
        }
        
        if (bufferlist[_idarray].Count > 0)
        {
            int i = 0;

                    if (bufferlist[_idarray][0].loop != true) // qui a été trop jouée
                    {
                        bufferlist[_idarray][0].currentholding = 0;
                        if (!bufferlist[_idarray][0].should_loop_in_array) bufferlist[_idarray].RemoveAt(0);
                        if (bufferlist[_idarray].Count == 0)
                            {
                                ImmediatelyPAUD(_idarray);
                    
                            } else
                            {
                                if (shouldrandom) i = Random.Range(0, bufferlist[_idarray].Count);
                                else i = 0;
                                bufferlist[_idarray][i].Play();
                    //print(bufferlist[_idarray][i].name);
                }

                    }
                    else // Musique qui loop ou musique pas assez jouée
                    {
                        bufferlist[_idarray][0].Play();
                print(bufferlist[_idarray][i].name);
                //bufferlist[_idarray].RemoveAt(0);
                        
                    }

        } // Fin sur buffer pas vide
    } // FIN IPAUD

    // =========================================================//
    // =========================================================//
    // =========================================================//
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
        } // ===================== //

        // ========== BUFFER ========== //
        if (bufferlist[_idarray].Count == 0)
        {

            // ===== Parcours du tableau pour le remplir ===== //
            if (Theone && bufferlist[_idarray].Count == 0)
            {
                for (int i = 0; i < Theone.sounds.Length; i++)
                {
                    bufferlist[_idarray].Add(Theone.sounds[i]);
                }
            }

        } // ========================== //
       
        for (int z=0; z < bufferlist[_idarray].Count; z++)
        {
            //print(id_array+" -- "+bufferlist[_idarray][z].name);
        }

        if (bufferlist[_idarray].Count > 0)
        {
            int i = 0;

            if (bufferlist[_idarray][0].IsPlaying()) // Si son joué
            {
                
            } else // Si son terminé
            {
                if (holdonbaby)
                {
                    holdonbaby = false;
                    if (bufferlist[_idarray][0].loop != true && // Si musique qui loop pas
                        bufferlist[_idarray][0].currentholding >= bufferlist[_idarray][0].ArrayHoldingNumber) // qui a été trop jouée
                    {
                        bufferlist[_idarray][0].currentholding = 0;
                        bufferlist[_idarray].RemoveAt(0);
                        if (shouldrandom) i = Random.Range(0, bufferlist[_idarray].Count);
                        else i = 0;
                        bufferlist[_idarray][i].Play();
                    }
                    else // Musique qui loop ou musique pas assez jouée
                    {
                        bufferlist[_idarray][0].Play();
                        bufferlist[_idarray][0].currentholding++;
                    }
                } else
                {
                    if (bufferlist[_idarray][0].loop != true) // qui a été trop jouée
                    {
                        bufferlist[_idarray][0].currentholding = 0;
                        bufferlist[_idarray].RemoveAt(0);
                        if (shouldrandom) i = Random.Range(0, bufferlist[_idarray].Count);
                        else i = 0;
                        bufferlist[_idarray][i].Play();
                    }
                    else // Musique qui loop ou musique pas assez jouée
                    {
                        bufferlist[_idarray][0].Play();
                        bufferlist[_idarray][0].currentholding++;
                    }
                }


                

            }


        }

    }

}