using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : Entity {

	[SerializeField]
	GameObject target;
	[SerializeField]
	Camera maincamera;
	[SerializeField]
	Vector3 fallPoint;
	[SerializeField]
	Trumpet trumpet;
	[SerializeField]
	GameObject crowdCheck;

    [SerializeField]
    Collider2D collider;

	[SerializeField]
	FallingRock fallingRock1;
	[SerializeField]
	FallingRock fallingRock2;



	MusicSwitcher ThisMusicSwitcher;
	bool fall = false;


	// Use this for initialization
	void Start()
	{
		ThisMusicSwitcher = GetComponent<MusicSwitcher>();
		//Entity_Follow(target);
		//Entity_Flee(target);
		// Entity_Stay(Position1);
		//followradius = newfollowradius;

	}

	// Update is called once per frame
	protected override void Update()
	{

	
		base.Update();
	}


   void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.transform.position.y > transform.position.y)
            {
				fallingRock1.Fall ();
				fallingRock2.Fall ();
                Destroy(crowdCheck.gameObject);
                trumpet.stopCrowd = false;
            }

        }
    }

	public void Fall(){
		Entity_GoTo(fallPoint, 0);
	}

	IEnumerator checkHellDuration(){
		
		//launch beginning of desctruction animation
		yield return new WaitForSeconds (2);
		if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Hell) {
			//lauch destruction animation
			Entity_GoTo(fallPoint, 0);
			Destroy (crowdCheck.gameObject);
			trumpet.stopCrowd = false;
			//Entity_Stop ();
		}
	}
}
