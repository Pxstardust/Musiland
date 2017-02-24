using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificDestruct : MonoBehaviour {
    [SerializeField]
    GameObject Tueur;
	[SerializeField]
	EnumList.StyleMusic styleNeeded;
	[SerializeField]
	GameObject joueur;
    float radiusdedestruction;
    [SerializeField]
    CircleCollider2D MainCollider;

	MusicSwitcher ThisMusicSwitcher;

	// Use this for initialization
	void Start () {
		ThisMusicSwitcher = joueur.GetComponent<MusicSwitcher> ();
        radiusdedestruction = MainCollider.radius;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Tueur)
        {

            if ( (collision.gameObject.transform.position.x > transform.position.x-9) &&
                (collision.gameObject.transform.position.x < transform.position.x+9) &&
                (collision.gameObject.transform.position.y > transform.position.y-9) &&
                (collision.gameObject.transform.position.y < transform.position.y + 9))
            {
                if (styleNeeded == ThisMusicSwitcher.currentstyle)
                {
                    Destroy(gameObject);
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
                    Destroy(gameObject);
                }
            }

            // Ajouter animation
            // Ajouter son
        }
    }
}
