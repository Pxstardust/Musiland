using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBridge : Entity {

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


	MusicSwitcher ThisMusicSwitcher;
	bool fall = false;
	bool dozing = false;
	bool sleeping = false;
	Vector3 beginPoint;


	// Use this for initialization
	void Start()
	{
		ThisMusicSwitcher = GetComponent<MusicSwitcher>();
		//Entity_Follow(target);
		//Entity_Flee(target);
		// Entity_Stay(Position1);
		//followradius = newfollowradius;
		beginPoint = transform.position;

	}

	// Update is called once per frame
	protected override void Update()
	{

		Vector3 positioncamera = maincamera.WorldToViewportPoint(this.transform.position);
		base.Update();
		if (!sleeping) {
			if (positioncamera.x > 0 && positioncamera.x < 1.2f && positioncamera.y > -1 && positioncamera.y < 2) {
				if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm) {
					if (!dozing)
						StartCoroutine (checkCalmDuration ());
					else
						Entity_GoTo (fallPoint, 180);
				} else {
					Entity_GoTo (beginPoint, 120);
					dozing = false;
				}

			} else {
				Entity_GoTo (beginPoint, 120);
				dozing = false;
			}
		}

		if (transform.position == fallPoint) {
			sleeping = true;
			Destroy (crowdCheck.gameObject);
			trumpet.stopCrowd = false;
		}
			
	}

	IEnumerator checkCalmDuration(){

		//launch beginning of desctruction animation
		yield return new WaitForSeconds (1);
		if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm) {
			//lauch destruction animation
			dozing = true;
		}
	}

}