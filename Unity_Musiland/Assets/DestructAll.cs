using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructAll : MonoBehaviour {

	[SerializeField]
	GameObject allRocks;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Launch(){
		Destructible[] destructibles = allRocks.GetComponentsInChildren<Destructible> ();
		foreach (Destructible destrucible in destructibles) {
			destrucible.Destruction ();
		}
	}
}
