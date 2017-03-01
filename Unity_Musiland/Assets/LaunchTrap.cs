using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrap : MonoBehaviour {

	[SerializeField]
	GameObject[] trap;

	Player player;

	// Use this for initialization
	void Start () {
		player = (Player)FindObjectOfType (typeof(Player));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player" && player.GetComponent<Rigidbody2D>().velocity.x > 0) {
			trap[0] = Instantiate (trap[0], new Vector3(168, 47, 0), Quaternion.identity);
			trap[1] = Instantiate (trap[1], new Vector3(168, 49, 0), Quaternion.identity);
			trap[2] = Instantiate (trap[2], new Vector3(168, 50, 0), Quaternion.identity);
			StartCoroutine (LaunchDangerousBall());

		}
	}

	IEnumerator LaunchDangerousBall(){
		yield return new WaitForSeconds (2);
		trap[3] = Instantiate (trap[3], new Vector3(168, 50, 0), Quaternion.identity);
	}
}
