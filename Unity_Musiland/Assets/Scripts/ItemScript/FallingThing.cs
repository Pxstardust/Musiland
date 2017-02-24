using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingThing : Entity {

    [SerializeField]
    Vector3 fallPoint;
    bool isfallen = false;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    protected override void Update()
    {


        base.Update();
    }


    public void Fall()
    {
       if (!isfallen)
        {
            Entity_GoTo(fallPoint, 140);
            isfallen = true;
        }

    }
}
