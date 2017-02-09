using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeball : Entity
{
    [SerializeField]
    Vector3 Position1;
    [SerializeField]
    Vector3 Position2;
    [SerializeField]
    float Rota1, Rota2;

    [SerializeField]
    float newfollowradius;
    [SerializeField]
    float newspeed;
    [SerializeField]
    float newrotaspeed;
    [SerializeField]
    GameObject target;

    // Use this for initialization
    void Start()
    {
        speed = newspeed;
        rotaspeed = newrotaspeed;
        //Entity_Follow(target);
        //Entity_Flee(target);
       // Entity_Stay(Position1);
        //followradius = newfollowradius;

    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();
 
        if (Input.anyKeyDown)
        {
            
            //Entity_Patrol(Position1, Position2);
            //Entity_Rotate(Rota1);
        }
    }



}
