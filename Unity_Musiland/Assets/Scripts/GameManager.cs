using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        }
    }

    public string spawnsoundname;
    private AudioManager audioManager;
	// Use this for initialization

	void Start () {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager non trouvé");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
