using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour {

    
    HUDManager manager;

    // ================= //
    // ===== AUDIO ===== //
    AudioManager audioManager;


    // Use this for initialization
    void Start()
    {
        // == AUDIO == //
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError(this + " n'a pas trouvé d'AudioManager");
        manager = (HUDManager)FindObjectOfType(typeof(HUDManager));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ============== Trigger= ================ //
    // ======================================== //
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //manager.notesQuantity++;
            audioManager.PlaySound("Reward");
            Destroy(gameObject);
        }

    }
}
