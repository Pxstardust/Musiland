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
    Vector3 CurrentRespawnPoint;

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
    bool bIsGrabbingWall = false;

    // =============== //
    // ===== HUD ===== //
    public Text playerpdv;
    [SerializeField]
    public GameObject playerpdvgameobject;
    public Text victoiretext;
    [SerializeField]
    public GameObject victoiretextgameobject;

    // ========================================================================================================= //
    // ============================================= START ===================================================== //
    // ========================================================================================================= //
    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hp = 5;
        decalCamOrigine = maincamera.transform.position - transform.position;
        anim = GetComponent<Animator>();
        anim.enabled = true;
        CurrentRespawnPoint = sprite.transform.position;
    }

    // ========================================================================================================= //
    // =========================================== UPDATE ====================================================== //
    // ========================================================================================================= //
    void Update () {
        if (doubletapcooldown > 0) { doubletapcooldown -= Time.deltaTime; }
        
        // == DEBUG == //
        if (Input.GetButton("DebugKey"))
        {
            PlayerRespawn();
        }
        // == DEBUG == //
        
        // ================================================================================ //
        // ============================== I. CONTROLS ===================================== //
        // ================================================================================ //

        // =========================================================================== //
        // ============================= I.1 MUSIC =================================== //
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
        // ============================ I.2 MOUVEMENT ================================ //
        // =========================================================================== //
        // ===== Left/right ===== //
        if (Input.GetButtonDown("Horizontal"))
        {
            //if (!bInAir) anim.SetBool("isrunning", true);
            // ========== RUN ========== //
            if (doubletapcooldown > 0 && tapcount == 1) // Double tap
            {
                bRun = true;
                print("RUN!");
            }
            else // Premier coup pour le double tap :
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
        if ( (Input.GetButton("Jump") && !bInAir && (Time.time > timelastjump+mintimejump)))
        {
            bInAir = true;
            rigid.AddForce((new Vector3(0.0f,300,0)));
            timelastjump = Time.time;
        }
        // ================= //

        // ===== Down ===== //
        if (Input.GetButton("Down"))
        {
            if (bInAir) // Si on est en l'air
            {
                rigid.gravityScale = 5f; // FAST FALL

            } else // Si on est au sol
            {
                if (playercurrentstyle == EnumList.StyleMusic.Calm) 
                {
                    HideUnderSnow();
                }
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
    } // ================================== FIN UPDATE ========================================================= //

    // ========================================================================================================= //
    // ===================================== FIXED UPDATE ====================================================== //
    // ========================================================================================================= //
    void FixedUpdate()
    {
        
        if (Input.GetButton("Horizontal")) // Si le joueur se déplace latéralement : F() de déplacement différente selon theme en cours
        {

            switch (playercurrentstyle)
            {
                case EnumList.StyleMusic.Hell:
                    if (!bRun) // S'il n'as pas double tap,
                    {
                        rigid.velocity = new Vector2((Input.GetAxis("Horizontal") * 6), rigid.velocity.y); // Déplacement direct
                    } else // S'il a double tap
                    {
                        print("Dash!");
                        // ========================================== 
                        // Mécanique de dash sur une distance prévue/qui s'incrémente si le joueur laisse appuyer
                        // ==========================================
                    }

                    break;

                case EnumList.StyleMusic.Fest:
                    rigid.velocity = new Vector2((Input.GetAxis("Horizontal") * 6), rigid.velocity.y); // Déplacement direct
                    break;

                case EnumList.StyleMusic.Calm:
                    rigid.AddForce((new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * 300f * Time.deltaTime)); // Déplacement avec force, lent et flottant
                    break;
            }
        }

       // sprite.transform.position += new Vector3(Input.GetAxis("Horizontal") * slowFactor * vitesse * Time.deltaTime,0,0); // Position
        if (Input.GetAxis("Horizontal") > 0) sprite.flipX = false;
        if (Input.GetAxis("Horizontal") < 0) sprite.flipX = true;
    }


    // ========================================================================================================= //
    // ================================== Collisions & TRIGGER ================================================= //
    // ========================================================================================================= //
    // ======================================== //
    // ============= Collision ================ //
    // ======================================== //
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Sol")) {
            bInAir = false;
            anim.SetBool("isjump", false);
            rigid.gravityScale = initialgravity; // Disable Fast Fall
            
            if(playercurrentstyle == EnumList.StyleMusic.Fest)
            {
                bIsGrabbingWall = true;
            }
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
    }

    // ======================================== //
    // ============== Trigger= ================ //
    // ======================================== //
    void OnTriggerEnter2D(Collision2D collision)
    {
    }

    // =======================================
    void OnTriggerStay2D(Collision2D collision)
    {
    }

    // =======================================
    void OnTriggerExit2D(Collision2D collision)
    {
    }


    // ========================================================================================================= //
    // ======================================== Fonctions ====================================================== //
    // ========================================================================================================= //

    // ===================================== //
    // ===== Changement de musique (+) ===== //
    public void ChangeMusictoNext()
    {
        if (Time.time > lastchange + changeTime)
        {
            lastchange = Time.time;
            if (playercurrentstyle == EnumList.StyleMusic.Calm)playercurrentstyle = EnumList.StyleMusic.Hell;
            else { playercurrentstyle++; }
        }
    }

    // ===================================== //
    // ===== Changement de musique (-) ===== //
    public void ChangeMusictoPrevious()
    {
        if (Time.time > lastchange + changeTime)
        {
            lastchange = Time.time;
            if (playercurrentstyle == EnumList.StyleMusic.Hell) playercurrentstyle = EnumList.StyleMusic.Calm;
            else {playercurrentstyle--; }
        }
    }
    
    // ================================== //
    // ===== Se cache sous la neige ===== //
    public void HideUnderSnow()
    {
        print("Je suis caché sous la neige");
    }

    // =========================================================================== //
    // ===== Fonction qui applique différents effets au perso selon le style ===== //
    void ApplyStyleCarac (EnumList.StyleMusic newstyle)  
    {
        switch (newstyle)
        {
            case EnumList.StyleMusic.Hell:
                initialgravity = 1f; // -- Gravité (Hover/Not)      
                rigid.gravityScale = initialgravity;
                bIsGrabbingWall = false;
                break;
            case EnumList.StyleMusic.Fest:
                initialgravity = 1f; // -- Gravité (Hover/Not)
                rigid.gravityScale = initialgravity;
                break;
            case EnumList.StyleMusic.Calm:
                initialgravity = 0.5f; // -- Gravité (Hover/Not)
                rigid.gravityScale = initialgravity;
                bIsGrabbingWall = false;
                break;

        }
    }

    // ==================================================== //
    // ===== Fonction qui permet au joueur de respawn ===== //
    public void PlayerRespawn()
    {
        // ==== Ajouter mise en scène : son, anim... ===== //
        rigid.velocity = new Vector2(0, 0); // Annule toutes les forces en jeu
        transform.position = CurrentRespawnPoint;
    }

    // =========================================================================== //
    // ===== Fonction qui pemret de téléporter le joueur à un endroit précis ===== //
    public void PlayerTeleport(Vector3 destination, bool keepforce)
    {
        if (keepforce) rigid.velocity = new Vector2(0, 0);
        transform.position = destination;
    }

}
