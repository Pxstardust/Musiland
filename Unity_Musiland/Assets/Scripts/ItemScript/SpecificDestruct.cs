﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificDestruct : MonoBehaviour {
    [SerializeField]
    GameObject Tueur;
	[SerializeField]
	EnumList.StyleMusic styleNeeded;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Tueur)
        {
			if (styleNeeded == EnumList.StyleMusic.Fest) {
				Destroy(gameObject);
			}
            // Ajouter animation
            // Ajouter son
        }
    }
}
