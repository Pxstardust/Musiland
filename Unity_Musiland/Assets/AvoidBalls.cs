using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidBalls : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.name != "Player"){
			Physics2D.IgnoreCollision (GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
		}
	}
}
