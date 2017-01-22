using UnityEngine;
using System.Collections;
using System;

public class GroundTry : MonoBehaviour{

    // ===== Les 3 sprites ===== //
    [SerializeField]
    Sprite HellTile;
    [SerializeField]
    Sprite FestTile;
    [SerializeField]
    Sprite CalmTile;
    SpriteRenderer spriterenderer;

    // ===== Things to add to each style ===== //

    // ===== VAR ===== //
    public EnumList.StyleMusic currentstyle = EnumList.StyleMusic.Hell;
    float changeTime=1;
    float lastchange=0;


    // Use this for initialization
    void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();

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
                spriterenderer.sprite = HellTile;
                break;
            case EnumList.StyleMusic.Fest:
                spriterenderer.sprite = FestTile;
                break;
            case EnumList.StyleMusic.Calm:
                spriterenderer.sprite = CalmTile;
                break;

        }
    }
}
