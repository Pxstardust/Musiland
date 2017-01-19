using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class Player : MonoBehaviour {

    // === Joueur === //
    [SerializeField]
    GameObject playerprefab;
    GameObject player0;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;

    // === Camera === //
    [SerializeField]
    Camera maincamera;
    Vector3 decalCamOrigine;

    // === Some var === //
    float deathheight = -10;
    float vitesse = 3F;
    float xpos = 0;
    float ypos = 0;
    bool bInAir;
    float slowFactor = 1;
    float jumpslowFactor = 1;
    float mintimejump = 1f;
    float timelastjump;
    float recoveryTime = 1.5f;
    float lastDamage;
    int hp;
    int hpmax = 5;

    // === Enemy === //
    [SerializeField]
    GameObject enemyprefab;
    GameObject enemy0;
    GameObject[] listeEnemy = new GameObject[30];
    float lastEnemy = 0;
    int idEnemy = 0;

    // HUD
    public Text playerpdv;
    [SerializeField]
    public GameObject playerpdvgameobject;
    public Text victoiretext;
    [SerializeField]
    public GameObject victoiretextgameobject;

    // === Keys === //
    private bool KeyShoot;
    private bool KeyDown;
    private GameObject newone;
    private Component currentscript;


    // ===================================================== Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hp = 5;
        decalCamOrigine = maincamera.transform.position - transform.position;
        anim = GetComponent<Animator>();
        anim.enabled = true;
        
    }



    // ===================================================== Update is called once per frame
    void Update () {

        // ================================================================================ //
        // =============================== CONTROLS ======================================= //
        // ================================================================================ //
        
        // =========================== //
        // ========== MUSIC ========== //
        // ===== NEXT MUSIC ===== //
        if (Input.GetButton("ChangeMusicPlus")) {
            
            // ===== CHANGEMENT DES TILES + BCKG ===== //
            GroundTry[] tabmagik = (GroundTry[])FindObjectsOfType(typeof(GroundTry));
            foreach (GroundTry themetile in tabmagik)
            {
                GroundTry script = (GroundTry)themetile.GetComponent(typeof(GroundTry));
                script.ChangetoNext();
            }
            
            
        }

        // ===== PREVIOUS MUSIC ===== //
        if (Input.GetButton("ChangeMusicMinus"))
        {
            // ===== CHANGEMENT DES TILES + BCKG ===== //
            GroundTry[] tabmagik = (GroundTry[])FindObjectsOfType(typeof(GroundTry));
            foreach (GroundTry themetile in tabmagik)
            {
                GroundTry script = (GroundTry)themetile.GetComponent(typeof(GroundTry));
                script.ChangetoPrevious();
            }
        }
        
        // ================= //
        // ===== Death ===== //
        if (sprite.transform.position.y < deathheight) hp = 0; // Mort si en dessous certaine hauteur

        if (hp == 0)
        {
            print("Death");
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        // ================= //

        // ======================================== //
        // ============= Mouvement ================ //
       
        // ===== Left/right ===== //
        if (Input.GetButton("Horizontal"))
        { }
        if (Input.GetAxis("Horizontal") == 0) {
            if (!bInAir) anim.SetBool("isrunning", false);
        } else if (!bInAir) { anim.SetBool("isrunning", true); }
        // ====================== //
    

        // ===== Jump  ===== //
        if (Input.GetButton("Jump") && !bInAir && (Time.time > timelastjump+mintimejump))
        {
            bInAir = true;
            rigid.AddForce(new Vector3(0, jumpslowFactor*300, 0));
            timelastjump = Time.time;
        }
        // ================= //

        // ===== Down ===== //
        if (Input.GetButton("Down"))
        {
            rigid.AddForce(new Vector3(0, -30, 0));
        }
        // =============== //


        maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, transform.position + decalCamOrigine, Time.deltaTime);

        // ========================================
        // ============= HUD ======================
        playerpdv = playerpdvgameobject.GetComponent<Text>(); // Texte pv mob
        playerpdv.text = "HP: " + hp + "/"+hpmax;
    }


    void FixedUpdate()
    {
        rigid.AddForce( 
            (new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f)*300f*Time.deltaTime)
            );
       // sprite.transform.position += new Vector3(Input.GetAxis("Horizontal") * slowFactor * vitesse * Time.deltaTime,0,0); // Position
        if (Input.GetAxis("Horizontal") > 0) sprite.flipX = false;
        if (Input.GetAxis("Horizontal") < 0) sprite.flipX = true;
    }


    // ========================================
    // ============= Collision ================

    void OnCollisionEnter2D(Collision2D collision)
    {

        if ((collision.gameObject.tag == "Sol") || (collision.gameObject.tag == "Slow")) { bInAir = false; anim.SetBool("isjump", false);}
        if (collision.gameObject.tag == "Goal")
        {
            victoiretext = victoiretextgameobject.GetComponent<Text>();
            victoiretext.text = "Victoire !";
        }

    }

    void OnCollisionStay2D(Collision2D collision) // Empêche de ne plus pouvoir jump si atterrissage alors que bouton Jump maintenu
    {
        if ((collision.gameObject.tag == "Sol") ||( collision.gameObject.tag == "Slow" )) { bInAir = false; anim.SetBool("isjump", false); }
        if (collision.gameObject.tag == "Damage" && (Time.time > lastDamage + recoveryTime))
        {
            hp--;
            bInAir = true;
            rigid.AddForce(new Vector3(0, 200, 0));
            lastDamage = Time.time;
        }

        if (collision.gameObject.tag == "Slow") {
            slowFactor = 0.5f;
            jumpslowFactor = 0.8f;
        }
        
    }
    
    void OnCollisionExit2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Slow")
        {
            slowFactor = 1f;
            jumpslowFactor = 1f;
        }

        if ((collision.gameObject.tag == "Sol") || (collision.gameObject.tag == "Slow"))
        {
            bInAir = true;
            anim.SetBool("isjump", true);
        }
        
    }
}
