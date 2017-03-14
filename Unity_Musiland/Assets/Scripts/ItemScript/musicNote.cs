using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicNote : MonoBehaviour {

	HUDManager manager;

	// Use this for initialization
	void Start () {
		manager = (HUDManager) FindObjectOfType (typeof(HUDManager));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// ============== Trigger= ================ //
	// ======================================== //
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player") {
			manager.notesQuantity++;
			Destroy (gameObject);
		}
			
	}
}
