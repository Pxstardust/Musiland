using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEnabled : MonoBehaviour {

    [SerializeField]
    GameObject[] AutoEnabledGO;

    // Use this for initialization
    void Start () {
		for (int i=0; i<AutoEnabledGO.Length; i++)
        {
            AutoEnabledGO[i].SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
