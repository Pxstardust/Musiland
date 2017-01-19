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
    public int currentthemeid = 1;
    float changeTime=1;
    float lastchange=0;


    // Use this for initialization
    void Start () {
        spriterenderer = GetComponent<SpriteRenderer>();

    }


    // Update is called once per frame
    void Update () {
	
	}

    public void ChangetoNext()
    {
        if (Time.time > lastchange + changeTime)
        {
            lastchange = Time.time;
            currentthemeid++;
            if (currentthemeid > 3) currentthemeid = 1;
            ChangeTheme();
        }

    }

    public void ChangetoPrevious()
    {
        if (Time.time > lastchange+ changeTime)
        {
            lastchange = Time.time;
            currentthemeid--;
            if (currentthemeid < 1) currentthemeid = 3;
            ChangeTheme();
        }

    }

    void ChangeTheme()
    {
        switch (currentthemeid)
        {
            case 1:
                spriterenderer.sprite = HellTile;
                break;
            case 2:
                spriterenderer.sprite = FestTile;
                break;
            case 3:
                spriterenderer.sprite = CalmTile;
                break;

        }
    }
}
