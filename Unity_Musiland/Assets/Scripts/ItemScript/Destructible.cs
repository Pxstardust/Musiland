using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ======================================================================= //
// ========== Script qui rend un objet destructible avec un dash ========= //
// ============== Juste ajouter le script sur l'objet voulu ============== //

public class Destructible : MonoBehaviour {

    [SerializeField]
    ParticleSystem particleemitterGO;
    SpriteRenderer spriterenderer;
    [SerializeField]
    Collider2D Maincollider;
    Animator anim;
    ParticleSystem newparticle;
    AudioSource audio;
    [SerializeField]
    MusicSwitcher theMS;
    public bool isdestroyed;

    [SerializeField]
    bool dontplaymysound;

    // Use this for initialization
    void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        isdestroyed = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isdestroyed)
        {
            spriterenderer.enabled = false;
            Maincollider.enabled = false;
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        /*
        if ((collision.gameObject.tag == "Player"))
        {
            GameObject goplayer = collision.gameObject; // Recup' GO de l'objet collider (player)
            Player playerscript = goplayer.GetComponent("Player") as Player; // Recup' le sript de l'objet
            if (playerscript)
            {
                if (playerscript.bRun || playerscript.bVDash) // Si le joueur est en plein dash
                {
                    Destruction();
                }
            }
            
        }
        */
    }

    // ===== Fonction qui détruit l'objet ===== //
    public void Destruction()
    {
        //Destroy(gameObject);
        if (!isdestroyed)
        {
            Destroy(theMS);
            spriterenderer.sprite = null;
            Maincollider.enabled = false;
            if (!dontplaymysound) audio.Play();

            if (particleemitterGO)
            {
                newparticle = Instantiate(particleemitterGO, new Vector3(transform.position.x, transform.position.y, transform.position.y), Quaternion.identity);
            }
            isdestroyed = true;

        }

    }
}
