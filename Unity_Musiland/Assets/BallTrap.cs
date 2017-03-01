using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrap : MonoBehaviour {

	public bool isActive = false;
	Rigidbody2D rigid;

	[SerializeField]
	bool isDangerous;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		if (!isDangerous) {
			Player player = (Player)FindObjectOfType (typeof(Player));
			Physics2D.IgnoreCollision (GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionStay2D(Collision2D collision)
	{
		rigid.AddForce (new Vector3(-1, 0, 0));
	}
}
