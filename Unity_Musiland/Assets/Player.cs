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
    public SpriteRenderer sprite;
    Animator anim;

    // ============================ //
    // ===== Collider Checker ===== //
	public Transform groundCheck;
    public Transform leftCheck;
    public Transform rightCheck;

    // ================== //
    // ===== Camera ===== //
    [SerializeField]
    Camera maincamera;
    Vector3 decalCamOrigine;
    Vector3 CurrentRespawnPoint;
    public Vector3 PlayerScreenPos;

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
    public bool istransiting = false;
    float transitime = 0;

    float maxSpeedCalm = 4; // Vitesse max Calm
    float maxSpeedFest = 5; // Vitesse max Fest
    float maxSpeedHell = 6; // Vitesse max Hell

    float gravityScaleCalm = 0.75f;
    float gravityScaleFest = 1f;
    float gravityScaleHell = 1f;

    float moveForceHell = 30f;


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
	bool canMove = true;
    // -- Hide -- //
	bool hideUnderSnow = false;
    // -- WalLSlide -- //
    float WallSlideSpeed = -2;
    // -- H Dash -- //
    float doubletapCDDash = 0.5f;
    float HorizontalTapCount = 0;
    float DashCD = 2;
    float LastDashEnd, LastDashStart;
    float DureeDash = 0.25f;
    // -- V Dash -- //
    float doubletapCDVDash = 0.5f;
    float VerticalTapCount = 0;
    float LastVDashStart, LastVDashEnd;
    bool IsVDashDone = false;
    bool bVDash = false;
    float DureeVDash = 1f;

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
    public GameObject HUDManagerGO;
    public HUDManager HUDManager;

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
        HUDManager = (HUDManager)HUDManagerGO.GetComponent(typeof(HUDManager));
    }

    // ========================================================================================================= //
    // =========================================== UPDATE ====================================================== //
    // ========================================================================================================= //
    void Update () {
        PlayerScreenPos = maincamera.WorldToScreenPoint(this.transform.position);
        if (doubletapCDDash > 0) { doubletapCDDash -= Time.deltaTime; }
        if (doubletapCDVDash > 0) { doubletapCDVDash -= Time.deltaTime; }
        if (bRun && Time.time > LastDashStart + DureeDash) { bRun = false;  rigid.velocity = new Vector2(0, 0); } // Si on est en dash horizontal dpeuis un certain temps : stop
        if (bVDash && Time.time > LastVDashStart + DureeVDash) { bVDash = false; } // Si on est en V Dash depuis un certain temps :stop
        if (bRun)
        {
            sprite.color = new Color32(0,255,0, 255) ;
        } else sprite.color = Color.white;

        if (bVDash)
        {
            sprite.color = new Color32(0,255, 0,255);
        }

        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground")))
        {
            bVDash = false;
        }


        // == WALL JUMP == //
            bool isWallSliding = false;

        if (playercurrentstyle == EnumList.StyleMusic.Fest)
        {
            // ----- Si il y a un mur à gauche/droite et rien en dessous et qu'on est pas en train de monter ---- 
            if ((Physics2D.Linecast(transform.position, leftCheck.position, 1 << LayerMask.NameToLayer("ground")) ||
            Physics2D.Linecast(transform.position, rightCheck.position, 1 << LayerMask.NameToLayer("ground"))) &&
            !Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground")) && rigid.velocity.y < 0
            ) 
            {
                isWallSliding = true;
                rigid.velocity = new Vector2 (0, WallSlideSpeed);
            }
        }


        // == WALL JUMP == //
        
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
            ChangeMusictoNext();
            HUDManager.ChangeAllTiles();
            ApplyStyleCarac(playercurrentstyle);
        }

        // ===== PREVIOUS MUSIC ===== //
        if (Input.GetButton("ChangeMusicMinus"))
        {

            ChangeMusictoPrevious();
            HUDManager.ChangeAllTiles();
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
            if (doubletapCDDash > 0 && HorizontalTapCount == 1) // Double tap
            {
                if (Time.time > LastDashEnd + DashCD)
                {
                    bRun = true;
                    LastDashStart = Time.time;
                }
            }
            else // Premier coup pour le double tap :
            {
                doubletapCDDash = 0.5f;
                HorizontalTapCount = 1;
            }
        }

       

        if (Input.GetButtonUp("Horizontal"))
        {
            if (bRun)
            {
                bRun = false;
                LastDashEnd = Time.time;
            }
            anim.SetBool("isrunning", false);
        }

        // ===== Jump  ===== //
		if ( Input.GetButton("Jump") && !bInAir && Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground")) && (Time.time > timelastjump+mintimejump))
        {
            //bInAir = true; === Problème with colliders
            IsVDashDone = false;
			StartCoroutine(setJump());
            rigid.AddForce((new Vector3(0.0f,300,0)));
            timelastjump = Time.time;
        }

        if (Input.GetButton("Jump") && isWallSliding)
        {
            bool wallatleft = false;
            if (Physics2D.Linecast(transform.position, leftCheck.position, 1 << LayerMask.NameToLayer("ground"))) wallatleft = true;
            if (wallatleft)
            {
                rigid.AddForce((new Vector3(300, 300, 0)));

            } else
            {
                rigid.AddForce((new Vector3(-300, 300, 0)));

            }
        }
        // ================= //

		// ===== Up ===== //
		if (Input.GetButton ("Up")) {
			if (bInAir && playercurrentstyle == EnumList.StyleMusic.Calm && rigid.velocity.y < 0) { // Si on est en l'air
				rigid.gravityScale = 0.1f;
				rigid.AddForce ((new Vector3(0.0f,0.6f,0)));
			} else { // Si on est au sol

			}
		} else {
			if (bInAir && playercurrentstyle == EnumList.StyleMusic.Calm) { // Si on est en l'air
				rigid.gravityScale = gravityScaleCalm;
			} 
		}
		// =============== //


        // ===== Down ===== //
        if (Input.GetButton("Down"))
        {
            if (bInAir) // Si on est en l'air
            {
                rigid.gravityScale = 5f; // FAST FALL
            }

        }

		if (Input.GetButtonDown ("Down")) {
			if (playercurrentstyle == EnumList.StyleMusic.Calm && !bInAir) 
			{
				HideUnderSnow();
			}
        }

		if (Input.GetButtonUp ("Down")) {
			if (hideUnderSnow == true) 
			{
				UnhideUnderSnow();
			}
		}
        // =============== //


        // =========================================================================== //
        // =============================== HUD ======================================= //
        // =========================================================================== //
        maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, transform.position + decalCamOrigine, Time.deltaTime);
        backgroundsplash.transform.position = new Vector3(maincamera.transform.position.x, maincamera.transform.position.y, 0);
        //particlesplash.transform.position = new Vector3(maincamera.transform.position.x, maincamera.transform.position.y, 0);
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
                // =============================================================================================================
                case EnumList.StyleMusic.Hell:
				if (!bRun && !bInAir) // S'il n'as pas double tap et qu'il n'est pas en l'air
                {
					rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeedHell, rigid.velocity.y); // Déplacement direct
                } 
				else if(!bRun && bInAir && Mathf.Abs(rigid.velocity.x) < maxSpeedHell){
					rigid.AddForce ((new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * maxSpeedHell * 100 * Time.deltaTime));
				}
				if (bRun) // DASH
                    {
                        rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * moveForceHell, rigid.velocity.y);
                        
                        // ========================================== 
                        // Mécanique de dash sur une distance prévue
                        // ==========================================
                    }

                    break;

                // =============================================================================================================
                case EnumList.StyleMusic.Fest:
					if (!bInAir) // S'il n'as pas double tap et qu'il n'est pas en l'air
					{
						rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeedFest, rigid.velocity.y); // Déplacement direct
					} 
					else if(bInAir && Mathf.Abs(rigid.velocity.x) < maxSpeedFest){
						rigid.AddForce ((new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f) * maxSpeedFest * 100 * Time.deltaTime));
					}
                    break;

                // =============================================================================================================
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


        if (Input.GetButtonDown("Down"))
        {
            // ----- DOUBLE TAP ----- //
            if (doubletapCDVDash > 0 && VerticalTapCount == 1) // Double tap
            {
                if (!IsVDashDone && bInAir && playercurrentstyle == EnumList.StyleMusic.Hell) // Si on a pas encore fait de dash aérien
                {
                    bVDash = true; // On passe en mode dash vertical
                    rigid.velocity = new Vector2(0, Input.GetAxis("Vertical") * 150); // DASH VERTICAL
                    print("VDash");
                    LastVDashStart = Time.time; // Note le début du dash vertical
                }
            }
            else // Premier coup pour le double tap :
            {
                doubletapCDVDash = 0.5f;
                VerticalTapCount = 1;
            }
        }
    }


    // ========================================================================================================= //
    // ================================== Collisions & TRIGGER ================================================= //
    // ========================================================================================================= //
    // ======================================== //
	// ============= Collision ================ //
	// ======================================== //
	void OnCollisionEnter2D(Collision2D collision)
	{

        bVDash = false;
		if ((collision.gameObject.tag == "Sol") && Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"))) {
			bInAir = false;
			anim.SetBool("isjump", false);
			rigid.gravityScale = initialgravity; // Disable Fast Fall

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
            
            HUDManager.ScaleCircleTransition(35, PlayerScreenPos);
        }
    }

    // ===================================== //
    // ===== Changement de musique (-) ===== //
    public void ChangeMusictoPrevious()
    {
        if ((Time.time > lastchange + changeTime) && (!istransiting))
        {
            HUDManager.ScaleCircleTransition(35, PlayerScreenPos);
            lastchange = Time.time;
            if (playercurrentstyle == EnumList.StyleMusic.Hell) playercurrentstyle = EnumList.StyleMusic.Calm;
            else {playercurrentstyle--; }
            
        }

    }
    
	// ================================== //
	// ===== Se cache sous la neige ===== //
	public void HideUnderSnow()
	{
		rigid.velocity = new Vector2(0,0);
		hideUnderSnow = true;
		canMove = false;
		GetComponent<Collider2D> ().enabled = false;
		rigid.gravityScale = 0;
	}

	// ================================== //
	// ===== Sort de sous la neige ===== //
	public void UnhideUnderSnow()
	{
		hideUnderSnow = false;				
		canMove = true;
		GetComponent<Collider2D> ().enabled = true;
		rigid.gravityScale = gravityScaleCalm;
	}

	// =========================================================================== //
	// ===== Fonction qui applique différents effets au perso selon le style ===== //
	void ApplyStyleCarac (EnumList.StyleMusic newstyle)  
	{
		switch (newstyle)
		{
		case EnumList.StyleMusic.Hell:
			initialgravity = gravityScaleHell; // -- Gravité (Hover/Not)      
			rigid.gravityScale = initialgravity;
			break;
		case EnumList.StyleMusic.Fest:
			initialgravity = gravityScaleFest; // -- Gravité (Hover/Not)
			rigid.gravityScale = initialgravity;
			break;
		case EnumList.StyleMusic.Calm:
			initialgravity = gravityScaleCalm; // -- Gravité (Hover/Not)
			rigid.gravityScale = initialgravity;
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

    // ================ //
    // ===== DASH ===== //
    public void FDash()
    {

    }

}
