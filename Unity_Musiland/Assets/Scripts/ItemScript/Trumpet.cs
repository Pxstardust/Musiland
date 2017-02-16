using System.Collections;
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

    MusicSwitcher ThisMusicSwitcher;
	Rigidbody2D rigid2D;
	public Transform groundCheck;
	bool stopCrowd = false;


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
        if (positioncamera.x > 0 && positioncamera.x < 1 && positioncamera.y > 0 && positioncamera.y < 1)
        {
            switch (ThisMusicSwitcher.currentstyle)
            {
			case EnumList.StyleMusic.Hell:
					//if (!base.isfleeing)  Flee desactivé car on peut résoudre l'énigme d'une manière non prévue
                    //{
					Entity_Stop ();
                        //Entity_Flee(target);
                    //}
               		break;

                case EnumList.StyleMusic.Fest:
					//rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
				if (!base.isfollowing && !stopCrowd)
                    {
                        Entity_Stop();
                        Entity_Follow(target);
                    }

                    break;
                case EnumList.StyleMusic.Calm:
                	Entity_Stop();
                    break;
            }
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
	}
}