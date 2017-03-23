using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trumpet : Entity
{
    [SerializeField]
    Player player;

    [SerializeField]
    float newfollowradius;
    [SerializeField]
    float newspeed;
    [SerializeField]
    float newrotaspeed;
    [SerializeField]
    public GameObject target;
    [SerializeField]
    Camera maincamera;

	Vector3 scarePoint1;
	Vector3 scarePoint2;

    MusicSwitcher ThisMusicSwitcher;
	Rigidbody2D rigid2D;
	public bool stopCrowd = false;
	public bool scared = false;
	public bool fleeCloud = false;
	Vector3 fleeingPoint;

    [SerializeField]
    GameObject BulleMaison;

    EmotionMaker emotionmaker;

    [SerializeField]
    Collider2D foulecollider;
    //SpriteRenderer sprite;

    public bool Housefound = false;
    bool once = false;

    // ================= //
    // ===== AUDIO ===== //
    AudioManager audioManager;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        emotionmaker = GetComponent<EmotionMaker>();
        ThisMusicSwitcher = GetComponent<MusicSwitcher>();
        speed = newspeed;
        rotaspeed = newrotaspeed;
        //Entity_Follow(target);
        //Entity_Flee(target);
        // Entity_Stay(Position1);
        //followradius = newfollowradius;

        // == AUDIO == //
        audioManager = AudioManager.instance;

        rigid2D = gameObject.GetComponent<Rigidbody2D>();
        //rigid2D.constraints = RigidbodyConstraints2D.FreezeAll;

        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), foulecollider, true);

    }

    // Update is called once per frame
    protected override void Update()
    {

        Vector3 positioncamera = maincamera.WorldToViewportPoint(this.transform.position);
        base.Update();

        if (Housefound) {
            if (!once) { once = true; Entity_Stop(); }
            Entity_GoTo(target.transform.position, 0);
        }
        else if (!scared && !fleeCloud) {
			if (positioncamera.x > 0 && positioncamera.x < 1 && positioncamera.y > 0 && positioncamera.y < 1) {
				switch (ThisMusicSwitcher.currentstyle) {
				case EnumList.StyleMusic.Hell:
					if (!base.isfleeing && !base.isgoto && !stopCrowd) {  //Flee desactivé car on peut résoudre l'énigme d'une manière non prévue
						Entity_Stop ();
						Entity_Flee (target);

                        }
                        if (emotionmaker.currentemotion != EnumList.Emotion.Panic) emotionmaker.stopEmotion();
                        emotionmaker.MakeEmotion(EnumList.Emotion.Panic);
                        break;

				case EnumList.StyleMusic.Fest:

                        if (emotionmaker.currentemotion != EnumList.Emotion.Laugh) emotionmaker.stopEmotion();
                        emotionmaker.MakeEmotion(EnumList.Emotion.Laugh);
                        
                        //rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        if (!base.isfollowing && !stopCrowd) {
						Entity_Stop ();
						Entity_Follow (target);
                            if (BulleMaison) Destroy(BulleMaison);
					}

					break;
				case EnumList.StyleMusic.Calm:
                        emotionmaker.stopEmotion();
                        Entity_Stop ();
					break;
				}
			}
		}

		if (scared) {
            if (emotionmaker.currentemotion != EnumList.Emotion.Panic) emotionmaker.stopEmotion();
            emotionmaker.MakeEmotion(EnumList.Emotion.Panic);
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
            scarePoint1 = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            scarePoint2 = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            Destroy (collision.gameObject);
		}

		if (collision.gameObject.name == "YellowCloud1" && ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Hell) {


		}

        if (collision.gameObject.tag == "Player")
        {

            if (!player.isdisturbed)
            {
                player.isdisturbed = true;
                float avancement = 5;
                switch (ThisMusicSwitcher.currentstyle)
                {
                    case EnumList.StyleMusic.Hell:
                        avancement = audioManager.GetSoundTime("Hell_BGM");
                        audioManager.StopSound("Hell_BGM");
                        audioManager.PlaySoundAtTime("Hell_Foule", avancement);
                        break;

                    case EnumList.StyleMusic.Fest:
                        avancement = audioManager.GetSoundTime("Fest_BGM");
                        audioManager.StopSound("Fest_BGM");
                        audioManager.PlaySoundAtTime("Fest_Foule", avancement);
                        break;

                    case EnumList.StyleMusic.Calm:
                        avancement = audioManager.GetSoundTime("Calm_BGM");
                        audioManager.StopSound("Calm_BGM");
                        audioManager.PlaySoundAtTime("Calm_Foule", avancement);
                        break;
                }
                
            }

        }

	}

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (player.isdisturbed)
            {
                player.isdisturbed = false;
                float avancement = 5;
                switch (ThisMusicSwitcher.currentstyle)
                {
                    case EnumList.StyleMusic.Hell:
                        avancement = audioManager.GetSoundTime("Hell_Foule");
                        audioManager.PlaySoundAtTime("Hell_BGM",avancement);
                        audioManager.StopSound("Hell_Foule");
                        break;

                    case EnumList.StyleMusic.Fest:
                        avancement = audioManager.GetSoundTime("Fest_Foule");
                        audioManager.PlaySoundAtTime("Fest_BGM", avancement);
                        audioManager.StopSound("Fest_Foule");
                        break;

                    case EnumList.StyleMusic.Calm:
                        avancement = audioManager.GetSoundTime("Calm_Foule");
                        audioManager.PlaySoundAtTime("Calm_BGM", avancement);
                        audioManager.StopSound("Calm_Foule");
                        break;
                }
                
            }

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
					speed = 8;
                    emotionmaker.stopEmotion();
				}
			}
		}

	}
}