using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPlatform : MonoBehaviour {

	[SerializeField]
	float bouncingPower;
	[SerializeField]
	float bouncingDirection = 0;

	[SerializeField]
	GameObject player;
	Rigidbody2D playerRigid;

    [SerializeField]
    GameObject KillerZone;
    MusicSwitcher ThisMusicSwitcher;

    // ================= //
    // ===== AUDIO ===== //
    AudioManager audioManager;

    // Use this for initialization
    void Start () {
        ThisMusicSwitcher = GetComponent<MusicSwitcher>();
        playerRigid = player.GetComponent<Rigidbody2D> ();
        // == AUDIO == //
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError(this + " n'a pas trouvé d'AudioManager");
    }
	
	// Update is called once per frame
	void Update () {
        if (KillerZone)
        {
            if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Fest)
            {
                KillerZone.SetActive(true);
            }
            else
            {
                KillerZone.SetActive(false);
            }
        }

	}

	void OnCollisionEnter2D(Collision2D collision)
	{
			playerRigid = collision.gameObject.GetComponent<Rigidbody2D> ();
        audioManager.PlaySound("Bounce");
			playerRigid.AddForce (new Vector3(bouncingDirection, bouncingPower, 0));
	}

}
