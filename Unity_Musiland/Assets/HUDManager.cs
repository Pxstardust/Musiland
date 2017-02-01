using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

    [SerializeField]
    public GameObject cercletransition;
    [SerializeField]
    public GameObject particlesplash;
    [SerializeField]
    public Player Theplayer;

    // ===== CERCLE ===== //
    public float circletransitingtimer;
    public float circletransitingscale;
    public float transitime;
    float departtransi;
    float farleft, farright, starttransipoint, farest;

    // ===== Ornement ===== //
    MusicSwitcher OrnementScript;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (circletransitingtimer > 0)
        {
            circletransitingtimer -= Time.deltaTime;
            float initscale = cercletransition.transform.localScale.x;
            float Currentscale = Mathf.Lerp(cercletransition.transform.localScale.x, circletransitingscale, 1-(circletransitingtimer/9f));
            cercletransition.transform.localScale = new Vector3(Currentscale, Currentscale, 1);
        }

        if (transitime > 0)
        {
            TransitionChangement();
        }
        else
        {
            //Theplayer.istransiting = false;
            particlesplash.SetActive(false);
        }
    }

    // ===== Scale the circle ===== //
    public void ScaleCircleTransition(float scalefinale)
    {
        cercletransition.transform.localScale = new Vector3(0,0,0);
        circletransitingscale = scalefinale;
        circletransitingtimer = 9f;
       
    }


    // ===== Fonction qui peret d'initialiser la transition en analysant la map ===== //
    public void ChangeAllTiles()
    {
        starttransipoint = Theplayer.sprite.transform.position.x;
        farleft = 10; farright = -10; transitime = 0.5f;


        // ===== CHANGEMENT DES TILES + BCKG ===== //
        MusicSwitcher[] tabmagik = (MusicSwitcher[])FindObjectsOfType(typeof(MusicSwitcher)); // Recup' tout les items avec le script de changement
        foreach (MusicSwitcher themetile in tabmagik) // Parcours
        {
            if (themetile.gameObject.transform.position.x <= farleft) farleft = themetile.gameObject.transform.position.x; // Objet le plus à gauche 
            if (themetile.gameObject.transform.position.x >= farright) farright = themetile.gameObject.transform.position.x; // Objet le plus à droite
        }

        if (Mathf.Abs(starttransipoint - farleft) > Mathf.Abs(starttransipoint - farright)) farest = Mathf.Abs(starttransipoint - farleft);
        else farest = Mathf.Abs(starttransipoint - farright);
       
        transitime = 10f;
    }

    // ======================================================================================================== //
    // ===== Fonction qui permet d'avoir une transformation des tiles en forme de cercle depuis le joueur ===== //
    public void TransitionChangement()
    {
        particlesplash.SetActive(true);
        OrnementScript = (MusicSwitcher)particlesplash.GetComponent(typeof(MusicSwitcher));
        
        transitime -= Time.deltaTime;
        //print(farest);
        float cap = Mathf.Lerp(0, 200, 1 - (transitime/10));
        //print("cap" + cap + " // Transitime = "+transitime +" === "+ (1 - (transitime / 2) ));
        MusicSwitcher[] tabmagik = (MusicSwitcher[])FindObjectsOfType(typeof(MusicSwitcher)); // Recup' tout les items avec le script de changement
        //int tak = 0;
        foreach (MusicSwitcher themetile in tabmagik) // Parcours
        {
            MusicSwitcher script = (MusicSwitcher)themetile.GetComponent(typeof(MusicSwitcher)); // Recup' leur script

            if (
                (themetile.gameObject.transform.position.x > (Theplayer.sprite.transform.position.x - cap)) &&
                (themetile.gameObject.transform.position.x < (Theplayer.sprite.transform.position.x + cap)) &&
                (themetile.gameObject.transform.position.y > (Theplayer.sprite.transform.position.y - cap)) &&
                (themetile.gameObject.transform.position.y < (Theplayer.sprite.transform.position.y + cap))
                )
            {
                script.ChangeTheme(Theplayer.playercurrentstyle);
                OrnementScript.ChangeImageSrc(Theplayer.playercurrentstyle);
            }
        }

    }


}
