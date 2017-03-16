using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlscreenscript : MonoBehaviour {
    float timelimit = 2;
    bool ok = false;
    [SerializeField]
    GameObject text;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > timelimit)
        {
            ok = true;
            text.SetActive(true);
        }

        if (ok)
        {
            if (Input.GetKeyDown(KeyCode.Space) ||Input.GetKeyDown(KeyCode.A) ||Input.GetButtonDown("Submit") ||Input.GetButtonDown("Jump"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
            }
        }
    }
}
