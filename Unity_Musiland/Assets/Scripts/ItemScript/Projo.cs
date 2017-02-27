using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projo : Entity {
    bool onortwo = true;
    [SerializeField]
    float ang1, ang2;


	// Use this for initialization
	void Start () {
        Entity_RotateBetween(ang1, ang2 );
		
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();


        if (!isrotating)
        {
            /*
            if (onortwo)
            {
                Entity_Rotate(15);
            } else
            {
                Entity_Rotate(130);
            } */
        }
		
	}
}
