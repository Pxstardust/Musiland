using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSphere : MonoBehaviour {

    [SerializeField]
    EnumList.StyleMusic UnlockedTheme;
    [SerializeField]
    Player player;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("ding");
        if (collision.tag == "Player")
        {
            print("dong");
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
