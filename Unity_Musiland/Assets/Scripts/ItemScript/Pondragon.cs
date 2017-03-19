using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pondragon : Entity {

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
    Animator anim;
    float timerdozing = 0;

    [SerializeField]
    private PolygonCollider2D[] colliders;
    private int currentColliderIndex = 0;

    EmotionMaker emotionmaker;

    [SerializeField]
    private Collider2D downcollider;

    [SerializeField]
    GameObject EmotionSpawn;

    [SerializeField]
    private Collider2D upcollider;
    private float Timebed;


    // Use this for initialization
    protected override void Start () {
        base.Start();
        emotionmaker = GetComponent<EmotionMaker>();
        ThisMusicSwitcher = GetComponent<MusicSwitcher>();
        anim = GetComponent<Animator>();
        downcollider.enabled = false;
    }

    protected override void Update()
    {


        if (emotionmaker.currentemotion != EnumList.Emotion.Angry &&
            ThisMusicSwitcher.currentstyle != EnumList.StyleMusic.Calm) {
            emotionmaker.stopEmotion();
            emotionmaker.MakeEmotion(EnumList.Emotion.Angry);
        }  

        if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm)
        {
            if (emotionmaker.currentemotion== EnumList.Emotion.Angry) { emotionmaker.stopEmotion(); }
        }

        
        Vector3 positioncamera = maincamera.WorldToViewportPoint(this.transform.position);
        base.Update();
        if (!sleeping)
        {
            if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Hell) { anim.SetBool("IsRaging", true); }
            else { anim.SetBool("IsRaging", false); }

            if (positioncamera.x > 0 && positioncamera.x < 0.7f && positioncamera.y > -1 && positioncamera.y < 2)
            {
                if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm)
                {
                    if (!dozing) {
                        StartCoroutine(checkCalmDuration());
                    } else
                    {
                        timerdozing += Time.deltaTime;
                        //Entity_GoTo(fallPoint, 180);
                    }

                }
                else {
                    timerdozing = 0;
                    dozing = false;
                    anim.SetBool("IsDrooling", false);
                    if (emotionmaker.currentemotion == EnumList.Emotion.Sleepy) { emotionmaker.stopEmotion();}
                }

            }
            else {
                //Entity_GoTo(beginPoint, 120);
                dozing = false;
            }
        } else
        {
            Timebed += Time.deltaTime;
            if (Timebed > 1.02)
            {
                SetDragonFlatCollider();
                EmotionSpawn.transform.localPosition = new Vector3(-3.6f, -3.2f, 0);
            }
        }

       

        if (timerdozing > 2)
        {
            sleeping = true;
            
            anim.SetBool("IsAsleep", true);
            Destroy(crowdCheck.gameObject);
            trumpet.stopCrowd = false;
        }

    }

    IEnumerator checkCalmDuration()
    {

        //launch beginning of desctruction animation
        yield return new WaitForSeconds(1);
        if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm)
        {
            //lauch destruction animation

            emotionmaker.MakeEmotion(EnumList.Emotion.Sleepy);
            anim.SetBool("IsDrooling", true);
            dozing = true;
        }
    }

    public void SetDragonFlatCollider()
    {
        upcollider.enabled = false;
        downcollider.enabled = true;
    }
}
