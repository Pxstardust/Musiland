using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class Player : MonoBehaviour {
    // ================== //
    // ===== Joueur ===== //
    [SerializeField]
    GameObject playerprefab;
    GameObject player0;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;

    // ================== //
    // ===== Camera ===== //
    [SerializeField]
    Camera maincamera;
    Vector3 decalCamOrigine;

    // ===================== //
    // ===== VARIABLES ===== //
    // ===================== //
    // === Some var === //
    float xpos = 0;
    float ypos = 0;
    float recoveryTime = 1.5f;
    float lastDamage;
    int hp;
    int hpmax = 5;

    // === Style Var === //
    public EnumList.StyleMusic playercurrentstyle = EnumList.StyleMusic.Hell;
    float changeTime = 1; // Cooldown pour le changement de style

    // === Keys === //
    private bool KeyShoot;
    private bool KeyDown;

    // === Controller Var === //
    bool bInAir;
    bool bRun;
    float lastchange = 0; // Date du dernier changement
    float initialgravity = 1; // Gravity Scale 
    float deathheight = -10; // Hauteur avant de mourir
    float mintimejump = 1f; // Durée entre deux sauts
    float timelastjump; // Date du dernier saut
    float doubletapcooldown = 0.5f;
    float tapcount = 0;

    // =============== //
    // ===== HUD ===== //
    public Text playerpdv;
    [SerializeField]
    public GameObject playerpdvgameobject;
    public Text victoiretext;
    [SerializeField]
    public GameObject victoiretextgameobject;

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
        if (doubletapcooldown > 0) { doubletapcooldown -= Time.deltaTime; }

        // ================================================================================ //
        // =============================== CONTROLS ======================================= //
        // ================================================================================ //

        // =========================================================================== //
        // ================================ MUSIC ==================================== //
        // === NOTE : PAS DE PRINT ICI, LIMITER CE QU'ON MET POUR EVITER LE FREEZE === //
        // =========================================================================== //

        if (Input.GetButton("ChangeMusicPlus")) {

            ChangeMusictoNext();
            
            // ===== CHANGEMENT DES TILES + BCKG ===== //
            GroundTry[] tabmagik = (GroundTry[])FindObjectsOfType(typeof(GroundTry)); // Recup' tout les items avec le script de changement
            foreach (GroundTry themetile in tabmagik) // Parcours
            {
                GroundTry script = (GroundTry)themetile.GetComponent(typeof(GroundTry)); // Recup' leur script
                script.ChangeTheme(playercurrentstyle); // Modif'
            }

            ApplyStyleCarac(playercurrentstyle);
        }

        // ===== PREVIOUS MUSIC ===== //
        if (Input.GetButton("ChangeMusicMinus"))
        {
            ChangeMusictoPrevious();
            // ===== CHANGEMENT DES TILES + BCKG ===== //
            GroundTry[] tabmagik = (GroundTry[])FindObjectsOfType(typeof(GroundTry)); // Recup' tout les items avec le script de changement
            foreach (GroundTry themetile in tabmagik) // Parcours
            {
                GroundTry script = (GroundTry)themetile.GetComponent(typeof(GroundTry)); // Recup' leur script
                script.ChangeTheme(playercurrentstyle);  // Modif'

            }
            ApplyStyleCarac(playercurrentstyle);
        }
       
        // =========================================================================== //
        // ============================== MOUVEMENT ================================== //
        // =========================================================================== //
        // ===== Left/right ===== //
        if (Input.GetButtonDown("Horizontal"))
        {
            if (!bInAir) anim.SetBool("isrunning", true);
            if (doubletapcooldown > 0 && tapcount == 1) // Double tap
            {
                // ADD RUN FUNCTION OR THING
                bRun = true;
                print("RUN!");
            }
            else
            {
                doubletapcooldown = 0.5f;
                tapcount = 1;
            }
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            bRun = false;
            anim.SetBool("isrunning", false);
        }

        // ===== Jump  ===== //
        if (Input.GetButton("Jump") && !bInAir && (Time.time > timelastjump+mintimejump))
        {
            bInAir = true;
            rigid.AddForce(new Vector3(0, 300, 0));
            timelastjump = Time.time;
        }
        // ================= //

        // ===== Down ===== //
        if (Input.GetButton("Down"))
        {
            if (bInAir)
            {
                rigid.gravityScale = 5f; // FAST FALL

            }
        }
        // =============== //


        // =========================================================================== //
        // =============================== HUD ======================================= //
        // =========================================================================== //
        maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, transform.position + decalCamOrigine, Time.deltaTime);
        playerpdv = playerpdvgameobject.GetComponent<Text>();
        playerpdv.text = "HP: " + hp + "/"+hpmax;

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
    }

    // =========================================================================================================
    void FixedUpdate()
    {
        if (!bRun)
        {
            rigid.AddForce((new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * 300f * Time.deltaTime));
        } else
        {
            rigid.AddForce( (new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * 600f * Time.deltaTime) );
        }

       // sprite.transform.position += new Vector3(Input.GetAxis("Horizontal") * slowFactor * vitesse * Time.deltaTime,0,0); // Position
        if (Input.GetAxis("Horizontal") > 0) sprite.flipX = false;
        if (Input.GetAxis("Horizontal") < 0) sprite.flipX = true;
    }


    // ======================================== //
    // ============= Collision ================ //
    // ======================================== //
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Sol")) {
            bInAir = false;
            anim.SetBool("isjump", false);
            rigid.gravityScale = initialgravity; // Disable Fast Fall
        }
    }
    
    // =========================================
    void OnCollisionStay2D(Collision2D collision) // Empêche de ne plus pouvoir jump si atterrissage alors que bouton Jump maintenu
    {
        if ((collision.gameObject.tag == "Sol")) { bInAir = false; anim.SetBool("isjump", false); }
        if (collision.gameObject.tag == "Damage" && (Time.time > lastDamage + recoveryTime))
        {
            hp--;
            bInAir = true;
            rigid.AddForce(new Vector3(0, 200, 0));
            lastDamage = Time.time;
        }
    }

    // =========================================
    void OnCollisionExit2D (Collision2D collision)
    {
        if ((collision.gameObject.tag == "Sol")) // A MODIFIER : Lorsque passe d'une tile sol à l'autre, on "est en l'air"
        {
            bInAir = true;
            anim.SetBool("isjump", true);      
        }
    }


    // ======================================== //
    // ============= FONCTIONS ================ //
    // ======================================== //

    public void ChangeMusictoNext()
    {
        if (Time.time > lastchange + changeTime)
        {
            lastchange = Time.time;
            if (playercurrentstyle == EnumList.StyleMusic.Calm)playercurrentstyle = EnumList.StyleMusic.Hell;
            else { playercurrentstyle++; }
        }

    }

    public void ChangeMusictoPrevious()
    {
        if (Time.time > lastchange + changeTime)
        {
            lastchange = Time.time;
            if (playercurrentstyle == EnumList.StyleMusic.Hell) playercurrentstyle = EnumList.StyleMusic.Calm;
            else {playercurrentstyle--; }
        }

    }

    // FONCTION QUI PERMET D'APPLIQUER DIFFÉRENTS EFFETS AU PERSONNAGE EN FONCTION DU SYLE DE MUSIQUE
    void ApplyStyleCarac (EnumList.StyleMusic newstyle)  
    {
        switch (newstyle)
        {
            case EnumList.StyleMusic.Hell:
                initialgravity = 1f; // -- Gravité (Hover/Not)      
                rigid.gravityScale = initialgravity;
                break;
            case EnumList.StyleMusic.Fest:
                initialgravity = 1f; // -- Gravité (Hover/Not)
                rigid.gravityScale = initialgravity;
                break;
            case EnumList.StyleMusic.Calm:
                initialgravity = 0.5f; // -- Gravité (Hover/Not)
                rigid.gravityScale = initialgravity;
                break;

        }
    }

}
