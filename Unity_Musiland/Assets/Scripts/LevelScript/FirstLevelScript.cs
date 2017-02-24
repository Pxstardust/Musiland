﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevelScript : MonoBehaviour {
    [SerializeField]
    Player player;

    [SerializeField]
    TriggeredArea TriggerIncendie;

    bool OnceIncendie = false;

        [SerializeField]
    FallingThing Poutre;

	// Use this for initialization
	void Start () {
        //player.IsFestActivable = false;
        //player.IsHellActivable = false;
        player.IsCalmActivable = true;
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetButtonDown("DebugKey"))
        {
            FlameResize[] tabmagik = (FlameResize[])FindObjectsOfType(typeof(FlameResize)); // Recup' tout les items avec le script de changement
            foreach (FlameResize themetile in tabmagik) // Parcours
            {
                themetile.Resize(new Vector3(0,0,0));
            }

            Poutre.Fall();
        }

        if (TriggerIncendie.IAMTRIGGERED && !OnceIncendie)
        {
            Poutre.Fall();
            OnceIncendie = true;
        }
        
	}
}
