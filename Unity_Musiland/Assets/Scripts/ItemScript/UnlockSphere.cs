using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSphere : MonoBehaviour {

    [SerializeField]
    EnumList.StyleMusic UnlockedTheme;
    [SerializeField]
    Player player;
    AudioSource audio;
    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            //audio.Play();

            switch (UnlockedTheme)
            {
                case EnumList.StyleMusic.Hell:
                    player.IsHellActivable = true;
                    break;
                case EnumList.StyleMusic.Fest:
                    player.IsFestActivable = true;
                    break;
                case EnumList.StyleMusic.Calm:
                    player.IsCalmActivable = true;
                    break;
            }

            // Play song
            Destroy(this.gameObject);
        }

    }

}
