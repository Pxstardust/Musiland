using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrap : MonoBehaviour {

	[SerializeField]
	GameObject[] traps;
	GameObject[] balls = new GameObject[4];

	SpriteRenderer renderer;

	Player player;
	bool cooldown = false;

	// Use this for initialization
	void Start () {
		player = (Player)FindObjectOfType (typeof(Player));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.gameObject.name == "Player" && player.GetComponent<Rigidbody2D>().velocity.x > 0 && !cooldown) {
			cooldown = true;

			balls[0] = Instantiate (traps[0], new Vector3(125, 3, 0), Quaternion.identity);
			balls[1] = Instantiate (traps[1], new Vector3(126, 3, 0), Quaternion.identity);
			balls[2] = Instantiate (traps[2], new Vector3(127, 3, 0), Quaternion.identity);

			if (player.playercurrentstyle == EnumList.StyleMusic.Calm) {
				renderer = balls [0].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[0].GetComponent<MusicSwitcher> ().CalmTile;
				renderer = balls [1].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[1].GetComponent<MusicSwitcher> ().CalmTile;
				renderer = balls [2].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[2].GetComponent<MusicSwitcher> ().CalmTile;
			} else if (player.playercurrentstyle == EnumList.StyleMusic.Fest) {
				renderer = balls [0].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[0].GetComponent<MusicSwitcher> ().FestTile;
				renderer = balls [1].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[1].GetComponent<MusicSwitcher> ().FestTile;
				renderer = balls [2].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[2].GetComponent<MusicSwitcher> ().FestTile;
			} else {
				renderer = balls [0].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[0].GetComponent<MusicSwitcher> ().HellTile;
				renderer = balls [1].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[1].GetComponent<MusicSwitcher> ().HellTile;
				renderer = balls [2].gameObject.GetComponent<SpriteRenderer> ();
				renderer.sprite = balls[2].GetComponent<MusicSwitcher> ().HellTile;
			}


			StartCoroutine (LaunchDangerousBall());

		}
	}

	IEnumerator LaunchDangerousBall(){
		yield return new WaitForSeconds (1f);
		balls[3] = Instantiate (traps[3], new Vector3(134, 5, 0), Quaternion.identity);
		if (player.playercurrentstyle == EnumList.StyleMusic.Calm) {
			renderer = balls [3].gameObject.GetComponent<SpriteRenderer> ();
			renderer.sprite = balls[3].GetComponent<MusicSwitcher> ().CalmTile;
		} else if (player.playercurrentstyle == EnumList.StyleMusic.Fest) {
			renderer = balls [3].gameObject.GetComponent<SpriteRenderer> ();
			renderer.sprite = balls[3].GetComponent<MusicSwitcher> ().FestTile;
		} else {
			renderer = balls [3].gameObject.GetComponent<SpriteRenderer> ();
			renderer.sprite = balls[3].GetComponent<MusicSwitcher> ().HellTile;
		}
		cooldown = false;
	}
}
