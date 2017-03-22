using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameResize : MonoBehaviour {

    [SerializeField]
    float resizeSpeed;
    Vector3 finalsize;
    Vector3 initsize;
    bool isok=false;
    public bool done;


 

	// Use this for initialization
	void Start () {
        initsize = this.transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
       float resizestep = resizeSpeed * Time.deltaTime;
        if (isok)
        {
            if (transform.localScale != finalsize)
            {
                Vector3 scalevec = Vector3.Lerp(transform.localScale, finalsize, resizestep);
                transform.localScale = scalevec;
            }
            else isok = false;
        }
           
       
        if (transform.localScale.x < (initsize.x*0.05)) { done = true;  }
	}


    public void Resize(Vector3 vec)
    {
        finalsize = vec;
        isok = true;
    }

    public void Resetsize()
    {
        finalsize = initsize;
        isok = true;
           
    }
}
