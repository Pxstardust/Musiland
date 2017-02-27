using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingThing : Entity {

    [SerializeField]
    float fallHeight;
    bool isfallen = false;
    Vector3 fallPoint;

    // Use this for initialization
    void Start () {
        fallPoint = new Vector3(this.transform.position.x, fallHeight, this.transform.position.z);
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
