using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevelScript : MonoBehaviour {
    [SerializeField]
    Player player;

	// Use this for initialization
	void Start () {
        //player.IsFestActivable = false;
        //player.IsHellActivable = false;
        player.IsCalmActivable = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
