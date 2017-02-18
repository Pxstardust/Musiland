﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet : Entity
{


    [SerializeField]
    float newfollowradius;
    [SerializeField]
    float newspeed;
    [SerializeField]
    float newrotaspeed;
    [SerializeField]
    GameObject target;
    [SerializeField]
    Camera maincamera;
	[SerializeField]
	Vector3 scarePoint1;
	[SerializeField]
	Vector3 scarePoint2;

    MusicSwitcher ThisMusicSwitcher;
	Rigidbody2D rigid2D;
	public bool stopCrowd = false;
	public bool scared = false;
	public bool fleeCloud = false;
	Vector3 fleeingPoint;


    // Use this for initialization
    void Start()
    {
        ThisMusicSwitcher = GetComponent<MusicSwitcher>();
        speed = newspeed;
        rotaspeed = newrotaspeed;
        //Entity_Follow(target);
        //Entity_Flee(target);
        // Entity_Stay(Position1);
        //followradius = newfollowradius;
		rigid2D = gameObject.GetComponent<Rigidbody2D>();
		rigid2D.constraints = RigidbodyConstraints2D.FreezeAll;

    }

    // Update is called once per frame
    protected override void Update()
    {
		
        Vector3 positioncamera = maincamera.WorldToViewportPoint(this.transform.position);
        base.Update();
		if (!scared && !fleeCloud) {
			if (positioncamera.x > 0 && positioncamera.x < 1 && positioncamera.y > 0 && positioncamera.y < 1) {
				switch (ThisMusicSwitcher.currentstyle) {
				case EnumList.StyleMusic.Hell:
					if (!base.isfleeing && !base.isgoto) {  //Flee desactivé car on peut résoudre l'énigme d'une manière non prévue
						Entity_Stop ();
						Entity_Flee (target);
					}
					break;

				case EnumList.StyleMusic.Fest:
					//rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
					if (!base.isfollowing && !stopCrowd) {
						Entity_Stop ();
						Entity_Follow (target);
					}

					break;
				case EnumList.StyleMusic.Calm:
					Entity_Stop ();
					break;
				}
			}
		}

		if (scared) {
			if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm) {
				StartCoroutine (CalmModeFocus ());
			} else {
				speed = 8;
				if (!base.ispatrol) {  //Flee desactivé car on peut résoudre l'énigme d'une manière non prévue
					Entity_Stop ();
					Entity_Patrol (scarePoint1, scarePoint2);
				}
			}
		}

		if (fleeCloud) {
			if (transform.position != fleeingPoint) {
				Entity_Stop ();	
				Entity_GoTo (fleeingPoint, 0);
			}
			else
				fleeCloud = false;
		}
        
    }

	// ============= Collision ================ //
	// ======================================== //
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "StopCrowd") {
			stopCrowd = true;
			Entity_Stop ();
		}

		if (collision.gameObject.name == "ScareCrowd") {
			scared = true;

			Destroy (collision.gameObject);
		}

		if (collision.gameObject.name == "YellowCloud1" && ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Hell) {
			fleeCloud = true;
			fleeingPoint = transform.position + new Vector3 (-5, 0, 0);
		}
	}

	IEnumerator CalmModeFocus(){
		yield return new WaitForSeconds (2);
		if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm) {
			speed = 5;
			yield return new WaitForSeconds (2);
			if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm) {
				speed = 3;
				yield return new WaitForSeconds (2);
				if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm) {
					scared = false;
					speed = 5;
				}
			}
		}

	}
}