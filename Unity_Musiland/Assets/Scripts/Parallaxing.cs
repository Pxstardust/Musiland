using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds; // Array des différents layer du parallax
    private float[] parallaxScales; // Proportion du mouvement de cam' pour déplacer le background
    public float smoothing=1f;      // Level de smooth du parallax. ( > 0)

    [SerializeField]
    private Transform cam;          // Main cam
    private Vector3 previousCamPos; // frame prec

    [SerializeField]
    public GameObject player;

    void Awake()
    {
        //cam = Camera.main.transform;
    }


	// Use this for initialization
	void Start () {
        previousCamPos = cam.position;
        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i< backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float backgroundTargetPosX = backgrounds[i].position.x + parallax  ;
            //float backgroundTargetPosY = backgrounds[i].position.y - parallax;
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            print(parallax);
            // fade entre pos et target pos
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
	}
}
