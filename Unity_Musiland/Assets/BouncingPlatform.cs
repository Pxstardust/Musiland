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

	// Use this for initialization
	void Start () {
		playerRigid = player.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject == player) {
			playerRigid.AddForce (new Vector3(bouncingDirection, bouncingPower, 0));
		}
	}
}
