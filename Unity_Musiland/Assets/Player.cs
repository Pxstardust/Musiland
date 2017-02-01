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
	public Transform groundCheck;

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
    bool istransiting = false;
    float transitime = 0;
    float departtransi;
    float farleft, farright, starttransipoint, farest;

    // === Keys === //
    private bool KeyShoot;
    private bool KeyDown;

    // === Controller Var === //
    bool bInAir;
    bool bRun;
    float lastchange = 0; // Date du dernier changement
    float initialgravity = 1; // Gravity Scale 
    float deathheight = -10; // Hauteur avant de mourir
    float mintimejump = 0.5f; // Durée entre deux sauts
    float timelastjump; // Date du dernier saut
    float doubletapcooldown = 0.5f;
    float tapcount = 0;
    bool bIsGrabbingWall = false;
	bool canMove = true;

	float maxSpeedCalm = 4;
	float maxSpeedFest = 5;
	float maxSpeedHell = 6;

	float moveForceHell = 30f;

    // =============== //
    // ===== HUD ===== //
    public Text playerpdv;
    [SerializeField]
    public GameObject playerpdvgameobject;
    public Text victoiretext;
    [SerializeField]
    public GameObject victoiretextgameobject;
    [SerializeField]
    public GameObject backgroundsplash;
    [SerializeField]
    public GameObject particlesplash;

    // ========================================================================================================= //
    // ============================================= START ===================================================== //
    // ========================================================================================================= //
    void Start () {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hp = 5;
        decalCamOrigine = maincamera.transform.position - transform.position;
        anim = GetComponent<Animator>();
       // anim.enabled = true;
        CurrentRespawnPoint = sprite.transform.position;
    }

    // ========================================================================================================= //
    // =========================================== UPDATE ====================================================== //
    // ========================================================================================================= //
    void Update () {
        if (doubletapcooldown > 0) { doubletapcooldown -= Time.deltaTime; }

        if (transitime > 0)
        {
            TransitionChangement();
        } else
        {
            istransiting = false;
            particlesplash.SetActive(false);
        }
        
        // == DEBUG == //
        if (Input.GetButton("DebugKey"))
        {
           
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

            starttransipoint = sprite.transform.position.x;
            farleft = 10; farright = -10; transitime = 0.5f;
            ChangeMusictoNext();
            
            // ===== CHANGEMENT DES TILES + BCKG ===== //
            MusicSwitcher[] tabmagik = (MusicSwitcher[])FindObjectsOfType(typeof(MusicSwitcher)); // Recup' tout les items avec le script de changement
            foreach (MusicSwitcher themetile in tabmagik) // Parcours
            {
                if (themetile.gameObject.transform.position.x <= farleft) farleft = themetile.gameObject.transform.position.x; // Objet le plus à gauche 
                if (themetile.gameObject.transform.position.x >= farright) farright = themetile.gameObject.transform.position.x; // Objet le plus à droite
            }

            if (Mathf.Abs(starttransipoint - farleft) > Mathf.Abs(starttransipoint - farright)) farest = farleft;
            else farest = farright;
            transitime = 2f;

            ApplyStyleCarac(playercurrentstyle);
        }

        // ===== PREVIOUS MUSIC ===== //
        if (Input.GetButton("ChangeMusicMinus"))
        {
            starttransipoint = sprite.transform.position.x;
            farleft = 10;farright = -10; transitime = 0.5f;
            ChangeMusictoPrevious();

            // ===== CHANGEMENT DES TILES + BCKG ===== //
            MusicSwitcher[] tabmagik = (MusicSwitcher[])FindObjectsOfType(typeof(MusicSwitcher)); // Recup' tout les items avec le script de changement
            foreach (MusicSwitcher themetile in tabmagik) // Parcours
            {                
                if (themetile.gameObject.transform.position.x <= farleft) farleft = themetile.gameObject.transform.position.x; // Objet le plus à gauche 
                if (themetile.gameObject.transform.position.x >= farright) farright = themetile.gameObject.transform.position.x; // Objet le plus à droite
            }

            if (Mathf.Abs(starttransipoint - farleft) > Mathf.Abs(starttransipoint - farright)) farest = farleft;
            else farest = farright;
            transitime = 2f;
            
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
        //&& (Time.time > timelastjump+mintimejump)  === that was in the following condition
		if ( Input.GetButton("Jump") && !bInAir && Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground")) && (Time.time > timelastjump+mintimejump))
        {
            //bInAir = true; === Problème with colliders
			StartCoroutine(setJump());
            rigid.AddForce((new Vector3(0.0f,300,0)));
            timelastjump = Time.time;
        }
        // ================= //

		// ===== Up ===== //
		if (Input.GetButton ("Up")) {
			if (bInAir && playercurrentstyle == EnumList.StyleMusic.Calm && rigid.velocity.y < 0) { // Si on est en l'air
				rigid.gravityScale = 0.1f;
				rigid.AddForce ((new Vector3(0.0f,0.7f,0)));
			} else { // Si on est au sol
				
			}
		} else {
			if (bInAir && playercurrentstyle == EnumList.StyleMusic.Calm) { // Si on est en l'air
				rigid.gravityScale = 0.75f;
			} 
		}
		// =============== //


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
        backgroundsplash.transform.position = new Vector3(maincamera.transform.position.x, maincamera.transform.position.y, 0);
        particlesplash.transform.position = new Vector3(maincamera.transform.position.x, maincamera.transform.position.y, 0);
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
		float h = Input.GetAxis ("Horizontal");  


        if (Input.GetButton("Horizontal") && canMove) // Si le joueur se déplace latéralement : F() de déplacement différente selon theme en cours
        {

            switch (playercurrentstyle)
            {
                case EnumList.StyleMusic.Hell:
				if (!bRun && !bInAir) // S'il n'as pas double tap et qu'il n'est pas en l'air
                {
					rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeedHell, rigid.velocity.y); // Déplacement direct
                } 
				else if(!bRun && bInAir && Mathf.Abs(rigid.velocity.x) < maxSpeedHell){
					rigid.AddForce ((new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * maxSpeedHell * 100 * Time.deltaTime));
				}
				else // S'il a double tap
                    {
                        print("Dash!");
						rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeedHell, rigid.velocity.y);
                        // ========================================== 
                        // Mécanique de dash sur une distance prévue/qui s'incrémente si le joueur laisse appuyer
                        // ==========================================
                    }

                    break;

                case EnumList.StyleMusic.Fest:
					if (!bInAir) // S'il n'as pas double tap et qu'il n'est pas en l'air
					{
						rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeedFest, rigid.velocity.y); // Déplacement direct
					} 
					else if(bInAir && Mathf.Abs(rigid.velocity.x) < maxSpeedFest){
						rigid.AddForce ((new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * maxSpeedFest * 100 * Time.deltaTime));
					}
                    break;

                case EnumList.StyleMusic.Calm:
					if (!bInAir && Mathf.Abs(rigid.velocity.x) < maxSpeedCalm) // S'il n'as pas double tap et qu'il n'est pas en l'air
					{
						rigid.AddForce((new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * maxSpeedCalm * 100 * Time.deltaTime)); // Déplacement avec force, lent et flottant
					}
					else if(bInAir && Mathf.Abs(rigid.velocity.x) < maxSpeedCalm){
						rigid.AddForce ((new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * maxSpeedCalm * 100 * Time.deltaTime));
					}
					break;
            }
        }

		//On frene le personange si on navance plus sauf pour le mode calme
		if (!Input.GetButton ("Horizontal") && !bInAir && playercurrentstyle != EnumList.StyleMusic.Calm) {
			rigid.velocity = new Vector2 (0, rigid.velocity.y);
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
		//=== Cela provoque un bug avec le jump (collision stay better than condition)
        //if ((collision.gameObject.tag == "Sol")) { bInAir = false; anim.SetBool("isjump", false); }
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
    void OnTriggerEnter2D(Collider2D collision)
    {
    }

    // =======================================
    void OnTriggerStay2D(Collider2D collision)
    {
    }

    // =======================================
    void OnTriggerExit2D(Collider2D collision)
    {
    }


    // ========================================================================================================= //
    // ======================================== Fonctions ====================================================== //
    // ========================================================================================================= //

    // ===================================== //
    // ===== Changement de musique (+) ===== //
    public void ChangeMusictoNext()
    {
        if ( (Time.time > lastchange + changeTime) && (transitime >= 0))
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
        if ((Time.time > lastchange + changeTime) && (!istransiting))
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
                initialgravity = 0.75f; // -- Gravité (Hover/Not)
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

    // =============== //
    // ===== ??? ===== //
	IEnumerator setJump(){
		yield return new WaitForSeconds (0.1f);
		bInAir = true;
	}

    // ======================================================================================================== //
    // ===== Fonction qui permet d'avoir une transformation des tiles en forme de cercle depuis le joueur ===== //
    public void TransitionChangement()
    {
        particlesplash.SetActive(true);
       transitime -= Time.deltaTime;

        float cap = Mathf.Lerp(0, (farright - farleft), 1 - (transitime));
       // print("cap" + cap);
        MusicSwitcher[] tabmagik = (MusicSwitcher[])FindObjectsOfType(typeof(MusicSwitcher)); // Recup' tout les items avec le script de changement
        foreach (MusicSwitcher themetile in tabmagik) // Parcours
        {
            MusicSwitcher script = (MusicSwitcher)themetile.GetComponent(typeof(MusicSwitcher)); // Recup' leur script

            if (
                (themetile.gameObject.transform.position.x > (sprite.transform.position.x - cap)) &&
                (themetile.gameObject.transform.position.x < (sprite.transform.position.x + cap)) &&
                (themetile.gameObject.transform.position.y > (sprite.transform.position.y - cap)) &&
                (themetile.gameObject.transform.position.y < (sprite.transform.position.y + cap))
                )
            {
                script.ChangeTheme(playercurrentstyle);
            }
        }
    }

}
