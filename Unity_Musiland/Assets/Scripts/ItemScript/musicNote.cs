using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicNote : MonoBehaviour {

	HUDManager manager;
    Player player;
    // ================= //
    // ===== AUDIO ===== //
    AudioManager audioManager;

    // Use this for initialization
    void Start () {
        player = (Player)FindObjectOfType(typeof(Player));
		manager = (HUDManager) FindObjectOfType (typeof(HUDManager));
        // == AUDIO == //
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError(this + " n'a pas trouvé d'AudioManager");
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
            if (player.timerlastnote < Time.time - 2) { audioManager.ResetThatArray(5); }
            player.timerlastnote = Time.time;
            audioManager.ImmediatelyPAUD(5);

			Destroy (gameObject);
		}
			
	}
}
