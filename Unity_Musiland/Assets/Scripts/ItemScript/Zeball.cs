using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeball : Entity
{
    [SerializeField]
    Vector3 Position1;
    [SerializeField]
    Vector3 Position2;
    [SerializeField]
    float Rota1, Rota2;

    [SerializeField]
    float newfollowradius;
    [SerializeField]
    float newspeed;
    [SerializeField]
    float newrotaspeed;
    [SerializeField]
    GameObject target;

	[SerializeField]
	GameObject ignore1;
	[SerializeField]
	GameObject ignore2;

    MusicSwitcher ThisMusicSwitcher;
	Collider2D coll;


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
		coll = GetComponent<Collider2D>();

		Physics2D.IgnoreCollision (coll, ignore1.GetComponent<Collider2D>());
		Physics2D.IgnoreCollision (coll, ignore2.GetComponent<Collider2D>());
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
		if (!base.ispatrol)
		{
			Entity_Patrol(Position1, Position2);

		}
        /*switch (ThisMusicSwitcher.currentstyle)
        {
            case EnumList.StyleMusic.Hell:
                Entity_Stop();
                // + Bloquerses mouvements
                break;
            case EnumList.StyleMusic.Fest:
                Entity_Stop();
                // + Bloquerses mouvements
                break;
            case EnumList.StyleMusic.Calm:
                if (!base.ispatrol)
                {
                    Entity_Patrol(Position1, Position2);

                }


                break;
        }*/


    }

}
