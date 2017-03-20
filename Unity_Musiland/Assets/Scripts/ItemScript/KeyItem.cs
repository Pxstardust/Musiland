using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour {

    [SerializeField]
    Camera camera;
    [SerializeField]
    GameObject lueur;
    HUDManager manager;
    bool isdone;
    SpriteRenderer sprite;

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
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ============== Trigger= ================ //
    // ======================================== //
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !isdone)
        {
            isdone = true;
            //manager.notesQuantity++;
            audioManager.PlaySound("Reward");
            //Destroy(gameObject);
            sprite.enabled = false;
            lueur.SetActive(false);    

            StartCoroutine(End());
        }

    }

    IEnumerator End()
    {
        var fader = GameObject.FindObjectOfType<SceneFadeInOut>();
        fader.MakeItFade(2);
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndScreen");
    }
}
