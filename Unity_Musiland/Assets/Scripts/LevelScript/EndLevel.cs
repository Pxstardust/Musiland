    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {

    [SerializeField]
    TriggeredArea Restart;

    [SerializeField]
    TriggeredArea Quit;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if (Restart.IAMTRIGGERED)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
            print("hi");
        }

        if (Quit.IAMTRIGGERED)
        {
            Application.Quit();
        }
    }
}
