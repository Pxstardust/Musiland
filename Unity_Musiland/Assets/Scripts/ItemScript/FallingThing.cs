using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingThing : Entity {

    [SerializeField]
    float fallHeight;
    bool isfallen = false;
    Vector3 fallPoint;

    [SerializeField]
    public string SprotchSongName;
    [SerializeField]
    public string FallSongName;

    // ================= //
    // ===== AUDIO ===== //
    AudioManager audioManager;

    // Use this for initialization
    void Start () {
        // == AUDIO == //
        audioManager = AudioManager.instance;
        if (audioManager == null) Debug.LogError(this + " n'a pas trouvé d'AudioManager");

        fallPoint = new Vector3(this.transform.position.x, fallHeight, this.transform.position.z);
	}

    // Update is called once per frame
    protected override void Update()
    {


        base.Update();
    }


    public void Fall()
    {
       if (!isfallen)
        {
            StartCoroutine(FallSound());
            Entity_GoTo(fallPoint, 140);
            isfallen = true;
        }


    }

    IEnumerator FallSound()
    {
        audioManager.PlaySound(FallSongName);
        yield return new WaitForSeconds(0.35f);
        audioManager.PlaySound(SprotchSongName);
    }
}
