using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MusicSwitcher : MonoBehaviour{

    // ===== Les 3 sprites ===== //
    [SerializeField]
    Sprite HellTile;
    [SerializeField]
    Sprite FestTile;
    [SerializeField]
    Sprite CalmTile;
    SpriteRenderer spriterenderer;
    Image ImageSRC;
    [SerializeField]
    PhysicsMaterial2D PhysicHell;
    [SerializeField]
    PhysicsMaterial2D PhysicFest;
    [SerializeField]
    PhysicsMaterial2D PhysicCalm;

    PhysicsMaterial2D CurrentPhysic;
    BoxCollider2D boxcollider;

    // ===== Things to add to each style ===== //

    // ===== VAR ===== //
    public EnumList.StyleMusic currentstyle = EnumList.StyleMusic.Hell;
    float changeTime=1;
    float lastchange=0;


    // Use this for initialization
    void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();
        boxcollider = GetComponent<BoxCollider2D>();
        ImageSRC = GetComponent<Image>();
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
        switch (playerstyle)
        {
            case EnumList.StyleMusic.Hell:
                spriterenderer.sprite = HellTile; // Change le sprite
                if (PhysicHell != null) CurrentPhysic = PhysicHell; // Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                break;
            case EnumList.StyleMusic.Fest:
                spriterenderer.sprite = FestTile; // Change le sprite
                if (PhysicFest != null) CurrentPhysic = PhysicFest;// Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                break;
            case EnumList.StyleMusic.Calm:
                spriterenderer.sprite = CalmTile; // Change le sprite
                if (PhysicCalm != null) CurrentPhysic = PhysicCalm;// Si il y a une physique particulière à appliquer
                else CurrentPhysic = null; // Sinon la met à nulle
                break;

        }
        boxcollider.sharedMaterial = CurrentPhysic;
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
