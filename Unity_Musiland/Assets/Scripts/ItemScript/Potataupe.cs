using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potataupe : MonoBehaviour {

    [SerializeField]
    GameObject Tueur;
    [SerializeField]
    EnumList.StyleMusic styleNeeded;
    [SerializeField]
    GameObject joueur;
    float radiusdedestruction;
    [SerializeField]
    CircleCollider2D MainCollider;
    [SerializeField]
    Collider2D OtherCollider;

    MusicSwitcher ThisMusicSwitcher;
    Animator anim;

    bool isdone = false;

    // Use this for initialization
    void Start()
    {
        ThisMusicSwitcher = joueur.GetComponent<MusicSwitcher>();
        radiusdedestruction = MainCollider.radius;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ThisMusicSwitcher.currentstyle == EnumList.StyleMusic.Calm && !isdone)
        {
            anim.SetBool("A_Freeze", true);
        } else if (!isdone)
        {
            anim.SetBool("A_Freeze", false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Tueur)
        {

            if ((collision.gameObject.transform.position.x > transform.position.x - 9) &&
                (collision.gameObject.transform.position.x < transform.position.x + 9) &&
                (collision.gameObject.transform.position.y > transform.position.y - 9) &&
                (collision.gameObject.transform.position.y < transform.position.y + 9))
            {
                if (styleNeeded == ThisMusicSwitcher.currentstyle)
                {
                    anim.SetBool("A_Dig", true);
                    MainCollider.enabled = false;
                    OtherCollider.enabled = false;
                    isdone = true;
                    //Destroy(gameObject);
                }
            }

            // Ajouter animation
            // Ajouter son
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == Tueur)
        {

            if ((collision.gameObject.transform.position.x > transform.position.x - radiusdedestruction) &&
                (collision.gameObject.transform.position.x < transform.position.x + radiusdedestruction) &&
                (collision.gameObject.transform.position.y > transform.position.y - radiusdedestruction) &&
                (collision.gameObject.transform.position.y < transform.position.y + radiusdedestruction))
            {
                if (styleNeeded == ThisMusicSwitcher.currentstyle)
                {
                    anim.SetBool("A_Dig", true);
                    MainCollider.enabled = false;
                    OtherCollider.enabled = false;
                    isdone = true;
                    //Destroy(gameObject);
                }
            }

            // Ajouter animation
            // Ajouter son
        }
    }
}
