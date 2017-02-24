using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameResize : MonoBehaviour {

    [SerializeField]
    float resizeSpeed;
    Vector3 finalsize;

    bool isok=false;

	// Use this for initialization
	void Start () {
		
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
        }


	}


    public void Resize(Vector3 vec)
    {
        finalsize = vec;
        isok = true;
    }
}
