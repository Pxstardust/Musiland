using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevelScript : MonoBehaviour {
    [SerializeField]
    Player player;

    [SerializeField]
    Trumpet foule;

    [SerializeField]
    TriggeredArea TriggerIncendie;
    [SerializeField]
    TriggeredArea TriggerHouseEnd;
    [SerializeField]
    TriggeredArea TriggerInferno;

    bool OnceIncendie = false;
    bool IsOnFire = true;
    bool Startedcalm = false;
    bool OnceHouse = false;


    // ================= //
    // ===== AUDIO ===== //
    AudioManager audioManager;

    [SerializeField]
    FallingThing Poutre;

	// Use this for initialization
	void Start () {
        //player.IsFestActivable = false;
        //player.IsHellActivable = false;
        player.IsCalmActivable = true;

        // == AUDIO == //
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError(this + " n'a pas trouvé d'AudioManager");
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetButtonDown("DebugKey"))
        {
        }

        // ============================= //
        // ========= INCENDIE ========== //
        if (TriggerIncendie.IAMTRIGGERED && !OnceIncendie)
        {
            Poutre.Fall();
            OnceIncendie = true;
        }

        if (OnceIncendie && player.playercurrentstyle == EnumList.StyleMusic.Calm && IsOnFire)
        {
            Startedcalm = true;
            //IsOnFire = false;
            FlameResize[] tabmagik = (FlameResize[])FindObjectsOfType(typeof(FlameResize)); // Recup' tout les items avec le script de changement
            foreach (FlameResize themetile in tabmagik) // Parcours
            {
                themetile.Resize(new Vector3(0, 0, 0));
            }

            FadeOut[] tab2 = (FadeOut[])FindObjectsOfType(typeof(FadeOut));
            foreach (FadeOut fadetile in tab2)
            {
                fadetile.Fadeto(0);
            }
        }

        if (Startedcalm & IsOnFire)
        {
            FlameResize[] tabmagik = (FlameResize[])FindObjectsOfType(typeof(FlameResize)); // Recup' tout les items avec le script de changement
            foreach (FlameResize themetile in tabmagik) // Parcours
            {
               if (themetile.done)
                {
                    audioManager.StopSoundIfPlaying("Inferno");
                    //print("ok");
                    IsOnFire = false;
                    break;
                }
            }
        }

        // Si flamme pas éteinte et switch musique
        if (Startedcalm && IsOnFire && player.playercurrentstyle != EnumList.StyleMusic.Calm)
        {
            FlameResize[] tabmagik = (FlameResize[])FindObjectsOfType(typeof(FlameResize)); // Recup' tout les items avec le script de changement
            foreach (FlameResize themetile in tabmagik) // Parcours
            {
                themetile.Resetsize();
            }
        }

        // ============================= //
        // ============================= //

        // =========================== //
        // ========== HOUSE ========== //

        if (TriggerHouseEnd.IAMTRIGGERED && !OnceHouse)
        {
            foule.Housefound = true;
            foule.target = TriggerHouseEnd.gameObject;
        }

        if (TriggerInferno.IAMTRIGGERED && IsOnFire)
        {
            audioManager.PlaySoundIfNoPlaying("Inferno");
        }

        if (!TriggerInferno.IAMTRIGGERED)
        {
            audioManager.StopSoundIfPlaying("Inferno");
        }

    }
}
