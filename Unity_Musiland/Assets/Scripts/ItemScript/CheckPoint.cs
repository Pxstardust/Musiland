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
    Animator anim;

    // ================= //
    // ===== AUDIO ===== //
    AudioManager audioManager;

    // Use this for initialization
    void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // == AUDIO == //
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError(this + " n'a pas trouvé d'AudioManager");
        // == AUDIO == //
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
                audioManager.PlaySoundIfNoPlaying("Checkpoint_Sound");
                //int hash = anim.StringToHash("Checkpointanim");
                //anim.Play(hash, 0, 1.0f);
                IsUsed = true;
                anim.SetBool("IsActivated", true);
                player.CurrentRespawnPoint = PositionSave;
                //spriterenderer.sprite = UsedSprite;
                spriterenderer.sprite = null;
            }
        }

    }
}
