using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

    [SerializeField]
    float FadeSpeed;
    float finalsize;
    float initsize;
    bool isok = false;
    public bool done;
    SpriteRenderer sprite;

    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        float fadestep = FadeSpeed * Time.deltaTime;
        if (isok)
        {
            if (sprite.color.a != finalsize)
            {
                    
                float a = Mathf.Lerp(sprite.color.a, finalsize, fadestep);
               
                sprite.color = new Color(1f,0,0, a);
            }
            else isok = false;
        }

        if (sprite.color.a < initsize/5) { done = true; }
    }


    public void Fadeto(float a)
    {
        finalsize = a;
        isok = true;
    }

    public void Resetsize()
    {
        finalsize = initsize;
        isok = true;

    }
}
