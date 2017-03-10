using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MusicSwitcher : MonoBehaviour{

    SpriteRenderer spriterenderer;
    Image ImageSRC;
    Animator anim;

    [SerializeField]
    bool IsPlayer;

    [SerializeField]
    bool DisableFestCollider;
    [SerializeField]
    bool DisableCalmCollider;
    [SerializeField]
    bool DisableHellCollider;

     // ===== Les 3 sprites ===== //
     [SerializeField]
    public Sprite HellTile;
    [SerializeField]
    public Sprite FestTile;
    [SerializeField]
    public Sprite CalmTile;
    
    // ===== Les 3 Physiques ===== //
    [SerializeField]
    PhysicsMaterial2D PhysicHell;
    [SerializeField]
    PhysicsMaterial2D PhysicFest;
    [SerializeField]
    PhysicsMaterial2D PhysicCalm;

    //====== Les 3 Anim ===== //
    [SerializeField]
    RuntimeAnimatorController Hell_Anim;
    [SerializeField]
    RuntimeAnimatorController Fest_Anim;
    [SerializeField]
    RuntimeAnimatorController Calm_Anim;
    

    PhysicsMaterial2D CurrentPhysic;
    [SerializeField]
	Collider2D Maincollider;

    [SerializeField]
    bool NONJEVEUXPASDECOLLIDER;

    public bool isdone;

    // ===== Things to add to each style ===== //

    // ===== VAR ===== //
    public EnumList.StyleMusic currentstyle = EnumList.StyleMusic.Hell;
    float changeTime=1;
    float lastchange=0;


    // Use this for initialization
    void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();
        //collider = GetComponent<BoxCollider2D>();
        ImageSRC = GetComponent<Image>();
        anim = GetComponent<Animator>();
		if (!Maincollider && !IsPlayer && !NONJEVEUXPASDECOLLIDER) {
			Maincollider = GetComponent<Collider2D> ();
		}
    }


    // Update is called once per frame
    void Update () { }

    // =========================================================================== //
    // =========================================================================== //
    // === NOTE : PAS DE PRINT ICI, LIMITER CE QU'ON MET POUR EVITER LE FREEZE === //
    // =========================================================================== //
    // =========================================================================== //

    public void ChangeTheme(EnumList.StyleMusic playerstyle)
    {
        currentstyle = playerstyle;
        spriterenderer.enabled = true;
        if (Maincollider) Maincollider.enabled = true;

        switch (playerstyle)
        {
            // ---------- HELL ---------- //
            case EnumList.StyleMusic.Hell:
                if (anim && Hell_Anim) // Si il y a une animation
                {
                    anim.enabled = true;
                    anim.runtimeAnimatorController = Hell_Anim as RuntimeAnimatorController;
                } else if (HellTile) // Sinon si ya un sprite
                {
                    if (anim) anim.enabled = false;
                    spriterenderer.sprite = HellTile; // Change le sprite
                }
                else // Sinon ya rien
                {
                    spriterenderer.enabled = false;
                    if (Maincollider) Maincollider.enabled = false;
                }

                if (PhysicHell != null) CurrentPhysic = PhysicHell; // Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                if (DisableHellCollider) Maincollider.enabled = false;
                break;
            // -------------------------- //

            // ---------- FEST ---------- //
            case EnumList.StyleMusic.Fest:
                if (anim && Fest_Anim) // Si il y a une animation
                {
                   
                    anim.enabled = true;
                    anim.runtimeAnimatorController = Fest_Anim as RuntimeAnimatorController;

                }
                else if (FestTile) // Sinon si ya un sprite
                {
                    if (anim) anim.enabled = false;
                    spriterenderer.sprite = FestTile; // Change le sprite
                }
                else // Sinon ya rien
                {
                    spriterenderer.enabled = false;
                    if (Maincollider) Maincollider.enabled = false;
                }

                if (PhysicFest != null) CurrentPhysic = PhysicFest;// Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                if (DisableFestCollider) Maincollider.enabled = false;
                break;
            // -------------------------- //

            // ---------- CALM ---------- //
            case EnumList.StyleMusic.Calm:
                if (anim && Calm_Anim) // Si il y a une animation
                {

                    anim.enabled = true;
                    anim.runtimeAnimatorController = Calm_Anim as RuntimeAnimatorController;

                }
                else if (CalmTile) // Sinon si ya un sprite
                {
                    if (anim) anim.enabled = false;
                    spriterenderer.sprite = CalmTile; // Change le sprite
                }
                else // Sinon ya rien
                {
                    spriterenderer.enabled = false;
                    if (Maincollider) Maincollider.enabled = false;
                }

                if (PhysicCalm != null) CurrentPhysic = PhysicCalm;// Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                if (DisableCalmCollider) Maincollider.enabled = false;
                break;

        }
		if (Maincollider) Maincollider.sharedMaterial = CurrentPhysic;
    }

    public void ChangeImageSrc(EnumList.StyleMusic playerstyle)
    {
        switch (playerstyle)
        {
            case EnumList.StyleMusic.Hell:
                ImageSRC.sprite = HellTile;
                if (PhysicHell != null) CurrentPhysic = PhysicHell; // Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                break;
            case EnumList.StyleMusic.Fest:
                ImageSRC.sprite = FestTile; // Change le sprite
                if (PhysicFest != null) CurrentPhysic = PhysicFest;// Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                break;
            case EnumList.StyleMusic.Calm:
               ImageSRC.sprite = CalmTile; // Change le sprite
                if (PhysicCalm != null) CurrentPhysic = PhysicCalm;// Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                break;

        }
    }
}
