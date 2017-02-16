using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : Entity {

	[SerializeField]
	GameObject target;
	[SerializeField]
	Camera maincamera;

	MusicSwitcher ThisMusicSwitcher;


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

		Vector3 positioncamera = maincamera.WorldToViewportPoint(this.transform.position);
		base.Update();
		if (positioncamera.x > 0 && positioncamera.x < 1 && positioncamera.y > -1 && positioncamera.y < 0.1f)
		{
			if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Hell) {

			}
		}
	}
}
