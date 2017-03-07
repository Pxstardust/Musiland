using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredArea : MonoBehaviour {
    [SerializeField]
    public GameObject[] Triggerer;

    public bool IAMTRIGGERED=false;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       
        foreach (GameObject triggerer in Triggerer)
        {
            //print(collision.gameObject + "__" + triggerer);
            if (collision.gameObject == triggerer)
            {
                //print("found");
                IAMTRIGGERED = true;
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        foreach (GameObject triggerer in Triggerer)
        {
            if (collision.gameObject == triggerer)
            {
                IAMTRIGGERED = false;

            }
        }

    }




}
