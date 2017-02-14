using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {
    public bool IsUsed;
    public Vector3 PositionSave;
    [SerializeField]
    Player player;
    SpriteRenderer spriterenderer;
    [SerializeField]
    Sprite UsedSprite;

	// Use this for initialization
	void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Checking()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!IsUsed)
            {
                player.CurrentRespawnPoint = PositionSave;
                spriterenderer.sprite = UsedSprite;
            }
        }

    }
}
