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

	MusicSwitcher ThisMusicSwitcher;

	// Use this for initialization
	void Start () {
		ThisMusicSwitcher = joueur.GetComponent<MusicSwitcher> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Tueur)
        {
			if (styleNeeded == ThisMusicSwitcher.currentstyle) {
				Destroy(gameObject);
			}
            // Ajouter animation
            // Ajouter son
        }
    }
}
