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


    // ================= //
    // ===== AUDIO ===== //
    AudioManager audioManager;

    // Use this for initialization
    void Start () {
		playerRigid = player.GetComponent<Rigidbody2D> ();
        // == AUDIO == //
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError(this + " n'a pas trouvé d'AudioManager");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
			playerRigid = collision.gameObject.GetComponent<Rigidbody2D> ();
        audioManager.PlaySound("Bounce");
			playerRigid.AddForce (new Vector3(bouncingDirection, bouncingPower, 0));
	}
}
