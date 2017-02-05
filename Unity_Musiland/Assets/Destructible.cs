using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ======================================================================= //
// ========== Script qui rend un objet destructible avec un dash ========= //
// ============== Juste ajouter le script sur l'objet voulu ============== //

public class Destructible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter2D(Collision2D collision)
    {

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

    }

    // ===== Fonction qui détruit l'objet ===== //
    public void Destruction()
    {
        Destroy(gameObject);
        // -- Jouer un son -- //
        // -- Jouer une anim'/particule -- //
    }
}
