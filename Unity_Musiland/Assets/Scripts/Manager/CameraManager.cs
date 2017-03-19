using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraManager : MonoBehaviour {

    Bloom bloom;
    bool isblooming;
    bool isunblooming;
    float bloomduration;
    float bloomstart;
    float bloomdestination;
    float bloomfrom;
    float endtime;

	// Use this for initialization
	void Start () {
        bloom = Camera.main.GetComponent<Bloom>();
	}
	
	// Update is called once per frame
	void Update () {

     
		if (isblooming)
        {
            bloom.bloomIntensity = Mathf.Lerp(bloom.bloomIntensity, bloomdestination, (Time.time - bloomstart) / bloomduration);
          //if (bloom.bloomIntensity == 0.3f) { isblooming = false; bloomstart = 0; bloomdestination = 0; bloomduration = 0; }
        }


	}

    public void SwitchBloomOn(float duration, float to, float howlong = 0)
    {
        isblooming = true;
        bloomstart = Time.time;
        bloomduration = duration;
        bloomdestination = to;
        if (howlong > 0)
        {
            StartCoroutine(BloomMinuteur(howlong));
            //howlong = 0;
        }
    }

    public void SwitchBloomOff(float duration, float to, float howlong = 0)
    {
        bloomstart = Time.time;
        bloomduration = duration;
        bloomdestination = to;
    }



    public void BetaBloomOn(bool onoff, float howlong=0)
    {
        if (onoff)
        {
            
            bloom.bloomIntensity = 0.3f;
            //if (howlong > 0) StartCoroutine(BloomMinuteur(howlong));
        } 
        else
        {
            bloom.bloomIntensity = 0;
            //bloom.enabled = false;
        }
    }

        IEnumerator BloomMinuteur(float howlong)
    {

        yield return new WaitForSeconds(howlong);

        SwitchBloomOn(3, 0);
    }
}
