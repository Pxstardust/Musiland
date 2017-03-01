using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrap : MonoBehaviour {

	[SerializeField]
	GameObject[] traps;
	GameObject[] balls = new GameObject[6];

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
			if (balls[0] != null) {
				Destroy (balls [0].gameObject);
				Destroy (balls [1].gameObject);
				Destroy (balls [2].gameObject);
				Destroy (balls [3].gameObject);
			}

			balls[0] = Instantiate (traps[0], new Vector3(160, 47, 0), Quaternion.identity);
			balls[1] = Instantiate (traps[1], new Vector3(162, 49, 0), Quaternion.identity);
			balls[2] = Instantiate (traps[2], new Vector3(164, 50, 0), Quaternion.identity);
			StartCoroutine (LaunchDangerousBall());

		}
	}

	IEnumerator LaunchDangerousBall(){
		yield return new WaitForSeconds (2f);
		balls[3] = Instantiate (traps[3], new Vector3(172, 45, 0), Quaternion.identity);
		/*balls[4] = Instantiate (traps[3], new Vector3(172, 50, 0), Quaternion.identity);
		balls[5] = Instantiate (traps[3], new Vector3(172, 55, 0), Quaternion.identity);*/
	}
}
